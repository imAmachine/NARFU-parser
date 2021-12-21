using System.Net;

namespace NARFUClassLib.Tools
{
    public static class WebTools
    {
        public static string? GetHtmlPage(Uri url)
        {
            HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
            using (HttpClient client = new(handler) { BaseAddress = url }) {
                try
                {
                    HttpRequestMessage request = new(HttpMethod.Get, client.BaseAddress);
                    request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
                    
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStringAsync().Result;
                } catch 
                {
                    return null;
                }
            }
        }
    }
}
