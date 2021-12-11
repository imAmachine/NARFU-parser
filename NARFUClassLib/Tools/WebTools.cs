using System.Net;

namespace NARFUClassLib
{
    public static class WebTools
    {
        public static string? GetHtmlPage(Uri url)
        {
            HttpClient client = new() { BaseAddress = url };
            HttpRequestMessage request = new(HttpMethod.Get, client.BaseAddress);

            if (client.Send(request).StatusCode == HttpStatusCode.OK)
                using (StreamReader sr = new(client.GetStreamAsync(url).Result))
                    return sr.ReadToEnd();
            else
                return null;
        }
    }
}
