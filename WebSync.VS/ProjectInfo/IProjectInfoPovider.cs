using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb
{
    public interface IProjectInfoPovider {
        Task<bool> UpdateProjectsAsync(IProjectInfo sessionWeb, DocumentId changedDocumentId);
        Task<IProjectInfo> GetProjectInfoAsync(bool useCache);
    }
}