using RoslynSpike.BrowserConnection;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.Compiler;
using RoslynSpike.SessionWeb;
using System;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.Serializers.Mobx;

namespace WebSync.VS.Sync
{
    internal class BrowserMessagesHandler
    {
        private Microsoft.CodeAnalysis.Workspace _workspace;
        private Microsoft.CodeAnalysis.Solution _solution;
        private IAssemblyProvider _assemblyProvider;
        private IProjectInfoPovider _projectInfoProvider;

        public BrowserMessagesHandler(Microsoft.CodeAnalysis.Workspace workspace, IAssemblyProvider assemblyProvider, IProjectInfoPovider projectInfoProvider)
        {
            _workspace = workspace;
            _solution = workspace.CurrentSolution;
            _assemblyProvider = assemblyProvider;
            _projectInfoProvider = projectInfoProvider;
        }

        public async Task<VSMessage> HandleAsync(BrowserMessage message)
        {
            var command = GetCommand(message);
            return await command.ExecuteAsync();
        }

        private ICommand GetCommand(BrowserMessage message)
        {
            switch (message.Type)
            {
                case BrowserMessageType.MatchUrl:
                    return new MatchUrlCommand(_solution, _assemblyProvider,  message.Data);
                case BrowserMessageType.OpenFile:
                    return new OpenFileForClassCommand(_workspace, message.Data);
                case BrowserMessageType.GetProjectNames:
                    return new GetProjectsCommand(_solution,message.Data);
                case BrowserMessageType.GetProject:
                    return new GetProjectCommand(_solution, _projectInfoProvider, new MobxProjectInfoSerializer(), message.Data);
                case BrowserMessageType.DeleteWebsite:
                    return null;
                case BrowserMessageType.UpdateWebsite:
                    return new UpdateWebsiteCommand(_solution, message.Data);
                case BrowserMessageType.CreateWebsite:
                    return new CreateWebsiteCommand(_solution, message.Data);
                case BrowserMessageType.CreatePageType:
                    return new CreatePageTypeCommand(_solution, message.Data);
                case BrowserMessageType.CreateComponentType:
                    return new CreateComponentTypeCommand(_solution, message.Data);
                case BrowserMessageType.DeletePageType:
                    return null;
                case BrowserMessageType.UdpatePageType:
                    return null;
                case BrowserMessageType.DeleteCompoenentType:
                    return null;
                case BrowserMessageType.AddCompoenentInstance:
                    return new AddComponentInstanceCommand(_solution,message.Data);
                case BrowserMessageType.DeleteComponentInstance:
                    return new DeleteComponentInstanceCommand(_solution,message.Data);
                case BrowserMessageType.UpdateComponentInstance:
                    return new UpdateComponentInstanceCommand(_solution, message.Data);
                default:
                    throw new ArgumentOutOfRangeException(message.Type.ToString());
            }
        }
    }
}
