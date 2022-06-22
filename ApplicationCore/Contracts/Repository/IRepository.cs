using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GeyById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Updated(T entity);
        Task<T> Delete(T entity);

    }
}
