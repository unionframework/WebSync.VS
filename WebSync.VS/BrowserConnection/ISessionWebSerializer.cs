using System.Collections.Generic;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.BrowserConnection {
    public interface ISessionWebSerializer
    {
        string Serialize(IEnumerable<IWebInfo> webs);
        IEnumerable<IWebInfo> Deserialize(string data);
    }
}