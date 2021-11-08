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
        public async Task Should_returnSpecificZipCode_when_callingGetZipCodeWithId()
        {
            // Expensive to create factory for each test, but it ensures that each test is independent.
            // Arrange
            var client = _factory.CreateClient();

            int amountOfZipCodes = 2;
            ZipCodeDto firstZipCode = new()
            {
                City = "Aalborg",
                ZipCodeValue = "9000",
            };
            ZipCodeDto secondZipCode = new()
            {
                City = "Vordingborg",
                ZipCodeValue = "6007",
            };
            
            var firstResponse = await PostZipCode(firstZipCode, client);
            await PostZipCode(secondZipCode, client);

            ZipCode? responseContent =
                JsonConvert.DeserializeObject<ZipCode>
                    (await firstResponse.Content.ReadAsStringAsync());
            int id = responseContent.ZipCodeId;

            // Act
            var specificZipCodeResponse = await client.DeleteAsync(_uri + $"?id={id}");
            
            // Assert
            specificZipCodeResponse.EnsureSuccessStatusCode();
            
            List<ZipCode>? specificZipCode =
                JsonConvert.DeserializeObject<List<ZipCode>>(
                    await specificZipCodeResponse.Content.ReadAsStringAsync());
            specificZipCode.Should().BeNull();
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