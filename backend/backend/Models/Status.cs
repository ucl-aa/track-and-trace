using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Message { get; set; }
    }
}