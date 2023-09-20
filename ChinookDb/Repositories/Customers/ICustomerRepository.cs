using ChinookDb.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.Repositories.Customers
{
    internal interface ICustomerRepository : ICrudRepository<CustomerStruct, int>
    {

    }
}
