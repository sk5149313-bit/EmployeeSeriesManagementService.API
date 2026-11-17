namespace EmployeeSeriesManagementService.Api.Domain.Entities;

// ERD junction table: one employee can have personal and work address links;
// a shared work Address can be linked to many employees.
public  class EmployeeAddress
{
    public int EmployeeId { get; set; }
    public int AddressId { get; set; }
    public Employee Employee { get; set; } = null!;
    public Address Address { get; set; } = null!;
}
