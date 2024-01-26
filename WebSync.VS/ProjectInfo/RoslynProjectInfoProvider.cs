using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using NLog;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.SessionWeb.RoslynModels;
using Solution = Microsoft.CodeAnalysis.Solution;
using Microsoft.VisualStudio.LanguageServices;
using RoslynSpike.Utilities.Extensions;
using System.IO;

namespace RoslynSpike.SessionWeb
{
    public class RoslynProjectInfoProvider:IProjectInfoPovider
    {
        private readonly VisualStudioWorkspace _workspace;

        public RoslynProjectInfoProvider(VisualStudioWorkspace workspace)
        {
            _workspace = workspace;
        }

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public async Task<IProjectInfo> GetProjectInfoAsync(bool useCache) {
            //if (_cachedSessionWebs == null || !useCache) {
                try {
                    var solution = _workspace.CurrentSolution;
                    var services = await GetServicesAsync(solution);
                    var pages = await GetPagesAsync(solution);
                    var components = await GetComponentsAsync(solution);

                    // . for now, we unable to extract sessions, so everything is store in one session
                    var roslynProjectInfo = new RoslynProjectInfo(Path.GetFileNameWithoutExtension(solution.FilePath), services, components, pages);
                    //var projectInfoList = new List<IProjectInfo> {roslynProjectInfo};
                    //CacheWebInfo(projectInfoList);
                    return roslynProjectInfo;
                }
                catch (Exception ex) {
                    _log.Error(ex, "Unable to collect selenium contexts");
                    throw;
                }
            //}
            //return _cachedSessionWebs;
        }

        public async Task<bool> UpdateProjectsAsync(IProjectInfo projectInfo, DocumentId changedDocumentId)
        {
            try
            {
                var document = _workspace.CurrentSolution.GetDocument(changedDocumentId);
                if (document==null)
                {
                    throw new NotImplementedException("document removed");
                }
                SemanticModel semanticModel = await document.GetSemanticModelAsync().ConfigureAwait(false);
                var syntaxTree = await document.GetSyntaxTreeAsync().ConfigureAwait(false);
                var typeDeclarationsInDocument =
                    syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
                var typesInDocument = typeDeclarationsInDocument.Select(td => semanticModel.GetDeclaredSymbol(td))
                    .OfType<INamedTypeSymbol>();

                var pages = GetPages(typesInDocument);
                var components = GetComppnents(typesInDocument);
                if (pages.Any() || components.Any())
                {
                    UpdateProjectInfo(projectInfo, pages, components);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Unable to collect selenium contexts");
                throw;
            }
        }

        private void UpdateProjectInfo(IProjectInfo projectInfo, IEnumerable<RoslynPageType> pageTypes, IEnumerable<RoslynComponentType> componentTypes) {
            // Update PageTypes
            foreach (var roslynPageType in pageTypes) {
                if (projectInfo.PageTypes.ContainsKey(roslynPageType.Id)) {
                    projectInfo.PageTypes[roslynPageType.Id] = roslynPageType;
                }
                else {
                    projectInfo.PageTypes.Add(roslynPageType.Id, roslynPageType);
                }
            }

            // Update ComponentTypes
            foreach (var roslynComponentType in componentTypes) {
                if (projectInfo.ComponentTypes.ContainsKey(roslynComponentType.Id)) {
                    projectInfo.ComponentTypes[roslynComponentType.Id] = roslynComponentType;
                }
                else {
                    projectInfo.ComponentTypes.Add(roslynComponentType.Id, roslynComponentType);
                }
            }
        }

        public IEnumerable<RoslynComponentType> GetComppnents(IEnumerable<INamedTypeSymbol> types) {
            return types
                .Where(t => t.AllInterfaces.Any(i =>i.GetFullTypeName() == ReflectionNames.BASE_COMPONENT_INTERFACE_FULL_NAME))
                .Select(dc => {
                    var page = new RoslynComponentType(dc);
                    page.Fill();
                    return page;
                });
        }

        public IEnumerable<RoslynPageType> GetPages(IEnumerable<INamedTypeSymbol> types) {
            return types
                .Where(t => t.AllInterfaces.Any(i =>i.GetFullTypeName() == ReflectionNames.BASE_PAGE_INTERFACE_FULL_NAME))
                .Select(dc => {
                    var page = new RoslynPageType(dc);
                    page.Fill();
                    return page;
                });
        }

        private async Task<IEnumerable<RoslynComponentType>> GetComponentsAsync(Solution solution)
        {
            INamedTypeSymbol baseType = await GetTypeByNameAsync(solution, ReflectionNames.BASE_COMPONENT_TYPE);
            var derivedClasses = (await GetDerivedClassesAsync(solution, baseType)).ToList();
            derivedClasses.Add(baseType);
            return derivedClasses.Select(dc => {
                var component = new RoslynComponentType(dc);
                component.Fill();
                return component;
            });
        }

        private async Task<IEnumerable<RoslynPageType>> GetPagesAsync(Solution solution)
        {
            var derivedClasses = await GetDerivedClassesAsync(solution, ReflectionNames.BASE_PAGE_TYPE);
            return derivedClasses.Select(dc => {
                var page = new RoslynPageType(dc);
                page.Fill();
                return page;
            });
        }

        private async Task<IEnumerable<RoslynService>> GetServicesAsync(Solution solution)
        {
            var derivedClasses = await GetDerivedClassesAsync(solution, ReflectionNames.BASE_SERVICE_TYPE);
            return derivedClasses.Select(dc => {
                var service = new RoslynService(dc);
                service.Fill();
                return service;
            });
        }

        private async Task<IEnumerable<INamedTypeSymbol>> GetDerivedClassesAsync(Solution solution, string baseClassName)
        {
            INamedTypeSymbol baseType = await GetTypeByNameAsync(solution, baseClassName);
            return await GetDerivedClassesAsync(solution, baseType);
        }

        private async Task<IEnumerable<INamedTypeSymbol>> GetDerivedClassesAsync(Solution solution, INamedTypeSymbol baseType)
        {
            if (baseType != null)
            {
                return (await SymbolFinder
                    .FindDerivedClassesAsync(baseType, solution, true, solution.Projects.ToImmutableHashSet())
                    .ConfigureAwait(false));//.Where(dc => !dc.IsAbstract);
            }
            return new List<INamedTypeSymbol>();
        }

        private async Task<INamedTypeSymbol> GetTypeByNameAsync(Solution solution, string className)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    SemanticModel semantic = await document.GetSemanticModelAsync().ConfigureAwait(false);
                    var baseType = semantic.Compilation.GetSymbolsWithName(name => name == className, SymbolFilter.Type).FirstOrDefault() as INamedTypeSymbol;
                    if (baseType != null)
                        return baseType;
                }
            }
            return null;
        }
    }
}