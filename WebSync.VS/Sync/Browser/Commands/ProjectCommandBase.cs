using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace WebSync.VS.BrowserConnection.Commands
{
    internal abstract class ProjectCommandBase<TMessage> : CommandWithDataBase<TMessage>
    {

        protected ProjectCommandBase(Workspace workspace, object data) : base(workspace, data)
        {
        }

        protected Project GetProject(string name) {
            var project = Solution.Projects.FirstOrDefault(p => p.Name == name);
            if (project == null)
            {
                throw new InvalidOperationException($"Project not found: '{project.Name}'");
            }
            return project;
        }

        protected void ApplyChanges(Solution solution)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.Generic.Invoke(() =>
            {
                var updated = Workspace.TryApplyChanges(solution);
            });
        }
    }
}
