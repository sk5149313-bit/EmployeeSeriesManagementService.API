namespace EmployeeSeriesManagementService.Api.Contracts;

public record SeriesDto(
    int Code,
    int? ExternalId, 
    string Name,
    DateOnly AssignmentStartDate,
    DateOnly? AssignmentEndDate
    );
