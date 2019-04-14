using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Desktop.webconnect
{
    class Webconnect
    {
        //connection safty required
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
        public static HttpResponseMessage PostData(string rout,object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11835/Api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client.PostAsJsonAsync(rout, data).Result;
            }
        }

        public static string GetData(string rout)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11835/Api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client.GetStringAsync(rout).Result;
            }
        }

        public static HttpResponseMessage DeleteData(string rout)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11835/Api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client.DeleteAsync(rout).Result;
            }
        }
    }
}
