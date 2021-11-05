using Backend.Models;

namespace Backend.DataTransferObjects
{
    public class DropOffDto
    {
        public string Name { get; set; }

        public string Street { get; set; }

        public ZipCode ZipCode { get; set; }

        public int Terminal { get; set; }
    }
}