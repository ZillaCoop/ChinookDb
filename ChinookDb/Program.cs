using ChinookDb.DataAccess;

namespace ChinookDb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChinookDAO chinookDAO = new ChinookDAO();
            //chinookDAO.ReadAllCustomers().ForEach(c => Console.WriteLine(c));
            //chinookDAO.ReadCustomerById(1).ForEach(c => Console.WriteLine(c));
            chinookDAO.ReadCustomersByName("Jon").ForEach(c => Console.WriteLine(c));

        }
    }
}