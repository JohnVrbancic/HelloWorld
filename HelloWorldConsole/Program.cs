using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HelloWorldRunner
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            return CreateHostBuilder(args).Build().RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration(async (hostingContext, configuration) =>
             {
                 configuration.Sources.Clear();

                 IHostEnvironment env = hostingContext.HostingEnvironment;

                 configuration
                     .AddJsonFile("appsettings.json", false, true)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                 IConfigurationRoot configurationRoot = configuration.Build();

                 bool allowConsole = configurationRoot.GetValue<bool>("WriteToConsole");
                 bool allowDatabase = configurationRoot.GetValue<bool>("WriteToDatabase");

                 if (allowConsole)
                 {
                     var message = await CallHelloWorldService();

                     Console.WriteLine(message);
                 }

                 if (allowDatabase)
                 {
                     //throw new NotImplementedException();
                     Console.WriteLine("Database writing Not Implemented");
                 }
             });

        static async Task<string> CallHelloWorldService()
        {
            string  message=null;

            client.BaseAddress = new Uri("http://localhost:63851/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(
                "/helloworld");
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                message = await response.Content.ReadAsStringAsync();
            }

            return message;
        }
    }
}
