using EmployeeSeriesManagementService.Api.Contracts;
using EmployeeSeriesManagementService.Api.Data;
using EmployeeSeriesManagementService.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSeriesManagementService.Api.Services;

public class SeriesService(EmployeeSeriesDbContext db, ILogger<SeriesService> logger)
    : ISeriesService
{
    public async Task<IReadOnlyList<SeriesDto>?> GetForPeriodAsync(
        int employeeId, DateOnly from, DateOnly to)
    {
        var exists = await db.Employees.AnyAsync(x => x.EmployeeId == employeeId);
        if (!exists) return null;

        //fetching series for employees with in a given time period

        return await db.EmployeeSeries.AsNoTracking()
            .Where(x => x.EmployeeId == employeeId &&
                        x.StartDate <= to &&
                        (x.EndDate == null || x.EndDate >= from))
            .OrderBy(x => x.StartDate)
            .Select(x => new SeriesDto(x.Series.Code, x.Series.ExternalId, x.Series.Name,
                x.StartDate, x.EndDate))
            .ToListAsync();
    }

    public async Task<SeriesDto?> CreateForEmployeeAsync(
        int employeeId, CreateEmployeeSeriesRequest request)
    {
        if (request.AssignmentEndDate is not null &&
            request.AssignmentEndDate < request.AssignmentStartDate)
            throw new ArgumentException("AssignmentEndDate can't be less than AssignmentStartDate.");

        if (request.SeriesEndDate is not null && request.SeriesStartDate is not null &&
            request.SeriesEndDate < request.SeriesStartDate)
            throw new ArgumentException("SeriesEndDate can't be less than SeriesStartDate.");

        var employeeExists = await db.Employees.AnyAsync(x => x.EmployeeId == employeeId);
        if (!employeeExists) return null;

        await using var transaction = await db.Database.BeginTransactionAsync();

        var series = await db.Series.SingleOrDefaultAsync(x => x.Code == request.Code);
        if (series is null)
        {
            series = new Series
            {
                Code = request.Code,
                ExternalId = request.ExternalId,
                Name = request.Name.Trim(),
                StartDate = request.SeriesStartDate,
                EndDate = request.SeriesEndDate
            };
            db.Series.Add(series);
        }
        else if (series.Name != request.Name.Trim())
        {
            throw new ArgumentException(
                $"Series code {request.Code} already exists with a different name.");
        }

        var assignment = new EmployeeSeries
        {
            EmployeeId = employeeId,
            SeriesCode = request.Code,
            StartDate = request.AssignmentStartDate,
            EndDate = request.AssignmentEndDate
        };
        db.EmployeeSeries.Add(assignment);

        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        logger.LogInformation("Series {SeriesCode} assigned to employee {EmployeeId}",
            request.Code, employeeId);

        return new SeriesDto(series.Code, series.ExternalId, series.Name,
            assignment.StartDate, assignment.EndDate);
    }
}
