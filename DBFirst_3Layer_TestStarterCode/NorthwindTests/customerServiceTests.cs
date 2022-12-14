using Microsoft.EntityFrameworkCore;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;
using System.Linq;

namespace NorthwindTests
{
    internal class CustomerServiceTests
    {
        private CustomerService _sut;
        private NorthwindContext _context;



        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase("Example_DB")
                .Options;
            _context = new NorthwindContext(options);
            _sut = new CustomerService(_context);



            _context.Customers.Add(
                new Customer
                {
                    CustomerId = "WINDR",
                    ContactName = "Philip Windridge",
                    CompanyName = "Sparta Global",
                    City = "Birmingham"
                });
            _context.Customers.Add(
                new Customer
                {
                    CustomerId = "TOZER",
                    ContactName = "Laura Tozer",
                    CompanyName = "Sparta Global",
                    City = "London"
                });
            _context.SaveChanges();
        }

        [Test]
        public void GivenAValidId_GetCustomerById_ReturnsTheCorrectCustomer()
        {
            var result = _sut.GetCustomerById("TOZER");
            Assert.That(result, Is.TypeOf<Customer>());
            Assert.That(result.ContactName, Is.EqualTo("Laura Tozer"));
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
            Assert.That(result.City, Is.EqualTo("London"));
        }

        [Test]
        public void GivenANewCustomer_CreateCustomer_AddsItToTheDatabase()
        {
            // Arrange
            var numberOfCustomersBefore = _context.Customers.Count();
            var newCustomer = new Customer
            {
                CustomerId = "BEAR",
                ContactName = "Martin Beard",
                CompanyName = "Sparta Global",
                City = "Rome"
            };



            // Act
            _sut.CreateCustomer(newCustomer);
            var numberOfCustomersAfter = _context.Customers.Count();
            var result = _sut.GetCustomerById("BEAR");



            // Assert
            Assert.That(numberOfCustomersBefore + 1, Is.EqualTo(numberOfCustomersAfter));



            Assert.That(result, Is.TypeOf<Customer>());
            Assert.That(result.ContactName, Is.EqualTo("Martin Beard"));
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
            Assert.That(result.City, Is.EqualTo("Rome"));



            // Clean up
            _context.Customers.Remove(newCustomer);
            _context.SaveChanges();
        }

        [Test]
        public void WhenACustomerIsDeleted_RemoveCustomer_DeletesTheCustomerFromTheDatabase()
        {
            //arrange
            var numberOfCustomersBefore = _context.Customers.Count();
            var customer = _context.Customers.First();
            //act
            _sut.RemoveCustomer(customer);
            var numberofCustomersAfter = _context.Customers.Count();

            //Assert
            Assert.That(numberOfCustomersBefore - 1, Is.EqualTo(numberofCustomersAfter));

            _context.Customers.Add(customer);
        }

        [Test]
        public void When_GetCustomerList_isUsed_ItReturnsAList()
        {
            var newList = _sut.GetCustomerList();

            Assert.That(newList, Is.TypeOf<Customer>());
        }
    }
}