namespace EmployeeSeriesManagementService.Api.Domain.Entities;

public  class Employee
{
    public int EmployeeId { get; set; }
    public string? UserId { get; set; }
    public int? Number { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? SecondName { get; set; }
    public string Language { get; set; } = "FR";
    public DateOnly? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public byte[]? ProfileImage { get; set; }
    public string? Nationality { get; set; }
    public DateOnly? ExitDate { get; set; }
    public string? EmailAddress { get; set; }
    public int? OrganizationalUnit { get; set; }
    public string? PhoneNumber { get; set; }

    public ICollection<EmployeeAddress> EmployeeAddresses { get; set; } = [];
    public ICollection<EmployeeSeries> EmployeeSeries { get; set; } = [];
    public ICollection<EmployeeIdCard> IdCards { get; set; } = [];
}
