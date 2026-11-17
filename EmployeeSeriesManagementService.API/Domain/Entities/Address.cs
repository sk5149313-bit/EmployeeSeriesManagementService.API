namespace EmployeeSeriesManagementService.Api.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public int AddressTypeId { get; set; }
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? ZipCode { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? MailboxNumber { get; set; }
    public string? Building { get; set; }
    public string? Floor { get; set; }

    public AddressType AddressType { get; set; } = null!;
    public ICollection<EmployeeAddress> EmployeeAddresses { get; set; } = [];
}
