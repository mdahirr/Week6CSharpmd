﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_ModelFirst.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly SouthwindContext _context;

        public CustomerService()
        {
            _context = new SouthwindContext();
        }
        public void CreateCustomer(Customer c)
        {
            _context.Customers.Add(c);
            _context.SaveChanges();
        }

        public Customer GetCustomerById(string customerId)
        {
            return _context.Customers.Find(customerId);
        }

        public List<Customer> GetCustomerList()
        {
            return _context.Customers.ToList();
        }

        public void RemoveCustomer(Customer c)
        {
            _context.Customers.Remove(c);
            _context.SaveChanges();
        }

        public void SaveCustomerChanges()
        {
            _context.SaveChanges();
        }
    }
}
