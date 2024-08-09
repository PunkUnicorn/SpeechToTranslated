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

        public async Task Send(string countryCode, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("translation", countryCode, message);

            // Or look at the list of subscribers for language Y

            foreach (var client in clientList.Clients)
                if (client.Value == countryCode)
                    await Clients.Client(client.Key).SendAsync("translation2", countryCode, message);
        }

        public async Task<string[]> Hello(string countryCode=null)
        {
            clientList.AddClient(Context.ConnectionId, countryCode);
            // returns list of available country codes
            // add client
            return translationList.Translations.ToArray(); 
        }

        public async Task<int> IWantTranslationsFor(string uniqueId, string countryCode)
        {
            clientList.AddClient(Context.ConnectionId, countryCode);
            // Adds to list of subscribers for client X to get translations of language Y
            return 0;
        }
    }
}