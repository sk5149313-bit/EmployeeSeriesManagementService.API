namespace EmployeeSeriesManagementService.Api.Domain.Entities;

public  class EmployeeIdCard
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Number { get; set; } = null!;
    public DateOnly? ValidityDate { get; set; }
    public Employee Employee { get; set; } = null!;
}
