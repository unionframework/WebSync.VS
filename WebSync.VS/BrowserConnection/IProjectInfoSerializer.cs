using System.Collections.Generic;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.BrowserConnection {
    public interface IProjectInfoSerializer
    {
        string Serialize(IEnumerable<IProjectInfo> webs);
        IEnumerable<IProjectInfo> Deserialize(string data);
    }
}