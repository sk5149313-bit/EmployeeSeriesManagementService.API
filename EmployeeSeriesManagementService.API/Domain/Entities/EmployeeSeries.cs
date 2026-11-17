namespace EmployeeSeriesManagementService.Api.Domain.Entities;

// The dates belong to the employee-to-series assignment, not just to Series.
public class EmployeeSeries
{
    public int EmployeeId { get; set; }
    public int SeriesCode { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public Employee Employee { get; set; } = null!;
    public Series Series { get; set; } = null!;
}
