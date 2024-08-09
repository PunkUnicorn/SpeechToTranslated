using ChatSample.Hubs;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ChatSample
{
    public class ClientList : ConcurrentDictionary<string, string>, IClientList
    {
        public Dictionary<string /*uniqueId*/, string /*country code*/> Clients { get => ToArray().ToDictionary(k => k.Key, v => v.Value); }

        public void AddClient(string uniqueId, string countryCode = null)
        {
            if (!Clients.TryAdd(uniqueId, countryCode))
                Clients[uniqueId] = countryCode;
                
        }
    }
}