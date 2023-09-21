using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.DataAccess.Models
{
    internal readonly record struct Customer (int id, string FirstName, string LastName, string? PostalCode, string? Country, string? Phone, string Email);
}
