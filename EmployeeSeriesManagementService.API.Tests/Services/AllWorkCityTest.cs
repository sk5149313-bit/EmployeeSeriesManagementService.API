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

        //test case to validate city exists or not
        [Fact]
        public async Task GetAllWorkCityAsync_WhenCityDoesNotExist_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<EmployeeSeriesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var dbContext = new EmployeeSeriesDbContext(options);
            var logger = new Mock<ILogger<EmployeeService>>();
            var employeeService = new EmployeeService(dbContext, logger.Object);
            var result = await employeeService.GetAllWorkCityAsync();
            Assert.Empty(result);
        }
       
        }
    }
