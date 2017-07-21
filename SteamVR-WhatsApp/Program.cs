using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteamVR_WebKit;
using System.Drawing;
using System.IO;
using System.Threading;

namespace SteamVR_WhatsApp
{
    class Program
    {
        static WebKitOverlay overlay;

        static string script;

        static void Main(string[] args)
        {
            SteamVR_WebKit.SteamVR_WebKit.Init(new CefSharp.CefSettings() { PersistSessionCookies = true, CachePath = "chrome-cache" });
            SteamVR_WebKit.SteamVR_WebKit.FPS = 30;

            script = File.ReadAllText("Resources/Script.js");

            //SteamVR_WebKit.JsInterop.Notifications.RegisterIcon("default", new Bitmap(Environment.CurrentDirectory + "\\Resources\\spotify-logo-small.png")); //-- leads to exception

            overlay = new WebKitOverlay(new Uri("https://web.whatsapp.com/"), 1024, 1024, "whatsapp", "WhatsApp", OverlayType.Dashboard);
            overlay.CachePath = "chrome-cache";
            overlay.DashboardOverlay.Width = 2.0f;
            overlay.DashboardOverlay.SetThumbnail("Resources/whatsapp-icon.png");
            overlay.BrowserPreInit += Overlay_BrowserPreInit;
            if (args.Length > 0 && args[0] == "--debug")
            {
                overlay.BrowserReady += Overlay_BrowserReady;
            }
            overlay.StartBrowser();

            EventHandler<CefSharp.LoadingStateChangedEventArgs> handler = PageReady;
            overlay.Browser.LoadingStateChanged += handler;

            SteamVR_Application application = new SteamVR_Application();
            application.InstallManifest(true);
            if (args.Length > 0 && args[0] == "--debug")
            {
                application.RemoveManifest();
            }

            SteamVR_WebKit.SteamVR_WebKit.RunOverlays(); // Runs update/draw calls for all active overlays. And yes, it's blocking.
        }

        private static void Browser_ConsoleMessage(object sender, CefSharp.ConsoleMessageEventArgs e)
        {
            string[] srcSplit = e.Source.Split('/'); // We only want the filename
            //Console.WriteLine("[CONSOLE " + srcSplit[srcSplit.Length - 1] + ":" + e.Line + "] " + e.Message);
        }

        private static void Overlay_BrowserReady(object sender, EventArgs e)
        {
            overlay.Browser.GetBrowser().GetHost().ShowDevTools();
        }

        private static void Overlay_BrowserPreInit(object sender, EventArgs e)
        {
            Console.WriteLine("Browser is ready.");

            overlay.Browser.ConsoleMessage += Browser_ConsoleMessage;
            overlay.Browser.RegisterJsObject("notifications", new SteamVR_WebKit.JsInterop.Notifications(overlay.DashboardOverlay));
        }

        private static void PageReady(object sender, CefSharp.LoadingStateChangedEventArgs args)
        {
            if (!args.IsLoading)
            {
                overlay.Browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(script);
                Uri uri = new Uri(Environment.CurrentDirectory + "/Resources/login_page.html");
                string login_page_path = "file:///" + uri.AbsolutePath;
                //string escapedFilepath = login_page_path.Replace("\\", "\\\\");
                Console.WriteLine("Login page path: " + login_page_path);
                //Thread.Sleep(5000);
                overlay.Browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("onPageLoaded('" + login_page_path + "')");
                //overlay.Browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("onPageLoaded('text.html');");
                //overlay.TryExecAsyncJS(script);
                //overlay.TryExecAsyncJS("onPageLoaded('file://" + Environment.CurrentDirectory + "/Resources/login_page.html')");
            }
        }
    }
}