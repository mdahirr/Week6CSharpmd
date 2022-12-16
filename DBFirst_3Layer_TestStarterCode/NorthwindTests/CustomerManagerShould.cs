using NUnit.Framework;
using Moq;
using NorthwindBusiness;
using NorthwindData;
using NorthwindData.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace NorthwindTests
{
    public class CustomerManagerShould
    {
        private CustomerManager _sut;

        [Test]
        public void BeAbleToBeConstructedUsingMoq()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            // Act
            _sut = new CustomerManager(mockCustomerService.Object);
            // Assert
            Assert.That(_sut, Is.InstanceOf<CustomerManager>());
        }

        [Test]
        public void WhenCalledWithValidId_Update_ReturnsTrue()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer
            {
                CustomerId = "ROCK"
            };
            mockCustomerService.Setup(
                cs => cs.GetCustomerById("ROCK"))
                    .Returns(originalCustomer);



            _sut = new CustomerManager(mockCustomerService.Object);



            // Act
            var result = _sut.Update(
                "ROCK", It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void WhenCalledWithValidIdAndValues_Update_CorrectlyChangesValues()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer()
            {
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"



            };
            mockCustomerService.Setup(
                cs => cs.GetCustomerById("ROCK"))
                    .Returns(originalCustomer);
            _sut = new CustomerManager(mockCustomerService.Object);

            // Act
            _sut.Update("ROCK", "Rocky Raccoon", "UK", "Chester", null);



            // Assert
            Assert.That(
                _sut.SelectedCustomer.ContactName,
                Is.EqualTo("Rocky Raccoon"));
            Assert.That(
                _sut.SelectedCustomer.CompanyName,
                Is.EqualTo("Zoo UK"));
            Assert.That(
                _sut.SelectedCustomer.Country,
                Is.EqualTo("UK"));
            Assert.That(
                _sut.SelectedCustomer.City,
                Is.EqualTo("Chester"));
        }

        [Test]
        public void WhenCalledWithInvalidId_Update_ReturnsFalse()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();



            mockCustomerService.Setup(
                cs => cs.GetCustomerById("ROCK"))
                    .Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Update(
                "ROCK", It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>());



            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void When_Delete_IsCalledTheCorrectValueIsRemoved()
        {
            //arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer()
            {
                CustomerId = "ROCK",
            };
            mockCustomerService.Setup(
                cs => cs.GetCustomerById("ROCK"))
                .Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);
            //act
            var result = _sut.Delete(
                "ROCK");

            //Assert
            Assert.That(result, Is.False);
        }

        #region stock answers ------------------------

        [Test]
        public void ReturnTrue_WhenDeleteIsCalledWithValidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var customer = new Customer()
            {
                CustomerId = "ROCK",
            };
            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(customer);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Delete("ROCK");



            // Assert
            Assert.That(result);
        }



        [Test]
        public void SetSelectedCustomerToNull_WhenDeleteIsCalledWithValidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var customer = new Customer()
            {
                CustomerId = "ROCK",
            };
            _sut.SelectedCustomer = customer;
            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(customer);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Delete("ROCK");



            // Assert
            Assert.That(_sut.SelectedCustomer, Is.Null);
        }

        [Test]
        public void ReturnFalse_WhenDeleteIsCalled_WithInvalidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();



            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Delete("ROCK");



            // Assert
            Assert.That(result, Is.False);
        }



        [Test]
        public void NotChangeTheSelectedCustomer_WhenDeleteIsCalled_WithInvalidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();



            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);



            var originalCustomer = new Customer()
            {
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"



            };



            _sut = new CustomerManager(mockCustomerService.Object);
            _sut.SelectedCustomer = originalCustomer;
            // Act
            _sut.Delete("ROCK");



            // Assert that SelectedCustomer is unchanged
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Rocky Raccoon"));
            Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Zoo UK"));
            Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo(null));
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Telford"));
        }
        #endregion

        [Test]
        public void WhenADatabaseExceptionIsThrownWhileUpdating_Update_DoesNotChangeSelectedCustomer()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();



            mockCustomerService.Setup(
                cs => cs.GetCustomerById(
                    It.IsAny<string>()))
                .Returns(new Customer());



            mockCustomerService.Setup(
                cs => cs.SaveCustomerChanges())
                .Throws<DbUpdateConcurrencyException>();



            var originalCustomer = new Customer()
            {
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"
            };



            _sut = new CustomerManager(mockCustomerService.Object);
            _sut.SelectedCustomer = originalCustomer;
            // Act
            _sut.Update("ROCK", "Rocky Raccoon", "UK", "Chester", null);



            // Assert
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Rocky Raccoon"));
            Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Zoo UK"));
            Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo(null));
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Telford"));
        }

        [Test]
        public void WhenUpdateIsCalledWithValidId_SaveCustomerChanges_IsCalledOnce()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(
                cs => cs.GetCustomerById("ROCK"))
                    .Returns(new Customer());



            _sut = new CustomerManager(mockCustomerService.Object);



            // Act
            var result = _sut.Update(
                "ROCK", It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>());



            // Assert
            mockCustomerService.Verify(
                cs => cs.SaveCustomerChanges(),
                Times.Once);
        }

        [Test]
        public void LetsSeeWhatHappens_WhenUpdateIsCalled_IfAllInvocationsArentSetUp()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>(MockBehavior.Strict);
            mockCustomerService.Setup(
                cs => cs.GetCustomerById(It.IsAny<string>()))
                .Returns(new Customer());
            mockCustomerService.Setup(cs => cs.SaveCustomerChanges());
            _sut = new CustomerManager(mockCustomerService.Object);



            // Act
            var result = _sut.Update(
                "ROCK", It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>());



            // Assert
            Assert.That(result);
        }

        [Test]
        public void When_RetrieveAll_IsCalledTheCorrectValuesAreRetrieved()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(
                cs => cs.GetCustomerList())
                        .Returns(new List<Customer>() );

            _sut = new CustomerManager(mockCustomerService.Object);

            // Act
            var result = _sut.RetrieveAll();
                

            // Assert
            mockCustomerService.Verify(cs => cs.GetCustomerList(), Times.Once());
        }
        
    }
}