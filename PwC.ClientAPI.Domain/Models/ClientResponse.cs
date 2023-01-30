using System;
using System.Collections.Generic;

namespace PwC.ClientAPI.Domain.Models
{
    public class ClientResponse
    {
        public IEnumerable<ClientResponseObject> Clients { get; set; }
    }

    public class ClientResponseObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public DateTime RegisteredDateTime { get; set; }
    }
}
