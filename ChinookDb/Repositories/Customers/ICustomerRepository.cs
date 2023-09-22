using ChinookDb.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.Repositories.Customers
{
    internal interface ICustomerRepository : ICrudRepository<Customer, int>
    {
        List<Customer> GetByName(string name);
        List<CustomerGenre> GetMostPopularGenreForCustomer(int customerId);
        List<CustomerSpender> GetTopCustomerSpenders();
        List<CustomerCountry> GetCustomerCountByCountries();
        List<Customer> ReturnPageOfCustomersByOffsetAndLimit(int offset, int limit);

        bool UpdateAnExistingCustomersLastName(int customerId, string oldLastName, string newLastName);
    }
}
