using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;

namespace WebSync.VS.BrowserConnection.Commands
{
    internal interface ICommand
    {
        Task<VSMessage> ExecuteAsync();
    }
}
