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
    public class PutEndpointTest : IClassFixture<TestingControllerFactory<Startup>>
    {
        private readonly string _uri = "/ZipCode";
        private readonly TestingControllerFactory<Startup> _factory;

        public PutEndpointTest(TestingControllerFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_returnCreatedStatus_when_callingPutZipCodeWithNewId()
        {
            // Expensive to create factory for each test, but it ensures that each test is independent.
            // Arrange
            var client = _factory.CreateClient();

            ZipCodeDto zipCode = new()
            {
                City = "Aarhus",
                ZipCodeValue = "9250",
            };

            // Act
            ByteArrayContent byteContent = CreateContent(zipCode);
            var putWithCreateResponse = await client.PutAsync(_uri + $"?id={1}", byteContent);

            ZipCode? responseContent =
                JsonConvert.DeserializeObject<ZipCode>
                (await putWithCreateResponse.Content.ReadAsStringAsync());
                    int id = responseContent.ZipCodeId;

            // Assert
            putWithCreateResponse.Should().Be(System.Net.HttpStatusCode.Created);

        }

        [Fact]
        public async Task Should_returnOKStatus_when_callingPutZipCodeWithExistingId()
        {
            // Expensive to create factory for each test, but it ensures that each test is independent.
            // Arrange
            var client = _factory.CreateClient();

            ZipCodeDto zipCode = new()
            {
                City = "Aalborg",
                ZipCodeValue = "9000",
            };

            
            var firstResponse = await PutZipCode(zipCode, client, 1);

            ZipCode? responseContent =
                JsonConvert.DeserializeObject<ZipCode>
                    (await firstResponse.Content.ReadAsStringAsync());
            int id = responseContent.ZipCodeId;

            // Act
            ByteArrayContent byteContent = CreateContent(zipCode);

            HttpResponseMessage putWithCreateResponse = await PutZipCode(zipCode, client, id);

            // Assert
            putWithCreateResponse.Should().Be(System.Net.HttpStatusCode.OK);
            
        }

        private async Task<HttpResponseMessage> PutZipCode(ZipCodeDto firstZipCode, HttpClient client, int id)
        {
            ByteArrayContent byteContent = CreateContent(firstZipCode);

            HttpResponseMessage response = await client.PutAsync(_uri + $"?id={id}", byteContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        private static ByteArrayContent CreateContent(ZipCodeDto firstZipCode)
        {
            string? serialized = JsonConvert.SerializeObject(firstZipCode);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(serialized);
            ByteArrayContent byteContent = new(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}