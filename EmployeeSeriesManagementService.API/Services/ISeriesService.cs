using EmployeeSeriesManagementService.Api.Contracts;

namespace EmployeeSeriesManagementService.Api.Services;

public interface ISeriesService
{
    Task<IReadOnlyList<SeriesDto>?> GetForPeriodAsync(int employeeId, DateOnly from, DateOnly to);
    Task<SeriesDto?> CreateForEmployeeAsync(int employeeId, CreateEmployeeSeriesRequest request);
}
