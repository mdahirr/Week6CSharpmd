using EF_ModelFirst;

namespace EF_ModelFirstTests
{
    public class Tests
    {
        CustomerManager _customerManager;
        [SetUp]
        public void Setup()
        {
            // remove test entry in DB if present
            using (var db = new SouthwindContext())
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
        public void WhenANewCustomerIsAdded_TheNumberOfCustomersIncreasesBy1()
        {
            using (var db = new SouthwindContext())
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
            Assert.Fail();
        }

        [Test]
        public void WhenRetrievingCustomers_CustomerListIsRightLength()
        {
            Assert.Fail();
        }

        [Test]
        public void WhenRetrievingACustomer_ThatCustomerHasRightDetails()
        {
            Assert.Fail();
        }

        [Test]
        public void WhenACustomerIsUpdated_TheTableDetailsUpdated()
        {
            Assert.Fail();
        }

        [Test]
        public void WhenACustomerIsNotInTheDatabase_Update_ReturnsFalse()
        {
            Assert.Fail();
        }

        [Test]
        public void WhenACustomerIsRemoved_TheNumberOfCustomersDecreasesBy1()
        {
            Assert.Fail();
        }

        [Test]
        public void WhenACustomerIsRemoved_TheyAreNoLongerInTheDatabase()
        {
            Assert.Fail();
        }

        [Test]
        public void WhenACustomerIsNotInTheDatabase_Remove_ReturnsFalse()
        {
            Assert.Fail();
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new SouthwindContext())
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