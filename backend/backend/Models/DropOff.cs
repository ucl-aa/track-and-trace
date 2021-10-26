using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class DropOff
    {
        [Key]
        public int Key { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public ZipCode ZipCode { get; set; }

        public int Terminal { get; set; }
    }
}