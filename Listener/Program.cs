using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Listener
{
    class Program
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

                    // Parse the request URL to get the resource path
                    string resourcePath = context.Request.Url?.AbsolutePath.Trim('/');

                    // Process the request based on the resource path
                    string responseString = string.Empty;
                    if (resourcePath == "MyName")
                    {
                        responseString = GetMyName();
                    }
                    else
                    {
                        responseString = $"Hello, you requested: {context.Request.Url}";
                    }

                    // Send the response
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    context.Response.OutputStream.Close();

                    Console.WriteLine($"Request received: {context.Request.Url}");
                }
                catch (HttpListenerException)
                {
                    // Ignore if the listener is stopped
                }
            }
        }

        private static string GetMyName()
        {
            return "Success!";
        }
    }
}
