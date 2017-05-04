using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteamVR_WebKit;
using System.Drawing;

namespace SteamVR_Spotify
{
    class Program
    {
        static WebKitOverlay overlay;

        static void Main(string[] args)
        {
            SteamVR_WebKit.SteamVR_WebKit.Init(new CefSharp.CefSettings() { PersistSessionCookies = true, CachePath = "chrome-cache" });
            SteamVR_WebKit.SteamVR_WebKit.FPS = 30;

            //SteamVR_WebKit.JsInterop.Notifications.RegisterIcon("default", new Bitmap(Environment.CurrentDirectory + "\\Resources\\spotify-logo-small.png")); -- leads to exception

            overlay = new WebKitOverlay(new Uri("https://open.spotify.com"), 1024, 1024, "spotify", "Spotify", OverlayType.Dashboard);
            overlay.CachePath = "chrome-cache";
            overlay.DashboardOverlay.Width = 2.0f;
            overlay.DashboardOverlay.SetThumbnail("Resources/spotify-logo-small.png");
            overlay.BrowserPreInit += Overlay_BrowserPreInit;
            overlay.BrowserReady += Overlay_BrowserReady;
            overlay.PageReady += Overlay_PageReady;
            overlay.StartBrowser();

            EventHandler<CefSharp.LoadingStateChangedEventArgs> handler = PageReady;
            overlay.Browser.LoadingStateChanged += handler;

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

        private static void PageReady(object sender, CefSharp.LoadingStateChangedEventArgs args)
        {
            if (!args.IsLoading)
            {
                Console.WriteLine("Ready again!");
            }
            
        }

        private static void Overlay_PageReady(object sender, EventArgs e)
        {
            string script = @"(function() {
                    // Load the script
                    var script = document.createElement('SCRIPT');
                    script.src = 'https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js';
                    script.type = 'text/javascript';
                    script.onload = function() {
                        var $ = window.jQuery;

                        var user = '" + Properties.Settings.Default.user + "'\n" +
                        "var pwd = '" + Properties.Settings.Default.password + "'\n" +
                        @"account_link = $('a#has-account')[0]
                        if (account_link)
                        {
                            account_link.click()

                            login_user = $('input#login-usr')
                            if (login_user[0])
                            {
                                login_user.val(user)
                                $('input#login-pass').val(pwd)
                            }
                        } 
                        /*function track_notification( )
                        {
                            /*var artist_dom = $('.track-info__artists')
                            var artist = 'Unknown'
                            if (artist_dom) {
                                artist = artist_dom.firstChild.firstChild.firstChild.textContent
                            } else {
                                return
                            }

                            var name_dom = $('.track-info__name')
                            var name = 'Unknown'
                            if (name_dom) {
                                name = name_dom.firstChild.firstChild.firstChild.textContent
                            } else {
                                return
                            }*/

                            //notifications.sendNotification('Now playing: ' + artist + ' - ' + name)
                            notifications.sendNotification('Now playing: test')
                        }

                        setInterval(track_notification, 2000);*/
                    };
                    document.getElementsByTagName('head')[0].appendChild(script);
                })();";
            //Console.WriteLine(script);
            overlay.Browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(script);
        }
    }
}