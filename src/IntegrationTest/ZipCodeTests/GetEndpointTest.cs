using System.Net.Http;
using System.Threading.Tasks;
using Backend;
using Backend.DataTransferObjects;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTest.ZipCodeTests
{
    public class GetEndpointTest : IClassFixture<TestingControllerFactory<Startup>>
    {
        private readonly string _uri = "/ZipCode";
        private readonly TestingControllerFactory<Startup> _factory;

        public GetEndpointTest(TestingControllerFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_returnSuccess_when_callingGetZipCode()
        {
            //Expensive to create new client for each test,
            //but ensures that tests aren't dependant on each other.
            HttpClient? client = _factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync(_uri);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Should_returnSpecificZipCode_when_callingGetZipCodeWithId()
        {
            var client = _factory.CreateClient();

            ZipCodeDto firstZipCode = new()
            {
                City = "Aalborg",
                ZipCodeValue = "9000",
            };

            var serialized = JsonConvert.SerializeObject(firstZipCode);
            var buffer = System.Text.Encoding.UTF8.GetBytes(serialized);
            var byteContent = new ByteArrayContent(buffer);
            var firstResponse = await client.PostAsync(_uri, byteContent);
            
            ZipCodeDto secondZipCode = new()
            {
                City = "Vordingborg",
                ZipCodeValue = "6007",
            };
            serialized = JsonConvert.SerializeObject(secondZipCode);
            buffer = System.Text.Encoding.UTF8.GetBytes(serialized);
            byteContent = new ByteArrayContent(buffer);
            var secondResponse = await client.PostAsync(_uri, byteContent);
        }
        
    }
}