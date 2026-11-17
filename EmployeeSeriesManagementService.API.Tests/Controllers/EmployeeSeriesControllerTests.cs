
using EmployeeSeriesManagementService.Api.Controllers;
using EmployeeSeriesManagementService.Api.Contracts;
using EmployeeSeriesManagementService.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EmployeeSeriesManagementService.Api.Tests.Controllers;

public class EmployeeSeriesControllerTests
{
    private readonly Mock<ISeriesService> _seriesServiceMock;
    private readonly EmployeeSeriesController _controller;

    public EmployeeSeriesControllerTests()
    {
        _seriesServiceMock = new Mock<ISeriesService>();

        _controller = new EmployeeSeriesController(
            _seriesServiceMock.Object);
    }

    private static CreateEmployeeSeriesRequest GetValidRequest()
    {
        return new CreateEmployeeSeriesRequest
        {
            Code = 101,
            ExternalId = 5001,
            Name = "Travel Series",
            SeriesStartDate = new DateOnly(2026, 1, 1),
            SeriesEndDate = new DateOnly(2026, 12, 31),
            AssignmentStartDate = new DateOnly(2026, 2, 1),
            AssignmentEndDate = new DateOnly(2026, 12, 31)
        };
    }

    //test case to validate valid request
    [Fact]
    public async Task Create_WhenValidRequest_ReturnsCreatedAtAction()
    {
        
        int employeeId = 1;

        var request = GetValidRequest();

        var expectedResponse = new SeriesDto(
            Code: 101,
            ExternalId: 5001,
            Name: "Travel Series",
            AssignmentStartDate: new DateOnly(2026, 2, 1),
            AssignmentEndDate: new DateOnly(2026, 12, 31)
        );

        _seriesServiceMock
            .Setup(x => x.CreateForEmployeeAsync(employeeId, request))
            .ReturnsAsync(expectedResponse);

        
        var result = await _controller.Create(employeeId, request);

        
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        Assert.Equal(201, createdResult.StatusCode);

        Assert.Equal(
            nameof(EmployeeSeriesController.GetForPeriod),
            createdResult.ActionName);

        var actualResponse = Assert.IsType<SeriesDto>(
            createdResult.Value);

        Assert.Equal(101, actualResponse.Code);
        Assert.Equal("Travel Series", actualResponse.Name);
        Assert.Equal(5001, actualResponse.ExternalId);
    }

    //test case to check employee exist or not
    [Fact]
    public async Task Create_WhenEmployeeDoesNotExist_ReturnsNotFound()
    {
        
        int employeeId = 999;
        var request = GetValidRequest();

        _seriesServiceMock
            .Setup(x => x.CreateForEmployeeAsync(employeeId, request))
            .ReturnsAsync((SeriesDto?)null);

        
        var result = await _controller.Create(employeeId, request);

        
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(404, notFoundResult.StatusCode);

        _seriesServiceMock.Verify(
            x => x.CreateForEmployeeAsync(employeeId, request),
            Times.Once);
    }


    //negative testing for exception
    [Fact]
    public async Task Create_WhenServiceThrowsException_ReturnsBadRequest()
    {

        int employeeId = 1;
        var request = GetValidRequest();

        _seriesServiceMock
            .Setup(x => x.CreateForEmployeeAsync(employeeId, request))
            .ThrowsAsync(new Exception("Something went wrong"));


        var result = await _controller.Create(employeeId, request);


        var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(400, badRequest.StatusCode);
    }

    //on calling of endpoint 
    [Fact]
    public async Task Create_WhenCalled_InvokesServiceOnce()
    {

        int employeeId = 1;
        var request = GetValidRequest();

        var expectedResponse = new SeriesDto(
            101,
            5001,
            "Travel Series",
            new DateOnly(2026, 2, 1),
            new DateOnly(2026, 12, 31));

        _seriesServiceMock
            .Setup(x => x.CreateForEmployeeAsync(employeeId, request))
            .ReturnsAsync(expectedResponse);


        await _controller.Create(employeeId, request);


        _seriesServiceMock.Verify(
            x => x.CreateForEmployeeAsync(
                employeeId,
                It.Is<CreateEmployeeSeriesRequest>(r =>
                    r.Code == 101 &&
                    r.Name == "Travel Series")),
            Times.Once);
    }
}
