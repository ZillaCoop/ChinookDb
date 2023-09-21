using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.DataAccess.Models
{
    internal readonly record struct CustomerGenre(int CustomerId, string GenreName, int TrackCount);
}
