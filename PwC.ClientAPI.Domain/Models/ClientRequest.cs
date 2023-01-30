using System;
using System.Collections.Generic;

namespace PwC.ClientAPI.Domain.Models
{
    public class ClientRequest
    {
        public IEnumerable<ClientRequestObject> Clients { get; set; }
    }

    public class ClientRequestObject
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public DateTime RegisteredDateTime { get; set; }
    }
}
