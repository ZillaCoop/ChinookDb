﻿using ChinookDb.DataAccess;

namespace ChinookDb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChinookDAO chinookDAO = new ChinookDAO();
            //chinookDAO.ReadAllCustomers().ForEach(c => Console.WriteLine(c));
            chinookDAO.GetCustomerById(1).ForEach(c => Console.WriteLine(c));
        }
    }
}