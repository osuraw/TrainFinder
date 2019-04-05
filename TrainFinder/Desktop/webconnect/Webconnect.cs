using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http.Formatting;
using Desktop.Database;

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
        public static void CreateWeb(string rout,object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11835/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync(rout, data).Result;
            }
        }
    }
}
