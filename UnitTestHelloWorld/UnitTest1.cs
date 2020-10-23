using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UnitTestHelloWorld
{
    [TestClass]
    public class HelloWorldTest
    {
        static HttpClient client = new HttpClient();

        [TestMethod]
        public async Task HelloworldAsync()
        {
            var message = await CallHelloWorldService();

            message = message.Trim(new[] {'"'});

            Assert.AreEqual("Hello World",message);
        }

        static async Task<string> CallHelloWorldService()
        {
            string message = null;

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