using System.Net;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Azure.NotificationHubs;
using Twilio.Http;

namespace pro_web_a.Helpers
{
    public class NotificationHubHelper
    {
        private static  NotificationHubClient _hubClient;

        private NotificationHubHelper()
        {
           _hubClient = new NotificationHubClient(WebConfigurationManager.AppSettings["NotificationHubConnectionString"], WebConfigurationManager.AppSettings["NotificationHubName"]);
        }

        public static NotificationHubClient GetHubClient()
        {
            if (_hubClient == null)
                new NotificationHubHelper();
            return _hubClient;
        }

        public static async Task<bool> SendPushNotification(string message)
        {
                NotificationOutcome outcome = null;

                var payload = "{ \"data\" : {\"message\":\""+ message + "\"}}";
                outcome = await GetHubClient().SendFcmNativeNotificationAsync(payload);

                if (outcome != null)
                {
                    if (!((outcome.State == NotificationOutcomeState.Abandoned) ||(outcome.State == NotificationOutcomeState.Unknown)))
                    {return true;}
                }return false;
        }
    }
}