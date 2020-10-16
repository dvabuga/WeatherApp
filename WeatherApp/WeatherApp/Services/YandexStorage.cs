using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WeatherApp.Files.SharedModels;
using WeatherApp.Services.Interfaces;
using YandexDisk.Client.Http;

namespace WeatherApp.Services
{
    public class YandexStorage : IStorageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherServiceSettings _configuration;
        public YandexStorage(IHttpClientFactory httpClientFactory, IOptions<WeatherServiceSettings> configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration.Value;
        }


        public async Task Upload(Stream file)
        {
            var oauthToken = _configuration.OauthToken;
            var diskApi = new DiskHttpApi(oauthToken);
            var uploadUrl = await diskApi.Files.GetUploadLinkAsync("/Files/faults1.xlsx", true, CancellationToken.None);
            await diskApi.Files.UploadAsync(uploadUrl, file);
        }
    }
}