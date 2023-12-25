using System.Collections.Generic;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.BrowserConnection {
    public interface IWebInfoSerializer
    {
        string Serialize(IEnumerable<IProjectInfo> webs);
        IEnumerable<IProjectInfo> Deserialize(string data);
    }
}