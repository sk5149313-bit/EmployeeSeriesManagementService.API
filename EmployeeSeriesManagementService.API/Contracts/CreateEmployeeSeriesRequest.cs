using System.ComponentModel.DataAnnotations;

namespace EmployeeSeriesManagementService.Api.Contracts;

public class CreateEmployeeSeriesRequest
{
    [Range(1, int.MaxValue)]
    public int Code { get; init; }

    public int? ExternalId { get; init; }

    [Required, StringLength(255, MinimumLength = 1)]
    public string Name { get; init; } = "";

    public DateOnly? SeriesStartDate { get; init; }
    public DateOnly? SeriesEndDate { get; init; }

    [Required]
    public DateOnly AssignmentStartDate { get; init; }

    public DateOnly? AssignmentEndDate { get; init; }
}
