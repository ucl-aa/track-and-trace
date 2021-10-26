using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Delivery
    {
        [Key]
        public int Id { get; init; }

        public string TracingId { get; set; }

        public DateTime DeliveryDate { get; set; }

        public List<Status> StatusHistory { get; set; }

        public DropOff DropOff { get; set; }
    }
}