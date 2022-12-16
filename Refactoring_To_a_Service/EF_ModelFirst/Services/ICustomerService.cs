using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_ModelFirst.Services
{
    internal interface ICustomerService
    {
        List<Customer> GetCustomerList();
        Customer GetCustomerById(string customerId);
        void CreateCustomer(Customer c);
        void SaveCustomerChanges();
        void RemoveCustomer(Customer c);
    }
}
