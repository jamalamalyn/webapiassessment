using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PwC.ClientAPI.Domain.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }

        [JsonIgnore]
        public DateTime CreateDate { get; set; }

        [JsonIgnore]
        public DateTime UpdateDate { get; set; }
    }
}
