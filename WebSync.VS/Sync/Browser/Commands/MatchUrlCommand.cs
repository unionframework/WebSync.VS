using Microsoft.CodeAnalysis;
using RoslynSpike.Compiler;
using RoslynSpike.Reflection;
using System;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    internal class MatchUrlCommand : CommandBase
    {
        private IAssemblyProvider _assemblyProvider;

        public MatchUrlCommand(Solution solution, IAssemblyProvider assemblyProvider, object data) : base(solution, data)
        {
            _assemblyProvider = assemblyProvider;
        }

        public override Task<StandardCommandResult> ExecuteAsync()
        {
            try
            {
                var url = Data as string;
                //LogInfo($"MatchUrl: {url}");
                //_log.Info($"MatchUrl: {url}");
                var assemblies = _assemblyProvider.GetAssemblies();
                if (assemblies != null)
                {
                    //LogInfo($"Compiled successfully.");
                    var urlMatcher = new UrlMatcher(assemblies.Item1, assemblies.Item2);
                    var matchUrlResult = urlMatcher.Match(url);
                    return Task.FromResult(new StandardCommandResult());
                    //LogInfo($"Matched successfully: {matchUrlResult.ServiceId}-{matchUrlResult.PageId}");
                    //_browserConnection.SendUrlMatchResult(matchUrlResult);
                    //if (!string.IsNullOrWhiteSpace(matchUrlResult.PageId))
                    //{
                    //    OpenDocumentWithType(matchUrlResult.PageId);
                    //}
                }
                return Task.FromResult(new StandardCommandResult());
            }
            catch (Exception e)
            {
                //LogInfo($"Error!!!");
                //LogInfo(e.Message);
                //LogInfo(e.StackTrace);
                //if (e.InnerException != null)
                //{
                //    LogInfo(e.InnerException.Message);
                //    LogInfo(e.InnerException.StackTrace);
                //}
                return Task.FromResult(new StandardCommandResult());
            }

        }
    }
}