using EmployeeSeriesManagementService.Api.Data;
using EmployeeSeriesManagementService.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeSeriesManagementService.API.Tests.Services
{
    public class AllWorkCityTest
    {


        [Fact]
        public async Task GetAllWorkCityAsync_WhenCityDoesNotExist_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EmployeeSeriesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new EmployeeSeriesDbContext(options);

            var logger = new Mock<ILogger<EmployeeService>>();

            var employeeService = new EmployeeService(dbContext, logger.Object);

            // Act
            var result = await employeeService.GetAllWorkCityAsync();

            // Assert
            Assert.Empty(result);
        }
       
        }
    }
