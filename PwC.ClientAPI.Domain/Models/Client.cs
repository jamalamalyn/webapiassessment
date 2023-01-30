using System;
using System.ComponentModel.DataAnnotations;

namespace PwC.ClientAPI.Domain.Models
{
    public class Client : Entity
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public DateTime RegisteredDateTime { get; set; }
    }
}
