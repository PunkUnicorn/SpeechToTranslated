using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatSample.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ITranslationList translationList;
        private readonly IClientList clientList;

        public ChatHub(ITranslationList translationList, IClientList clientList)
        {
            this.translationList = translationList;
            this.clientList = clientList;
        }

        //public async Task Send(string countryCode, string message)
        //{
        //    // Call the broadcastMessage method to update clients.
        //    //wait Clients.All.SendAsync("translation", countryCode, message);

        //    // Or look at the list of subscribers for language Y

        //    foreach (var client in clientList.Clients)
        //        if (client.Value == countryCode)
        //            await Clients.Client(client.Key).SendAsync("translation2", countryCode, message);
        //}

        public void Hello()
        {
            clientList.AddClient(Context.ConnectionId, null);
            // returns list of available country codes
            // add client
            //return translationList.Translations.ToArray(); 
            Clients.Client(Context.ConnectionId).SendAsync("languages", translationList.Translations.ToArray());
        }


        public async Task<int> IWantTranslationsFor(string countryCode)
        {
            clientList.AddClient(Context.ConnectionId, countryCode);
            // Adds to list of subscribers for client X to get translations of language Y
            return 0;
        }
    }
}