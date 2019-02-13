using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsApp.API.Models;

namespace ContactsApp.API.Data
{
    public interface IRepo<T>
    {
        Task Insert(T entity);
        void Delete(T entity);
         Task Update(T entity);
        Task<bool> SaveAll();
        Task<T> Get(int id );
        IQueryable<T> GetAll();  
    }
}