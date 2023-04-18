using Newtonsoft.Json;

namespace OAuthDemo.Tests.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<T> DeserializeContentAsync<T>(this HttpResponseMessage responseMessage)
    {
        var content = await responseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content)!;
    }
}