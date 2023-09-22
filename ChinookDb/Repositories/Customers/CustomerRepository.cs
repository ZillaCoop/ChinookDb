using ChinookDb.DataAccess.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.Repositories.Customers
{
    internal class CustomerRepository : ICustomerRepository
    {

        private static string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "N-NO-01-01-4419\\SQLEXPRESS";
            builder.InitialCatalog = "Chinook";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            return builder.ConnectionString;
        }




        /// <summary>
        /// Retrieves a list of countries and the number of customers from each country.
        /// </summary>
        /// <returns>A list of customer counts grouped by country.</returns>
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
        public List<CustomerCountry> GetCustomerCountByCountries()
        {
            List<CustomerCountry> customerCountries = new List<CustomerCountry>();
            string sql = "SELECT Country, Count(CustomerId) as CustomerCount FROM Customer " +
                "GROUP BY Country ORDER BY CustomerCount DESC";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customerCountries.Add(new CustomerCountry(
                    reader.GetString(0),
                    reader.GetInt32(1)
                    ));
            }

            return customerCountries;
        }





        /// <summary>
        /// Retrieves the most popular genre, or two most popular genres in case of a tie, for a specific customer based on their purchases.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>A list of genres with their purchase counts.</returns>
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
        public List<CustomerGenre> GetMostPopularGenreForCustomer(int customerId)
        {
            List<CustomerGenre> topGenres = new List<CustomerGenre>();
            //To connect customer to genre, four joins were used to connect them by their foreign keys
            string sql =
                "SELECT C.CustomerId, G.Name as GenreName, COUNT(T.TrackId) as TrackCount " +
                "FROM Customer C " +
                "JOIN Invoice I ON C.CustomerId = I.CustomerId " +
                "JOIN InvoiceLine IL ON I.InvoiceId = IL.InvoiceId " +
                "JOIN Track T on T.TrackId = IL.TrackId " +
                "JOIN Genre G on G.GenreId = T.GenreId " +
                "WHERE C.CustomerId = @CustomerId " +
                "GROUP BY C.CustomerId, G.Name " +
                "ORDER BY TrackCount DESC";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);

            SqlDataReader reader = cmd.ExecuteReader();

            int highestTrackCount = -1; // default value that will be overridden by the first read 


            while (reader.Read())
            {
                int currentTrackCount = reader.GetInt32(2);

                // If it's the first read, or the track count matches the highest count, add to the list
                if (highestTrackCount == -1 || currentTrackCount == highestTrackCount)
                {
                    topGenres.Add(new CustomerGenre(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        currentTrackCount
                    ));

                    if (highestTrackCount == -1)
                    {
                        highestTrackCount = currentTrackCount; // Set the highest track count from the first read
                    }
                }
                else
                {
                    break;
                }
            }

            return topGenres;

        }




        /// <summary>
        /// Retrieves a list of customers and their total spending in descending order.
        /// </summary>
        /// <returns>A list of customers with their spending details.</returns
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
        public List<CustomerSpender> GetTopCustomerSpenders()
        {
            List<CustomerSpender> topCustomerSpenders = new List<CustomerSpender>();
            //Left join in case customer has not spent money
            string sql = "SELECT C.CustomerId, C.FirstName, C.LastName, ISNULL(SUM(I.Total), 0) as TotalSpent " +
                "FROM Customer C " +
                "LEFT JOIN Invoice I ON C.CustomerId = I.CustomerId " +
                "GROUP BY C.CustomerId, C.FirstName, C.LastName " +
                "ORDER BY TotalSpent DESC";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                topCustomerSpenders.Add(new CustomerSpender(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDecimal(3)
                    ));
            }

            return topCustomerSpenders;
        }






        /// <summary>
        /// Returns list of customers based on a specified offset and limit.
        /// </summary>
        /// <param name="offset">The offset from which to start the list.</param>
        /// <param name="limit">The number of customers to return.</param>
        /// <returns>A list of customers.</returns>
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
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



        /// <summary>
        /// Updates the last name of an existing customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="oldLastName">The old last name of the customer.</param>
        /// <param name="newLastName">The new last name to set for the customer.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
        public bool UpdateAnExistingCustomersLastName(int customerId, string oldLastName, string newLastName)
        {
            string sql = "UPDATE Customer SET LastName = @NewLastName WHERE CustomerId = @CustomerId AND LastName = @OldLastName";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);
            cmd.Parameters.AddWithValue("@NewLastName", newLastName);
            cmd.Parameters.AddWithValue("@OldLastName", oldLastName);

            int result = cmd.ExecuteNonQuery();

            return result > 0;
        }



        /// <summary>
        /// Adds a new customer to the database.
        /// </summary>
        /// <param name="obj">The customer to be added.</param>
        /// <returns>True if the the adding was successful, otherwise false.</returns>
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
        public bool AddCustomer(Customer obj)
        {
            string sql = "INSERT INTO Customer (FirstName, LastName, PostalCode, Country, Phone, Email)" +
                "VALUES (@FirstName, @LastName, @PostalCode, @Country, @Phone, @Email)";
            using SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FirstName", obj.FirstName);
            cmd.Parameters.AddWithValue("@LastName", obj.LastName);
            cmd.Parameters.AddWithValue("@PostalCode", obj.PostalCode);
            cmd.Parameters.AddWithValue("@Country", obj.Country);
            cmd.Parameters.AddWithValue("@Phone", obj.Phone);
            cmd.Parameters.AddWithValue("@Email", obj.Email);

            int result = cmd.ExecuteNonQuery();

            return result > 0;
        }





        /// <summary>
        /// Retrieves all customers from the database.
        /// </summary>
        /// <returns>A list of all customers.</returns>
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
        public List<Customer> GetAllCustomers()
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




        /// <summary>
        /// Retrieves a specific customer from the database by their ID.
        /// </summary>
        /// <param name="CustomerId">The ID of the customer to retrieve.</param>
        /// <returns>A list containing the customer, or an empty list if not found.</returns>
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
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



        /// <summary>
        /// Retrieves customers from the database based on a name search.
        /// </summary>
        /// <param name="name">The name or part of the name to search for.</param>
        /// <returns>A list of customers that match the search.</returns>
        /// <exception cref="SqlException">Thrown when there's an issue executing the SQL query or connecting to the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the SqlDataReader operation is invalid.</exception>
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



        bool ICrudRepository<Customer, int>.Update(Customer obj)
        {
            throw new NotImplementedException();
        }

        bool ICrudRepository<Customer, int>.Delete(Customer obj)
        {
            throw new NotImplementedException();
        }

        bool ICrudRepository<Customer, int>.DeleteById(Customer obj)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Add(Customer obj)
        {
            throw new NotImplementedException();
        }
    }
}

