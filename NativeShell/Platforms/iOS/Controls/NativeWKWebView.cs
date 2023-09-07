using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebKit;

namespace NativeShell.Platforms.iOS.Controls
{
    class NativeWebViewUserContentController: WebKit.WKUserContentController {

        public NativeWebViewUserContentController()
        {
        
        }
    }

    internal class NativeWKWebView : MauiWKWebView
    {
        private static WKWebViewConfiguration Init(WKWebViewConfiguration configuration)
        {
            configuration.AllowsInlineMediaPlayback = true;
            configuration.Preferences.JavaScriptCanOpenWindowsAutomatically = true;
            configuration.Preferences.JavaScriptEnabled = true;
            configuration.MediaTypesRequiringUserActionForPlayback = WKAudiovisualMediaTypes.None;
            configuration.UserContentController = new NativeWebViewUserContentController();
            return configuration;
        }

        public NativeWKWebView(
            CGRect frame,
            WebViewHandler handler,
            WKWebViewConfiguration configuration) : base(frame, handler, Init(configuration))
        {
        }
    }
}
