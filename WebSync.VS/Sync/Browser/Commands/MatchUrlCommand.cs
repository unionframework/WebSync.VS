using Microsoft.CodeAnalysis;
using RoslynSpike.Compiler;
using RoslynSpike.Reflection;
using System;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Sync;
using WebSync.VS.Sync.Browser.Messages;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    internal class MatchUrlCommand : CommandWithDataBase<MatchUrlMessage>
    {
        private IAssemblyProvider _assemblyProvider;

        public MatchUrlCommand(Workspace workspace, IAssemblyProvider assemblyProvider, object data) : base(workspace, data)
        {
            _assemblyProvider = assemblyProvider;
        }

        public override Task<VSMessage> ExecuteAsync(MatchUrlMessage message)
        {
            try
            {
                //LogInfo($"MatchUrl: {url}");
                //_log.Info($"MatchUrl: {url}");
                var assemblies = _assemblyProvider.GetAssemblies();
                if (assemblies != null)
                {
                    //LogInfo($"Compiled successfully.");
                    var urlMatcher = new UrlMatcher(assemblies.Item1, assemblies.Item2);
                    var matchUrlResult = urlMatcher.Match(message.url);
                    return Task.FromResult(new VSMessage(VSMessageType.UrlMatchResponse, matchUrlResult));
                    //LogInfo($"Matched successfully: {matchUrlResult.ServiceId}-{matchUrlResult.PageId}");
                    //_browserConnection.SendUrlMatchResult(matchUrlResult);
                    //if (!string.IsNullOrWhiteSpace(matchUrlResult.PageId))
                    //{
                    //    OpenDocumentWithType(matchUrlResult.PageId);
                    //}
                }
                return Task.FromResult(new VSErrorMessage(VSMessageType.UrlMatchResponse, "Unable to match URL") as VSMessage);
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
                return Task.FromResult(new VSErrorMessage(VSMessageType.UrlMatchResponse, e.Message) as VSMessage);
            }
        }
    }
}