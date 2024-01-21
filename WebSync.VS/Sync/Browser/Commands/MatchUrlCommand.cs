using Microsoft.CodeAnalysis;
using RoslynSpike.Compiler;
using RoslynSpike.Reflection;
using System;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    internal class MatchUrlCommand : CommandBase
    {
        private IAssemblyProvider _assemblyProvider;

        public MatchUrlCommand(Solution solution, IAssemblyProvider assemblyProvider, object data) : base(solution, data)
        {
            _assemblyProvider = assemblyProvider;
        }

        public override Task<VSMessage> ExecuteAsync()
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
                    return Task.FromResult(new VSMessage(VSMessageType.UrlMatchResult, matchUrlResult));
                    //LogInfo($"Matched successfully: {matchUrlResult.ServiceId}-{matchUrlResult.PageId}");
                    //_browserConnection.SendUrlMatchResult(matchUrlResult);
                    //if (!string.IsNullOrWhiteSpace(matchUrlResult.PageId))
                    //{
                    //    OpenDocumentWithType(matchUrlResult.PageId);
                    //}
                }
                return Task.FromResult(new VSErrorMessage(VSMessageType.UrlMatchResult, "Unable to match URL") as VSMessage);
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
                return Task.FromResult(new VSErrorMessage(VSMessageType.UrlMatchResult, e.Message) as VSMessage);
            }
        }
    }
}