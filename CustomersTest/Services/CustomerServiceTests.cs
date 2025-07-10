using Core.Models;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SalesOrderApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfrastructureTest.Services
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private AppDbContext _dbContext;
        private CustomerService _customerService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AppDbContext(options);
            _customerService = new CustomerService(_dbContext);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Customer { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
            };

            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task GetAllAsync_Return2()
        {
            var customers = await _customerService.GetAllAsync();

            Assert.That(customers.Count(), Is.EqualTo(2));
        }
    }
}