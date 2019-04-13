using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Desktop.webconnect
{
    class Webconnect
    {
        private static HttpClient _client;
        private static void Client()
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:11835/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client=client;
            }
        }
        public static HttpResponseMessage ParssData(string rout,object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11835/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client.PostAsJsonAsync(rout, data).Result;
            }
        }
    }
}
