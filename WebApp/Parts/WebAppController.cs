using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;
using System.IO;

namespace WebApp
{
    internal class WebAppController
    {
        static InterfaceConfigurator iConfig { get; set; }
        static EventManager eventManager { get; set; }
        static LocalServer localServer = new LocalServer();
        static string IndexFile { get; set; }

        public static void Initialize(MainWindow window, WebView2 target)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            RuntimeLog.Initialize();
            RuntimeLog.WriteLog("App started");

            IndexFile = $"{Environment.CurrentDirectory}\\App\\index.html";

            iConfig = new InterfaceConfigurator(window);
            iConfig.Layout(File.ReadAllText(IndexFile));

            eventManager = new EventManager(target);
            WebAppConfig.TargetWebView = target;
            target.DefaultBackgroundColor = System.Drawing.Color.Transparent;
            localServer.Start();

            target.Source = new Uri(localServer.Url + "App/index.html");
            RuntimeLog.WriteLog($"Navigated to {localServer.Url + "App/index.html"}");

            RuntimeLog.WriteLog($"App fully initialized");
            sw.Stop();
            RuntimeLog.WriteLog($"App fully initialized in {sw.ElapsedMilliseconds}ms");
        }
    }
}
