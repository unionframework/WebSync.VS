using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.BrowserConnection {
    public interface IProjectInfoSerializer
    {
        object Serialize(IProjectInfo projectInfo);
    }
}