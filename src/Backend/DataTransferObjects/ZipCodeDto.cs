using Backend.Models;

namespace Backend.DataTransferObjects
{
    public class ZipCodeDto
    {
        public string City { get; set; }

        public string ZipCodeValue { get; set; }

        public ZipCode GetZipCode()
        {
            return new ZipCode
            {
                City = City,
                ZipCodeValue = ZipCodeValue,
            };
        }
    }
}