using System.Net.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pro_web_a.Controllers;

namespace WebApiTest
{
    [TestClass]
    public class LocationTest
    {
        [TestMethod]
        public void SetLocation_ByDevice_OKString()
        {
            var locationController = new LocationsController();
            var result = locationController.SetLocation(1,"1,1,20190629023015.000,6.848852,79.862513,9.200,35.85,170.5,1,,1.0,3.3,3.1,,12,8,,,51,,")as OkNegotiatedContentResult<string>;
            Assert.IsInstanceOfType(result.Content,typeof(string));
            Assert.AreEqual(result.Content,"OK");
        }
    }

}
