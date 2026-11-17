using EmployeeSeriesManagementService.Api.Contracts;
using EmployeeSeriesManagementService.Api.Data;
using EmployeeSeriesManagementService.Api.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSeriesManagementService.Api.Services;

public class EmployeeService(EmployeeSeriesDbContext db, ILogger<EmployeeService> logger)
    : IEmployeeService
{
    public async Task<IReadOnlyList<AddressDto>?> GetAddressesAsync(int employeeId)
    {
        //fetching all addresses employeeid wise 
        var exists = await db.Employees.AnyAsync(x => x.EmployeeId == employeeId);
        
       
        if (!exists) return null;

        return await db.EmployeeAddresses.AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Select(x => new AddressDto(x.Address.Id, 
                x.Address.AddressType.Description,
                x.Address.Country,
                x.Address.City, 
                x.Address.ZipCode, 
                x.Address.Street,
                x.Address.Number, 
                x.Address.MailboxNumber, 
                x.Address.Building, 
                x.Address.Floor))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<PersonalAddressDto>> GetPersonalAddressesByWorkCityAsync(
        string workCity)
    {
        var city = workCity.Trim();

        // fetching personal addresss employee woth work address as 'Brusells'

        var query = db.Employees.AsNoTracking()
            .Where(e => e.EmployeeAddresses.Any(a =>
                a.Address.AddressType.Description == "Work" &&
                a.Address.City == city))
            .Select(e => new
            {
                Employee = e,
                Personal = e.EmployeeAddresses
                    .Where(a => a.Address.AddressType.Description == "Personal")
                    .Select(a => a.Address)
                    .FirstOrDefault()
            })
            .Where(x => x.Personal != null);

        var results = await query.Select(x => new PersonalAddressDto(
            x.Employee.EmployeeId, 
            x.Employee.FirstName, 
            x.Employee.LastName,
            x.Employee.EmailAddress,
            new AddressDto(x.Personal!.Id, 
                x.Personal.AddressType.Description,
                x.Personal.Country, 
                x.Personal.City, 
                x.Personal.ZipCode, 
                x.Personal.Street,
                x.Personal.Number, 
                x.Personal.MailboxNumber, 
                x.Personal.Building, 
                x.Personal.Floor)))
            .ToListAsync();

        logger.LogInformation("employees whose work city is Brusells",
            results.Count, city);
        
        return results;
    }

    public async Task<IReadOnlyList<WorkCityDto>> GetAllWorkCityAsync()
    {

        // fetching all distinct work cities

        return await db.EmployeeAddresses
        .AsNoTracking()
        .Where(x =>
            x.Address.AddressType.Description == "Work" &&
            x.Address.City != null &&
            x.Address.City != "")
        .Select(x => x.Address.City)
        .Distinct()
        .OrderBy(city => city)
        .Select(city => new WorkCityDto(city))
        .ToListAsync();
    }
    
}
