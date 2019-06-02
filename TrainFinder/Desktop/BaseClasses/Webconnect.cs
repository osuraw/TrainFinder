using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Desktop
{ 
    class WebConnect
    {
        //connection safty required
       

        public static HttpResponseMessage PostData(string rout,object data)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://trainfinder.azurewebsites.net/Api/");
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

        public static HttpResponseMessage UpdateDate(string rout,object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:11835/Api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client.PutAsJsonAsync(rout,data).Result;
            }
        }
    }
}
