using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using RoslynSpike.BrowserConnection.WebSocket;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace WebSync.VS.BrowserConnection.Commands
{
    internal abstract class CommandBase : ICommand
    {
        protected Workspace Workspace;
        protected Solution Solution => Workspace.CurrentSolution;
        protected object Data;

        public CommandBase(Workspace workspace, object data)
        {
            Workspace = workspace;
            Data = data;
        }

        public abstract Task<VSMessage> ExecuteAsync();
    }

    internal abstract class CommandWithDataBase<TMessage> : CommandBase
    {
        protected CommandWithDataBase(Workspace workspace, object data) : base(workspace, data)
        {
        }

        public override async Task<VSMessage> ExecuteAsync()
        {
            var message = JsonConvert.DeserializeObject<TMessage>(JsonConvert.SerializeObject(Data));
            return await ExecuteAsync(message);
        }

        public abstract Task<VSMessage> ExecuteAsync(TMessage message);
    }
}
