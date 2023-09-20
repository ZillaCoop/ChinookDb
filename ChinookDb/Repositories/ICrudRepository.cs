using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDb.Repositories
{
    internal interface ICrudRepository<T, TKey>
    {
        List<T> GetAll();
        List<T> GetById(TKey id);

        bool Add(T obj);
        bool Update(T obj);
        bool Delete(T obj);
        bool DeleteById(T obj);
    }
}
