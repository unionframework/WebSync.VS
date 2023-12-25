using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb
{
    public interface ISessionWebPovider {
        Task<bool> UpdateSessionWebsAsync(IWebInfo sessionWeb, DocumentId changedDocumentId);
        Task<IEnumerable<IWebInfo>> GetSessionWebsAsync(bool useCache);
    }
}