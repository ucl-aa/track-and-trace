using System;
using System.Threading.Tasks;
using Backend;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTest.ZipCodeTests
{
    public class GetEndpointTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly string _uri = "/ZipCode";
        private readonly WebApplicationFactory<Startup> _factory;

        public GetEndpointTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_returnSuccess_when_callingGetZipCode()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(_uri);

            response.EnsureSuccessStatusCode();
        }
    }
}