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
    internal class NativeWKWebView : MauiWKWebView
    {
        public NativeWKWebView(CGRect frame, WebViewHandler handler) : base(frame, handler)
        {

        }

    }
}
