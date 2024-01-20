using Nulo.Modules.MultiLanguageManager;
using Nulo.Modules.WorkspaceManager;
using Nulo.Modules.ExtensionManager;
using Nulo.Core.Pages;
using Nulo.Core.Menus;

namespace Nulo {

    internal static class Program {
        public static ExtensionManager<ExtensionData, Miscellaneous> ExtensionManager;
        public static MultiLanguageManager<LanguageData> MultiLanguageManager;
        public static WorkspaceManager<WorkspaceTheme, WorkspaceData> WorkspaceManager;

        [STAThread]
        private static void Main() {
            var mutex = new Mutex(true, "NuloTimeg2024.1", out bool result);
            if(!result) {
                MessageBox.Show("Another instance is already running.");
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.SetCompatibleTextRenderingDefault(false);

            #region Loading Modules

            var splash = new SplashScreen();
            splash.Show();
            Application.DoEvents();
            Thread.Sleep(200);

            #region Multi-Language Manager

            // Notify
            splash.SetStatusLabel("...");
            // Settings
            MultiLanguageManager = new MultiLanguageManager<LanguageData>("Nulo.Modules.MultiLanguageManager.Language");
            // Await
            Thread.Sleep(200);

            #endregion Multi-Language Manager

            #region Extension Manager

            // Notify
            splash.SetStatusLabel(MultiLanguageManager.GetText("Pages_SplashScreen_PluginManager_LoadLocalMenuItem"));
            // Settings
            ExtensionManager = new ExtensionManager<ExtensionData, Miscellaneous>();
            ExtensionManager.LoadLocalMenuItem();
            // Await
            Thread.Sleep(200);

            foreach(var pluginItem in ExtensionManager.LoadPluginItems()) {
                // Notify
                splash.SetStatusLabel($"{MultiLanguageManager.GetText("Pages_SplashScreen_PluginManager_LoadPluginMenuItem")}: {pluginItem.ProductName}");
                // Settings
                ExtensionManager.LoadPluginMenuItem(pluginItem);
                // Await
                Thread.Sleep(200);
            }

            #endregion Extension Manager

            #region Workspace Manager

            // Notify
            splash.SetStatusLabel(MultiLanguageManager.GetText("Pages_SplashScreen_WorkspaceManager"));
            // Settings
            WorkspaceManager = new WorkspaceManager<WorkspaceTheme, WorkspaceData>();
            // Await
            Thread.Sleep(200);

            #endregion Workspace Manager

            splash.SetStatusLabel(string.Empty);
            var mainPage = new MainPage(ExtensionManager.Render());
            Thread.Sleep(200);
            splash.Dispose();

            #endregion Loading Modules

            Application.Run(mainPage);

            GC.KeepAlive(mutex);
        }
    }
}