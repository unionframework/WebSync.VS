﻿using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.BrowserConnection;

using RoslynSpike.Ember;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;
using RoslynSpike.SessionWeb;
using RoslynSpike.Compiler;
using Microsoft.VisualStudio.ComponentModelHost;

namespace WebSync.VS
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(RoslynPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class RoslynPackage : AsyncPackage
    {
        /// <summary>
        /// RoslynPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "46bca1d3-661d-4f59-9da7-7e7e3318e176";

        private WebSync _webSync;
        //private SelectorsConverter _selectorsConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoslynPackage"/> class.
        /// </summary>
        public RoslynPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            IComponentModel componentModel = GetService(typeof(SComponentModel)) as IComponentModel;
            var workspace = componentModel.GetService<VisualStudioWorkspace>();

            var browserConnection = CreateBrowserConnection();
            _webSync = new WebSync(
                workspace, 
                browserConnection, 
                new RoslynWebInfoProvider(workspace),
                new RoslynAssemblyProvider(workspace));
            //_selectorsConverter = new SelectorsConverter(browserConnection);
        }

        #endregion.


        private IBrowserConnection CreateBrowserConnection()
        {
            try
            {
                //#if !DEBUG
                //                var connection = new WebSocketBrowserConnection(18000, "/websync", new EmberSerializer());
                //#else
                var connection = new WebSocketBrowserConnection(1804, "/websync", new EmberSerializer());
                //#endif

                connection.Connect();
                return connection;
            }
            catch (Exception)
            {
                Console.WriteLine();
                throw;
            }
        }

    }
}
