using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IRepository<T> where T : class
    {
        T GeyById(int id);
        IEnumerable<T> GetAll();
        T Add(T entity);
        T Updated(T entity);
        T Delete(T entity);

    }
}
