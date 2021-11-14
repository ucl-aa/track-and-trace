using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Backend;
using Backend.DataTransferObjects;
using Backend.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTest.ZipCodeTests
{
    public class DeleteEndpointTest : IClassFixture<TestingControllerFactory<Startup>>
    {
        private readonly string _uri = "/ZipCode";
        private readonly TestingControllerFactory<Startup> _factory;

        public DeleteEndpointTest(TestingControllerFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_returnNoContent_when_callingDeleteZipCodeWithId()
        {
            // Expensive to create factory for each test, but it ensures that each test is independent.
            // Arrange
            var client = _factory.CreateClient();

            ZipCodeDto zipCode = new()
            {
                City = "Aalborg",
                ZipCodeValue = "9000",
            };
            
            var firstResponse = await PostZipCode(zipCode, client);

            ZipCode? responseContent =
                JsonConvert.DeserializeObject<ZipCode>
                    (await firstResponse.Content.ReadAsStringAsync());
            int id = responseContent.ZipCodeId;

            // Act
            HttpResponseMessage specificZipCodeResponse = await client.DeleteAsync(_uri + $"?id={id}");

            // Assert
            specificZipCodeResponse.Should().NotBeNull().And.Be(System.Net.HttpStatusCode.NoContent);

            List<ZipCode>? specificZipCode =
                JsonConvert.DeserializeObject<List<ZipCode>>(
                    await specificZipCodeResponse.Content.ReadAsStringAsync());
            specificZipCode.Should().BeNull();
        }

        [Fact]
        public async Task Should_returnNotFound_when_callingDeleteZipCodeWithId()
        {
            //Arrange 
            var client = _factory.CreateClient();

            //Act
            HttpResponseMessage specificZipCodeResponse = await client.DeleteAsync(_uri + $"?id={1}");

            // Assert
            specificZipCodeResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task<HttpResponseMessage> PostZipCode(ZipCodeDto firstZipCode, HttpClient client)
        {
            string? serialized = JsonConvert.SerializeObject(firstZipCode);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(serialized);
            ByteArrayContent byteContent = new(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            HttpResponseMessage response = await client.PostAsync(_uri, byteContent);
            response.EnsureSuccessStatusCode();
            
            return response;
        }
    }
}