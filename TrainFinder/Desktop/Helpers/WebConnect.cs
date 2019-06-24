using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop
{
    class WebConnect
    {
        //connection safty required

        private const string Url = "https://trainfinder.azurewebsites.net/Api/";
        //private const string Url = "http://localhost:11835/Api/";

        public static async Task<string> GetData(string rout)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    return await client.GetStringAsync(rout);
                }
                catch (Exception e)
                {
                    if (e.InnerException != null && (e.InnerException.GetType() == typeof(WebException)))
                    {
                        ConnectionFailed(e.Message);
                        throw new HttpRequestException();
                    }

                    throw;
                }
            }
        }

        public static HttpResponseMessage PostData(string rout, object data)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client.PostAsJsonAsync(rout, data).Result;
            }
        }

        public static HttpResponseMessage UpdateDate(string rout, object data)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client.PutAsJsonAsync(rout, data).Result;
            }
        }

        public static HttpResponseMessage DeleteData(string rout)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client.DeleteAsync(rout).Result;
            }
        }

        public static async void CheckConnection()
        {
            try
            {
                while (true)
                {
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol =
                            SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        client.BaseAddress = new Uri(Url);
                        await client.GetStringAsync("Route/Test");
                    }
                    await Task.Delay(10000);
                }
            }
            catch (Exception e)
            {
                ConnectionFailed(e.Message);
            }
        }

        private static void ConnectionFailed(string msg)
        {
            //var data = Thread.GetDomain()
            //Debug.WriteLine(data.);
            //Debug.WriteLine(11111111111111111);
            Debug.WriteLine(Thread.CurrentThread.Name);
            DialogDisplayHelper.DisplayMessageBox(msg + "\n Retry Again", "Error", MessageBoxButton.YesNo,
                MessageBoxImage.Stop);
        }
    }
}