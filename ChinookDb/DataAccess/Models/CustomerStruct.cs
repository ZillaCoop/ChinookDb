using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.DataAccess.Models
{
    internal readonly struct CustomerStruct
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PostalCode { get; }
        public string Country { get; }
        public string Phone { get; }
        public string Email { get; }

        public CustomerStruct(int id, string firstName, string lastName, string postalCode, string country, string phone, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PostalCode = postalCode;
            Country = country;
            Phone = phone;
            Email = email;
        }

        public override bool Equals(object? obj)
        {
            return obj is CustomerStruct customer &&
                   Id == customer.Id &&
                   FirstName == customer.FirstName &&
                   LastName == customer.LastName &&
                   PostalCode == customer.PostalCode &&
                   Country == customer.Country &&
                   Phone == customer.Phone &&
                   Email == customer.Email;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, PostalCode, Country, Phone, Email);
        }

        public override string? ToString()
        {
            return $"Id={Id}, FirstName={FirstName}, LastName={LastName}, PostalCode={PostalCode}, Country={Country}, Phone={Phone}, Email={Email}";
        }
    }
}
