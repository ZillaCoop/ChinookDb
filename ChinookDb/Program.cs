using ChinookDb.DataAccess;
using ChinookDb.DataAccess.Models;

namespace ChinookDb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChinookDAO chinookDAO = new ChinookDAO();
            //chinookDAO.ReadAllCustomers().ForEach(c => Console.WriteLine(c));
            //chinookDAO.ReadCustomerById(1).ForEach(c => Console.WriteLine(c));
            //chinookDAO.ReadCustomersByName("Jon").ForEach(c => Console.WriteLine(c));
            //chinookDAO.ReturnPageOfCustomersByOffsetAndLimit(5, 10).ForEach(c => Console.WriteLine(c));

            
            //Customer customer = new Customer(0, "John", "Travolta IV", "337-336", "Denmark", "98374839", "notjohn@travolta.com");
            //chinookDAO.AddNewCustomer(customer);
            //chinookDAO.ReadCustomersByName("Travolta").ForEach(c => Console.WriteLine(c));
            

            //chinookDAO.UpdateAnExistingCustomersLastName(60, "Travolta IV", "Travolta III");
            //chinookDAO.ReadCustomersByName("Travolta").ForEach(c => Console.WriteLine(c));

            //chinookDAO.GetCustomerCountByCountries().ForEach(c => Console.WriteLine(c));
            //chinookDAO.GetTopCustomerSpenders().ForEach(c => Console.WriteLine(c));

            chinookDAO.GetMostPopularGenreForCustomer(10).ForEach(c => Console.WriteLine(c));

        }
    }
}