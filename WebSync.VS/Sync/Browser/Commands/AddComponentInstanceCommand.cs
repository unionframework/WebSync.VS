using Microsoft.CodeAnalysis;
using RoslynSpike.BrowserConnection.WebSocket;
using System;
using System.Threading.Tasks;

namespace WebSync.VS.BrowserConnection.Commands
{
    internal class AddComponentInstanceCommand : CommandBase
    {
        public AddComponentInstanceCommand(Solution solution, object data) : base(solution, data)
        {
        }

        public override Task<VSMessage> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
