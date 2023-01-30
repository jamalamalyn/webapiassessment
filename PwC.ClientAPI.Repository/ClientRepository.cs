using Microsoft.EntityFrameworkCore.Query.Internal;
using PwC.ClientAPI.Domain;
using PwC.ClientAPI.Domain.Interfaces;
using PwC.ClientAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PwC.ClientAPI.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository, IDisposable
    {
        public ClientRepository(DataContext dataContext) : base(dataContext) {}

        public void DeleteRange(IEnumerable<int> ids)
        {
            var entities = _dataContext.Clients.Where(c => ids.Contains(c.Id));
            _dataContext.RemoveRange(entities);
            _dataContext.SaveChanges();
        }

        public IEnumerable<Client> GetRange(IEnumerable<int> ids)
        {
            return _dataContext.Clients.Where(c => ids.Contains(c.Id));
        }

        public void UpdateClient(Client client)
        {
            var entity = _dataContext.Clients.Find(client.Id);
            if(entity != null)
            {
                entity.UpdateDate = DateTime.Now;
                entity.RegisteredDateTime = client.RegisteredDateTime;
                entity.Name = client.Name;
                entity.EmailAddress = client.EmailAddress;
                _dataContext.SaveChanges();
            }
        }

        public void UpdateClients(IEnumerable<Client> clients)
        {
            foreach(var client in clients) 
            {
                var entity = _dataContext.Clients.Find(client.Id);

                if (entity != null)
                {
                    entity.UpdateDate = DateTime.Now;
                    entity.RegisteredDateTime = client.RegisteredDateTime;
                    entity.Name = client.Name;
                    entity.EmailAddress = client.EmailAddress;
                    _dataContext.SaveChanges();
                }
            }
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        // Used just for testing purposes
        public void DeleteDatabase()
        {
            _dataContext.Database.EnsureDeleted();
        }
    }
}
