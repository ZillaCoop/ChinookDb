using ChinookDb.DataAccess;
using ChinookDb.DataAccess.Models;
using ChinookDb.Repositories.Customers;

namespace ChinookDb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomerRepository customerRepository = new CustomerRepository();
            //customerRepository.GetAllCustomers().ForEach(c => Console.WriteLine(c));
            //customerRepository.ReadCustomerById(1).ForEach(c => Console.WriteLine(c));
            //customerRepository.ReadCustomersByName("Jon").ForEach(c => Console.WriteLine(c));
            //customerRepository.ReturnPageOfCustomersByOffsetAndLimit(5, 10).ForEach(c => Console.WriteLine(c));

            //Customer customer = new Customer(0, "John", "Travolta IV", "337-336", "Denmark", "98374839", "notjohn@travolta.com");
            //customerRepository.AddCustomer(customer);
            //customerRepository.ReadCustomersByName("Travolta").ForEach(c => Console.WriteLine(c));

            //customerRepository.UpdateAnExistingCustomersLastName(60, "Travolta IV", "Travolta III");
            //customerRepository.ReadCustomersByName("Travolta").ForEach(c => Console.WriteLine(c));

            //customerRepository.GetCustomerCountByCountries().ForEach(c => Console.WriteLine(c));
            //customerRepository.GetTopCustomerSpenders().ForEach(c => Console.WriteLine(c));

            customerRepository.GetMostPopularGenreForCustomer(10).ForEach(c => Console.WriteLine(c));

        }
    }
}