using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using OnlineBookStore.Services;
using System.Text.Json;

namespace OnlineBookStore.Integration.Tests
{
    public class CalculateShoppingCartTotalValueTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CalculateShoppingCartTotalValueTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task CalculateTotal_ShouldReturnTotalAmount()
        {
            // Arrange
            long shoppingCartId = 1;

            // Create an HttpRequestMessage for a GET request
            var request = new HttpRequestMessage(HttpMethod.Get, $"/checkout/{shoppingCartId}/total");

            // Optionally, add headers if needed (e.g., Authorization)
            request.Headers.Add("Accept", "application/json");

            // Act: send the request using SendAsync
            var response = await _client.SendAsync(request);

            // Ensure the response status code is successful
            response.EnsureSuccessStatusCode();

            // Read the response body as a string
            var responseString = await response.Content.ReadAsStringAsync();

            // Deserialize the response into the ServiceResponse<string> object
            var actualResponse = JsonSerializer.Deserialize<ServiceResponse<string>>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert: Verify that the response is not null and matches the expected result
            Assert.NotNull(actualResponse);
            Assert.Equal("Total calculated", actualResponse.Message);
        }
    }
}
