using ChinookDb.DataAccess.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.DataAccess
{
    internal class ChinookDAO
    {
        public ChinookDAO()
        {
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
        }

        private static string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "N-NO-01-01-4419\\SQLEXPRESS";
            builder.InitialCatalog = "Chinook";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            return builder.ConnectionString;
        }

        public List<Customer> ReadAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            string sql = "SELECT CustomerId, FirstName, LastName, PostalCode, Country, Phone, Email FROM Customer";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new Customer(
                    reader.GetInt32(0), 
                    reader.GetString(1), 
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.IsDBNull(5) ? null : reader.GetString(5), 
                    reader.GetString(6)));
            }
            return customers;
        }

        public List<Customer> ReadCustomerById(int CustomerId)
        {
            List<Customer> customers = new List<Customer>();
            string sql = "SELECT CustomerId, FirstName, LastName, PostalCode, Country, Phone, Email " +
                "FROM Customer WHERE CustomerId = @CustomerId";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.IsDBNull(5) ? null : reader.GetString(5),
                    reader.GetString(6)));
            }
            return customers;
        }

        public List<Customer> ReadCustomersByName(string name)
        {
            List<Customer> customers = new List<Customer>();
            string sql = "SELECT CustomerId, FirstName, LastName, PostalCode, Country, Phone, Email " +
                "FROM Customer WHERE FirstName LIKE @Name OR LastName LIKE @Name";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", "%" + name + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.IsDBNull(5) ? null : reader.GetString(5),
                    reader.GetString(6)));
            }
            return customers;
        }

        public List<Customer> ReturnPageOfCustomersByOffsetAndLimit(int offset, int limit)
        {
            List<Customer> customers = new List<Customer>();
            string sql = "SELECT CustomerId, FirstName, LastName, PostalCode, Country, Phone, Email FROM Customer " +
                "ORDER BY CustomerID OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Offset", offset);
            cmd.Parameters.AddWithValue("@Limit", limit);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.IsDBNull(5) ? null : reader.GetString(5),
                    reader.GetString(6)));
            }
            return customers;
        }

        public bool AddNewCustomer(Customer customer)
        {
            string sql = "INSERT INTO Customer (FirstName, LastName, PostalCode, Country, Phone, Email)" +
                "VALUES (@FirstName, @LastName, @PostalCode, @Country, @Phone, @Email)";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand( sql, conn);
            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
            cmd.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
            cmd.Parameters.AddWithValue("@Country", customer.Country);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
            cmd.Parameters.AddWithValue("@Email", customer.Email);

            int result = cmd.ExecuteNonQuery();
            
            return result > 0;

        }



    }
}
