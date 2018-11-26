using System;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace DotNetCoreStreaming.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                var requestUri = "http://localhost:60596/api/Cliente/Streaming";
                var stream = httpClient.GetStreamAsync(requestUri).Result;

                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        Console.WriteLine(reader.ReadLine());
                    }
                }
            }
        }
    }
}
