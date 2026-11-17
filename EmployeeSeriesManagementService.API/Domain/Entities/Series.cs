using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EmployeeSeriesManagementService.Api.Domain.Entities;

public class Series
{
   [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Code { get; set; }
    public int? ExternalId { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ICollection<EmployeeSeries> EmployeeSeries { get; set; } = [];
}
