using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Order
    {
        [Key]
        public int Id { get; init; }

        public List<Status> StatusHistory { get; set; }

        public string TracingId { get; init; }
    }
}