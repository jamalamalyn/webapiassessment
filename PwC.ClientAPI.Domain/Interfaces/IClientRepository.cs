using PwC.ClientAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.ClientAPI.Domain.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        public void DeleteRange(IEnumerable<int> ids);

        public IEnumerable<Client> GetRange(IEnumerable<int> ids);

        public void UpdateClient(Client client);

        public void UpdateClients(IEnumerable<Client> client);
    }
}
