using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using pro_web_a.Helpers;

namespace pro_web_a.Controllers
{
    [RoutePrefix("Api/Notification")]
    public class NotificationsController : ApiController
    {
        private readonly NotificationHubClient _hubClient = NotificationHubHelper.GetHubClient();


        // This creates a registration id
        [Route("GetRegistrationId")]
        public async Task<IHttpActionResult> GetRegistrationId(string pns_FCM_Token = null)
        {
            string newRegistrationId = null;


            if (pns_FCM_Token != null)
            {
                var registrations = await _hubClient.GetRegistrationsByChannelAsync(pns_FCM_Token, 10);

                foreach (RegistrationDescription registration in registrations)
                {
                    if (newRegistrationId == null)
                    {
                        newRegistrationId = registration.RegistrationId;
                    }
                    else
                    {
                        await _hubClient.DeleteRegistrationAsync(registration);
                    }
                }
            }

            if (newRegistrationId == null)
                newRegistrationId = await _hubClient.CreateRegistrationIdAsync();

            return Ok(newRegistrationId);
        }


        // This creates or updates a registration (with provided channelURI) at the specified id
        [Route("updateRegistration")]
        public async Task<IHttpActionResult> GetUpdateRegistration(string id, string platform, string token)
        {
            RegistrationDescription registration = null;
            switch (platform)
            {
                case "fcm":
                    registration = new FcmRegistrationDescription(token);
                    break;
                default:
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            registration.RegistrationId = id;

            try
            {
                var  dd = await _hubClient.CreateOrUpdateRegistrationAsync(registration);
            }
            catch (MessagingException e)
            {
                ReturnGoneIfHubResponseIsGone(e);
            }

            return Ok();
        }

        private static void ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if (webex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webex.Response;
                if (response.StatusCode == HttpStatusCode.Gone)
                    throw new HttpRequestException(HttpStatusCode.Gone.ToString());
            }
        }
    }
}
