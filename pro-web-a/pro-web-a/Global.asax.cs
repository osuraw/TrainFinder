using System.Web.Http;
using AutoMapper;
using Microsoft.Azure.Storage.File;
using pro_web_a.Helpers;

namespace pro_web_a
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(c=>c.AddProfile<MappingProfile>());
            FileHandler.CreateFileShared();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
