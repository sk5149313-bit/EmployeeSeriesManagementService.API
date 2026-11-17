namespace EmployeeSeriesManagementService.Api.Domain.Entities;

public class AddressType
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public ICollection<Address> Addresses { get; set; } = [];
}
