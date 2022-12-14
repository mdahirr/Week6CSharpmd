using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_ModelFirst
{
    internal class CustomerManager
    {
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

        public void Create(Customer newCustomer)
        {
            using (var db = new SouthwindContext())
            {
                db.Customers.Add(newCustomer);
                db.SaveChanges();
            }
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
    }
}
