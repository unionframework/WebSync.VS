using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
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

        public abstract Task<VSMessage> ExecuteAsync();
    }

    internal abstract class CommandWithDataBase<TMessage> : CommandBase
    {
        protected CommandWithDataBase(Solution solution, object data) : base(solution, data)
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
