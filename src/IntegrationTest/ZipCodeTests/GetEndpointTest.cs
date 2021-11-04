using System.Threading.Tasks;
using Backend;
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
            var client = _factory.CreateClient();

            var response = await client.GetAsync(_uri);

            response.EnsureSuccessStatusCode();
        }
    }
}