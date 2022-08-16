using System.Text.Json;
using MyMicro.Dtos;

namespace MyMicro.SyncDS.Http
{
    public class HttpComDataCli : ICommandDataCli
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpComDataCli(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            
            var respond = await _httpClient.PostAsync(
                $"{_configuration["CommandsServ"]}",
                httpContent 
            );

            if(respond.IsSuccessStatusCode)
                Console.WriteLine("--> Sync Post to ComData OK");
            else
                Console.WriteLine("--> Sync Post to ComData ERROR");
        }
    }
}