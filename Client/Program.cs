using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using HttpClient client = new HttpClient();

        Console.WriteLine("Client started. Sending requests...");

        // URLs to request
        string[] urls = {
            "http://localhost:8888/MyName/",
            "http://localhost:8888/Information/",
            "http://localhost:8888/Success/",
            "http://localhost:8888/Redirection/",
            "http://localhost:8888/ClientError/",
            "http://localhost:8888/ServerError/"
        };

        // Send requests and display responses
        foreach (var url in urls)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"URL: {url} - Status Code: {(int)response.StatusCode} ({response.StatusCode}) - Response: {responseBody}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request to {url} failed: {ex.Message}");
            }
        }

        Console.WriteLine("Client stopped.");
    }
}