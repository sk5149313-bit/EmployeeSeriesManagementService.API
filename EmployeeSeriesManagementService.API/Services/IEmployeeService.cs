using EmployeeSeriesManagementService.Api.Contracts;

namespace EmployeeSeriesManagementService.Api.Services;

public interface IEmployeeService
{
    Task<IReadOnlyList<AddressDto>?> GetAddressesAsync(int employeeId);
    Task<IReadOnlyList<PersonalAddressDto>> GetPersonalAddressesByWorkCityAsync(string workCity);
    Task<IReadOnlyList<WorkCityDto>> GetAllWorkCityAsync();
    
}
