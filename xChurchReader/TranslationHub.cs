using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MVCRealtimeSignalR.Hubs
{
    [HubName("translationHub")]
    public class TranslationHub : Hub
    {
        public static void Broadcast(string translation, string colour)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TranslationHub>();
            context.Clients.All.updatedTranslation(translation, colour);
        }
    }
}