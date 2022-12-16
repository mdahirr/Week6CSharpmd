using EF_ModelFirst.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EF_ModelFirst
{
    public class CustomerManager
    {
        private ICustomerService _service;

        public CustomerManager()
        {
            _service = new CustomerService();
        }

        public CustomerManager(ICustomerService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("ICustomerService object cannot be null");
            }
            _service = service;
        }
        public Customer SelectedCustomer { get; set; }

        public void SetSelectedCustomer(object selectedItem)
        {
            SelectedCustomer = (Customer)selectedItem;
        }

        public List<Customer> RetrieveAll() 
        {
            return _service.GetCustomerList();
        }

        public void Read()
        {
            using (var db = new SouthwindContext()) 
            {
                foreach (var customer in db.Customers)
                {
                    Console.WriteLine($"ID: {customer.CustomerId}, Contact name: {customer.ContactName}, City: {customer.City}");
                }
            }
        }

        public void Create(string customerId, string contactName, string city, string postalCode, string country)
        {
            _service.CreateCustomer(
                 new Customer()
                 {
                     CustomerId = customerId,
                     ContactName = contactName,
                     City = city,
                     PostalCode = postalCode,
                     Country = country
                 }
                 );
        }

        public void Update(Customer updatedCustomer)
        {
            using (var db = new SouthwindContext())
            {
                var selectedCustomer = db.Customers.Find(updatedCustomer.CustomerId);
                selectedCustomer.ContactName = updatedCustomer.ContactName;
                selectedCustomer.City = updatedCustomer.City;
                selectedCustomer.PostalCode = updatedCustomer.PostalCode;
                db.SaveChanges();
            }
        }

        public void Delete(Customer customer) 
        {
            using (var db = new SouthwindContext())
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }
        }

        public void Save()
        {
            _service.SaveCustomerChanges(
                new Customer()
                {

                });
        }
    }
}
