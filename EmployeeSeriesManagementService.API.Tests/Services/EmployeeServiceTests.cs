
using EmployeeSeriesManagementService.Api.Data;
using EmployeeSeriesManagementService.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EmployeeSeriesManagementService.Api.Tests.Services;

public class EmployeeServiceTests
{
    // test case for employee not exists to get address
    [Fact]
    public async Task GetAddressesAsync_WhenEmployeeDoesNotExist_ReturnsNull()
    {
        var options = new DbContextOptionsBuilder<EmployeeSeriesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var dbContext = new EmployeeSeriesDbContext(options);

        var logger = new Mock<ILogger<EmployeeService>>();

        var employeeService = new EmployeeService(dbContext, logger.Object);

        int employeeId = 999;
        var result = await employeeService.GetAddressesAsync(employeeId);
        
        Assert.Null(result);

     
    }
}
