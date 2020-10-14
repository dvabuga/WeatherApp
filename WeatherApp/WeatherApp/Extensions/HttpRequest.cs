using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class HttpRequest
{

    public static async Task<JObject> GetJobjectAsync(this HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        var urlContents = await response.Content.ReadAsStringAsync();
        var dataObj = JsonConvert.DeserializeObject<JObject>(urlContents);
        return dataObj;
    }

}