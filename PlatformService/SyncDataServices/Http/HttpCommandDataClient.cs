using PlatformService.Dtos;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                   JsonSerializer.Serialize(plat),
                   Encoding.UTF8,
                   "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode) Console.Out.WriteLine("--> Sync post to command service was ok!");
            else Console.Out.WriteLine("--> Sync post to command service was NOT ok!");
        }
    }
}
