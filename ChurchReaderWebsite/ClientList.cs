using ChatSample.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ChatSample
{
    public class ClientList : ConcurrentDictionary<string, string>, IClientList
    {
        public Dictionary<string /*client code*/, string /*country code*/> Clients { get => ToArray().ToDictionary(k => k.Key, v => v.Value); }

        public void AddClient(string clientCode, string countryCode = null)
        {
            if (this.ContainsKey(clientCode)) 
                this[clientCode] = countryCode;
            else if (!this.TryAdd(clientCode, countryCode))
                throw new InvalidOperationException();
                
        }
    }
}