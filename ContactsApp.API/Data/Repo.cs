using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Data
{
    public class Repo<T> : IRepo<T> where T : Entity, new()
    {
        DataContext _context;
        public Repo(DataContext context){
           _context = context;
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync( e => e.Id == id);
        }

        public  IQueryable<T> GetAll()
        {
            IQueryable<T> entities =   _context.Set<T>().AsNoTracking();
            
            return entities;
        }

        public async Task Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveAll()
        {
            return  await _context.SaveChangesAsync() > 0 ;
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}