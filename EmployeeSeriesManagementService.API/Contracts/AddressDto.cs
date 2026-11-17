namespace EmployeeSeriesManagementService.Api.Contracts;

public record AddressDto(
    int Id, 
    string Type,
    string Country,
    string City, 
    string? ZipCode,
    string? Street,
    string? Number, 
    string? MailboxNumber,
    string? Building, 
    string? Floor
    );
