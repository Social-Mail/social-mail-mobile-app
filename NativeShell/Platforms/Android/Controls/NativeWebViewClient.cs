using Android.Webkit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using NativeShell.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeShell.Platforms.Android.Controls
{
    class NativeWebViewClient : MauiWebViewClient
    {
        private readonly global::Android.Webkit.WebView platformView;
        private readonly NativeWebView nativeWebView;

        public NativeWebViewClient(WebViewHandler handler, NativeWebView nativeWebView) : base(handler)
        {

            this.platformView = handler.PlatformView;
            this.nativeWebView = nativeWebView;
        }

        public override void OnPageFinished(global::Android.Webkit.WebView? view, string? url)
        {
            base.OnPageFinished(view, url);

            // setup message channel...
            var channel = platformView.CreateWebMessageChannel();
            var sender = channel[0];
            var receiver = channel[1];

            // send receiver to page ...
            var message = new WebMessage("native-port", new[] { receiver });
            platformView.PostWebMessage(message, global::Android.Net.Uri.Parse(url));

            sender.SetWebMessageCallback(new MessageCallback(this.nativeWebView, sender));
        }


        class MessageCallback: WebMessagePort.WebMessageCallback
        {
            private readonly NativeWebView client;
            private readonly WebMessagePort sender;

            public MessageCallback(NativeWebView client, WebMessagePort sender)
            {
                this.client = client;
                this.sender = sender;
            }

            public override void OnMessage(WebMessagePort? port, WebMessage? message)
            {
                client.RunMainThreadJavaScript(message.Data, (msg) => {
                    sender.PostMessage(new WebMessage(msg));
                });
            }
        }
    }
}
