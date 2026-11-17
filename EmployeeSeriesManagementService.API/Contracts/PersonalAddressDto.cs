namespace EmployeeSeriesManagementService.Api.Contracts;

public record PersonalAddressDto(
    int EmployeeId,
    string FirstName,
    string LastName,
    string? EmailAddress,
    AddressDto PersonalAddress
    );
