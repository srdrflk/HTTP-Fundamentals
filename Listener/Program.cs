using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Listener
{
    public class Program
    {
        private static HttpListener listener;
        private static bool isRunning = true;

        static async Task Main(string[] args)
        {
            // Start the listener
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8888/");
            listener.Start();
            Console.WriteLine("Listener started at http://localhost:8888/");

            // Handle incoming requests
            Task listenTask = HandleIncomingRequests();

            // Wait for the user to type "exit" to stop the listener
            Console.WriteLine("Type 'exit' to stop the listener...");
            while (isRunning)
            {
                string input = Console.ReadLine();
                if (input?.ToLower() == "exit")
                {
                    isRunning = false;
                    listener.Stop();
                    Console.WriteLine("Listener stopped.");
                }
            }

            await listenTask;
        }

        private static async Task HandleIncomingRequests()
        {
            while (isRunning)
            {
                try
                {
                    // Wait for an incoming request
                    HttpListenerContext context = await listener.GetContextAsync();

                    // Process the request
                    string responseString = $"Hello, you requested: {context.Request.Url}";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                    // Send the response
                    context.Response.ContentLength64 = buffer.Length;
                    await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    context.Response.OutputStream.Close();

                    Console.WriteLine($"Request received: {context.Request.Url}");
                }
                catch (HttpListenerException)
                {
                    // ignore for this sample application
                }
            }
        }
    }
}
