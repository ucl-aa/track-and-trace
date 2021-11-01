using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class ZipCode
    {
        [Key]
        public int ZipCodeId { get; set; }

        public string City { get; set; }

        public string ZipCodeValue { get; set; }
    }
}