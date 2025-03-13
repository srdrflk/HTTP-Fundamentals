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

                    // Process the request on a separate task to avoid blocking
                    _ = Task.Run(() => ProcessRequest(context));
                }
                catch (HttpListenerException)
                {
                    // Ignore if the listener is stopped
                }
            }
        }

        private static void ProcessRequest(HttpListenerContext context)
        {
            // Parse the request URL to get the resource path
            string resourcePath = context.Request.Url?.AbsolutePath.Trim('/');

            // Process the request based on the resource path
            string responseString = string.Empty;
            switch (resourcePath)
            {
                case "MyName":
                    responseString = GetMyName();
                    context.Response.StatusCode = (int)HttpStatusCode.OK; // 200 OK
                    break;
                case "Information":
                    context.Response.StatusCode = (int)HttpStatusCode.Continue; // 100 Continue
                    responseString = "Information - 100 Continue";
                    break;
                case "Success":
                    context.Response.StatusCode = (int)HttpStatusCode.OK; // 200 OK
                    responseString = "Success - 200 OK";
                    break;
                case "Redirection":
                    context.Response.StatusCode = (int)HttpStatusCode.MultipleChoices; // 300 Multiple Choices
                    responseString = "Redirection - 300 Multiple Choices";
                    break;
                case "ClientError":
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400 Bad Request
                    responseString = "Client Error - 400 Bad Request";
                    break;
                case "ServerError":
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500 Internal Server Error
                    responseString = "Server Error - 500 Internal Server Error";
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404 Not Found
                    responseString = "Not Found - 404";
                    break;
            }

            // Send the response
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();

            Console.WriteLine($"Request received: {context.Request.Url} - Status Code: {context.Response.StatusCode}");
        }

        private static string GetMyName()
        {
            return "Success!";
        }
    }
}
