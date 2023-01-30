using PwC.ClientAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.ClientAPI.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public void Add(T entity);

        public void AddRange(IEnumerable<T> entities);

        public IEnumerable<T> GetAll();

        public T Get(object id);

        public void Delete(object id);

        public void DeleteRange(IEnumerable<object> entities);
    }
}
