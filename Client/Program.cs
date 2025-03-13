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
            "http://localhost:8888/MyNameByHeader/",
            "http://localhost:8888/MyNameByCookies/",
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

                // Display status code and response body
                Console.WriteLine($"URL: {url} - Status Code: {(int)response.StatusCode} ({response.StatusCode}) - Response: {responseBody}");

                // For Task 3: Check if the URL is /MyNameByHeader/ and display the custom header
                if (url.EndsWith("MyNameByHeader/"))
                {
                    if (response.Headers.TryGetValues("X-MyName", out var headerValues))
                    {
                        Console.WriteLine($"Header 'X-MyName': {string.Join(", ", headerValues)}");
                    }
                    else
                    {
                        Console.WriteLine("Header 'X-MyName' not found.");
                    }
                }

                // For Task 4: Check if the URL is /MyNameByCookies/ and display the cookie
                if (url.EndsWith("MyNameByCookies/"))
                {
                    if (response.Headers.TryGetValues("Set-Cookie", out var cookieValues))
                    {
                        foreach (var cookie in cookieValues)
                        {
                            if (cookie.StartsWith("MyName="))
                            {
                                Console.WriteLine($"Cookie 'MyName': {cookie.Split('=')[1].Split(';')[0]}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cookie 'MyName' not found.");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request to {url} failed: {ex.Message}");
            }
        }

        Console.WriteLine("Client stopped.");
    }
}