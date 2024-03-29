using NUnit.Framework;
using NorthwindBusiness;
using NorthwindData;
using System.Linq;

namespace NorthwindTests
{
    public class CustomerTests
    {
        CustomerManager _customerManager;
        [SetUp]
        public void Setup()
        {
            _customerManager = new CustomerManager();
            // remove test entry in DB if present
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenANewCustomerIsAddedToTheDatabaseUsingCreate_TheNumberOfCustomersIncreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                var numberOfCustomersBefore = db.Customers.Count();
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                var numberOfCustomersAfter = db.Customers.Count();



                Assert.That(numberOfCustomersBefore + 1, Is.EqualTo(numberOfCustomersAfter));
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheirDetailsAreCorrect()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                var selectedCustomer = db.Customers.Find("MANDA");
                Assert.That(selectedCustomer.ContactName, Is.EqualTo("Nish Mandal"));
                Assert.That(selectedCustomer.CompanyName, Is.EqualTo("Sparta Global"));
            }
        }

        [Test]
        public void WhenACustomersDetailsAreChanged_TheDatabaseIsUpdatedWithTheCorrectDetails()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                _customerManager.Update("MANDA", "Phil Windridge", "France", "Paris", "ab12cd");
                var selectedCustomer = db.Customers.Find("MANDA");
                Assert.That(selectedCustomer.ContactName, Is.EqualTo("Phil Windridge"));
                Assert.That(selectedCustomer.Country, Is.EqualTo("France"));
                Assert.That(selectedCustomer.City, Is.EqualTo("Paris"));
                Assert.That(selectedCustomer.PostalCode, Is.EqualTo("ab12cd"));
            }
        }

        [Test]
        public void WhenACustomerIsUpdated_SelectedCustomerIsUpdated()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                _customerManager.Update("MANDA", "Nish Mandal", "UK", "London", "e123fd");
                var selectedCustomer = db.Customers.Find("MANDA");
                Assert.That(selectedCustomer.Country, Is.EqualTo("UK"));
                Assert.That(selectedCustomer.City, Is.EqualTo("London"));
            }
            
        }

        [Test]
        public void WhenACustomerIsNotInTheDatabase_Update_ReturnsFalse()
        {
            var result = _customerManager.Update("MANDA", contactName: "Nish Mandal", country: "UK", city: "Birmingham", postcode: "AB2 2DE");
            Assert.That(result, Is.False);
        }

        [Test]
        public void WhenACustomerIsRemoved_TheNumberOfCustomersDecreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                var numberOfCustomerBefore = db.Customers.Count();
                _customerManager.Delete("MANDA");
                var numberOfCustomerAfter = db.Customers.Count();
                Assert.That(numberOfCustomerBefore - 1, Is.EqualTo(numberOfCustomerAfter));
            }
        }

        [Test]
        public void WhenACustomerIsRemovedUsingDelete_TheyAreNoLongerInTheDatabase()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Delete("MANDA");
                var deletedCustomer = db.Customers.Find("MANDA");
                Assert.That(deletedCustomer, Is.Null);
            }
            
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }
    }
}