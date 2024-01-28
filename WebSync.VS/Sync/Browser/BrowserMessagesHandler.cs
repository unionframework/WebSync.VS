using RoslynSpike.BrowserConnection;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.Compiler;
using RoslynSpike.SessionWeb;
using System;
using System.Threading.Tasks;
using WebSync.VS.BrowserConnection.Commands;
using WebSync.VS.ProjectInfo;

namespace WebSync.VS.Sync
{
    internal class BrowserMessagesHandler
    {
        private Microsoft.CodeAnalysis.Workspace _workspace;
        private IAssemblyProvider _assemblyProvider;
        private IProjectInfoPovider _projectInfoProvider;
        private ProjectInfoCache _projectInfoCache;
        private IProjectInfoSerializer _projectInfoSerializer;

        public BrowserMessagesHandler(Microsoft.CodeAnalysis.Workspace workspace, IAssemblyProvider assemblyProvider, IProjectInfoPovider projectInfoProvider, ProjectInfoCache projectInfoCache, IProjectInfoSerializer projectInfoSerializer)
        {
            _workspace = workspace;
            _assemblyProvider = assemblyProvider;
            _projectInfoProvider = projectInfoProvider;
            _projectInfoCache = projectInfoCache;
            _projectInfoSerializer = projectInfoSerializer;
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
                    return new MatchUrlCommand(_workspace, _assemblyProvider,  message.Data);
                case BrowserMessageType.OpenFile:
                    return new OpenFileForClassCommand(_workspace, message.Data);
                case BrowserMessageType.GetProjectNames:
                    return new GetProjectsCommand(_workspace,message.Data);
                case BrowserMessageType.GetProject:
                    return new GetProjectCommand(_workspace, _projectInfoProvider, _projectInfoSerializer, _projectInfoCache, message.Data);
                case BrowserMessageType.DeleteWebsite:
                    return null;
                case BrowserMessageType.UpdateWebsite:
                    return new UpdateWebsiteCommand(_workspace, message.Data);
                case BrowserMessageType.CreateWebsite:
                    return new CreateWebsiteCommand(_workspace, message.Data, message.AsyncId);
                case BrowserMessageType.CreatePageType:
                    return new CreatePageTypeCommand(_workspace, message.Data);
                case BrowserMessageType.CreateComponentType:
                    return new CreateComponentTypeCommand(_workspace, message.Data);
                case BrowserMessageType.DeletePageType:
                    return null;
                case BrowserMessageType.UdpatePageType:
                    return null;
                case BrowserMessageType.DeleteCompoenentType:
                    return null;
                case BrowserMessageType.AddComponentInstance:
                    return new AddComponentInstanceCommand(_workspace,message.Data);
                case BrowserMessageType.DeleteComponentInstance:
                    return new DeleteComponentInstanceCommand(_workspace,message.Data);
                case BrowserMessageType.UpdateComponentInstance:
                    return new UpdateComponentInstanceCommand(_workspace, message.Data);
                default:
                    throw new ArgumentOutOfRangeException(message.Type.ToString());
            }
        }
    }
}
