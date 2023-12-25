using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb
{
    public interface ISessionWebPovider {
        Task<bool> UpdateSessionWebsAsync(IProjectInfo sessionWeb, DocumentId changedDocumentId);
        Task<IEnumerable<IProjectInfo>> GetProjectInfoAsync(bool useCache);
    }
}