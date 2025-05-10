using System.Collections.Generic;

namespace ChatSample.Hubs
{
    public interface IClientList
    {
        Dictionary<string /*uniqueId*/, string /*country code*/> Clients { get; }
        void AddClient(string uniqueId, string countryCode=null);
    }
}