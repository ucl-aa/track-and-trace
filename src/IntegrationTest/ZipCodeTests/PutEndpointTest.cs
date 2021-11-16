using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Backend;
using Backend.DataTransferObjects;
using Backend.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            int wantedId = 1;
            ZipCodeDto zipCode = new()
            {
                City = "Aarhus",
                ZipCodeValue = "9250",
            };

            // Act
            ByteArrayContent byteContent = CreateContent(zipCode);
            var putWithCreateResponse = await client.PutAsync(_uri + $"?id={wantedId}", byteContent);

            ZipCode? responseContent =
                JsonConvert.DeserializeObject<ZipCode>
                (await putWithCreateResponse.Content.ReadAsStringAsync()); 
            int id = responseContent.ZipCodeId;

            // Assert
            putWithCreateResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            id.Should().Be(wantedId);

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
            int wantedId = 1;

            var firstResponse = await PutZipCode(zipCode, client, wantedId);

            ZipCode? responseContent =
                JsonConvert.DeserializeObject<ZipCode>
                    (await firstResponse.Content.ReadAsStringAsync());
            int id = responseContent.ZipCodeId;

            // Act
            HttpResponseMessage putWithCreateResponse = await PutZipCode(zipCode, client, id);

            // Assert
            putWithCreateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            id.Should().Be(wantedId);
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