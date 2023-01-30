using PwC.ClientAPI.Domain;
using PwC.ClientAPI.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PwC.ClientAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DataContext _dataContext;
        public Repository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }
        public void Add(T entity)
        {
            if(entity != null)
            {
                _dataContext.Add(entity);
                _dataContext.SaveChanges();
            }
        }

        public void AddRange(IEnumerable<T> entities)
        {
            if (entities != null && entities.Any())
            {
                _dataContext.AddRange(entities);
                _dataContext.SaveChanges();
            }
        }

        public IEnumerable<T> GetAll()
        {
            return _dataContext.Set<T>();
        }

        public T Get(object id)
        {
            return _dataContext.Set<T>().Find(id);
        }

        public void Delete(object id)
        {
            T entity = _dataContext.Set<T>().Find(id);
            if (entity != null)
            {
                _dataContext.Remove(entity);
                _dataContext.SaveChanges();
            }
        }

        public void DeleteRange(IEnumerable<object> entities)
        {
            _dataContext.RemoveRange(entities);
            _dataContext.SaveChanges();
        }
    }
}
