using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.DataAccess.Models
{
    internal readonly record struct CustomerCountry(string Country, int CustomerCount);
}
