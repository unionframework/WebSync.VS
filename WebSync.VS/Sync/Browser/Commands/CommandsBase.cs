using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Threading.Tasks;

namespace WebSync.VS.BrowserConnection.Commands
{
    internal abstract class CommandBase : ICommand
    {
        protected Solution Solution;
        protected object Data;

        public CommandBase(Solution solution, object data)
        {
            Solution = solution;
            Data = data;
        }

        public abstract Task<StandardCommandResult> ExecuteAsync();
    }
}
