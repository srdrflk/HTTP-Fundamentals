using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using HttpClient client = new HttpClient();

        Console.WriteLine("Client started. Press any key to send a request or 'exit' to quit.");

        while (true)
        {
            string input = Console.ReadLine();
            if (input?.ToLower() == "exit")
            {
                break;
            }

            try
            {
                // Send a GET request to the listener
                HttpResponseMessage response = await client.GetAsync("http://localhost:8888/");
                response.EnsureSuccessStatusCode(); // Throw an exception if the request fails

                // Read and display the response
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseBody}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
            }
        }

        Console.WriteLine("Client stopped.");
    }
}