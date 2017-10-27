using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SimpleApiWithRedis.Tests
{
    public class TodoControllerTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public TodoControllerTest()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task Create_Todo_Test()
        {
            var formData = new Dictionary<string, string>()
            {
                { "todo", "Study REDIS on .NET Core."}
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "api/todo")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            var response = await _client.SendAsync(request);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Update_Todo_Test()
        {
            var formData = new Dictionary<string, string>()
            {
                { "todo", "Study REDIS on .NET Core v2."}
            };

            var request = new HttpRequestMessage(HttpMethod.Put, "api/todo/GUID")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            var response = await _client.SendAsync(request);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_Todo_Test()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/todo/dd2fec73-435d-4530-aa31-18b791a1b0fe");

            var response = await _client.SendAsync(request);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Get_Todo_Test()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/todo/GUID");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<string>(content);

            Assert.Equal("Study REDIS on .NET Core.", result);
        }
    }
}
