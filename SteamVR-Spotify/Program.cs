using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteamVR_WebKit;

namespace SteamVR_Spotify
{
    class Program
    {
        static WebKitOverlay overlay;

        static void Main(string[] args)
        {
            SteamVR_WebKit.SteamVR_WebKit.Init();
            SteamVR_WebKit.SteamVR_WebKit.FPS = 30;

            //Notifications.RegisterIcon("default", new Bitmap(Environment.CurrentDirectory + "\\Resources\\alert.png"));

            overlay = new WebKitOverlay(new Uri("https://open.spotify.com"), 1024, 1024, "spotify", "Spotify", OverlayType.Dashboard);
            overlay.DashboardOverlay.Width = 2.0f;
            overlay.DashboardOverlay.SetThumbnail("Resources/spotify-logo.small.png");
            overlay.BrowserPreInit += Overlay_BrowserPreInit;
            overlay.BrowserReady += Overlay_BrowserReady;
            overlay.PageReady += Overlay_PageReady;
            overlay.StartBrowser();

            SteamVR_WebKit.SteamVR_WebKit.RunOverlays(); // Runs update/draw calls for all active overlays. And yes, it's blocking.

        }

        private static void Browser_ConsoleMessage(object sender, CefSharp.ConsoleMessageEventArgs e)
        {
            string[] srcSplit = e.Source.Split('/'); // We only want the filename
            Console.WriteLine("[CONSOLE " + srcSplit[srcSplit.Length - 1] + ":" + e.Line + "] " + e.Message);
        }

        private static void Overlay_BrowserReady(object sender, EventArgs e)
        {
            overlay.Browser.GetBrowser().GetHost().ShowDevTools();
        }

        private static void Overlay_BrowserPreInit(object sender, EventArgs e)
        {
            Console.WriteLine("Browser is ready.");

            overlay.Browser.ConsoleMessage += Browser_ConsoleMessage;
            //overlay.Browser.RegisterJsObject("testObject", new JsCallbackTest());
            overlay.Browser.RegisterJsObject("notifications", new SteamVR_WebKit.JsInterop.Notifications(overlay.DashboardOverlay));
        }

        private static void Overlay_PageReady(object sender, EventArgs e)
        {
            overlay.Browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("alert('test alert')");
        }
    }
}