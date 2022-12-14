using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EF_ModelFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new SouthwindContext())
            {
                #region Adding customers
                //Customer cust = new Customer();
                //cust.CustomerId = "1";
                //cust.ContactName = "Mo";
                //cust.City = "London";
                //cust.PostalCode= "ab12cd";

                //Customer cust2 = new Customer();
                //cust.CustomerId = "2";
                //cust.ContactName = "Phil";
                //cust.City = "London";
                //cust.PostalCode = "ab34cd";
                
                //Customer cust3 = new Customer();
                //cust.CustomerId = "3";
                //cust.ContactName = "hamza";
                //cust.City = "London";
                //cust.PostalCode = "ab56cd";
                
                //Customer cust4 = new Customer();
                //cust.CustomerId = "4";
                //cust.ContactName = "rida";
                //cust.City = "London";
                //cust.PostalCode = "ab78cd"; 
                
                //Customer cust5 = new Customer();
                //cust.CustomerId = "5";
                //cust.ContactName = "sjad";
                //cust.City = "London";
                //cust.PostalCode = "ab90cd";
                #endregion

                
            }
        }
    }
}
