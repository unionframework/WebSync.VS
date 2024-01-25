using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;

namespace WebSync.VS.ProjectInfo
{
    internal class ProjectInfoCache
    {
        private Dictionary<string, IProjectInfo> _projects;
        public ProjectInfoCache()
        {
            _projects = new Dictionary<string,IProjectInfo>();
        }

        public void StoreProjectInfo(IProjectInfo projectInfo)
        {
            _projects[projectInfo.ProjectName] = projectInfo;
        }

        public IProjectInfo GetProjectInfo(string projectName)
        {
            if (_projects.TryGetValue(projectName, out IProjectInfo projectInfo))
            {
                return projectInfo;
            }
            return null;
        }
    }
}
