using Android.Webkit;
using Java.Interop;
using Microsoft.Maui.Handlers;
using NativeShell.Keyboard;
using NativeShell.Platforms.Android.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeShell.Controls
{

    public class JSBridge : Java.Lang.Object
    {
        private readonly NativeWebView nativeWebView;

        internal JSBridge(NativeWebView nativeWebView)
        {
            this.nativeWebView = nativeWebView;
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            nativeWebView.RunMainThreadJavaScript(data);
        }
    }

    partial class NativeWebView
    {

        private Android.Webkit.WebView PlatformView;

        partial void OnPlatformInit()
        {
            KeyboardService.Instance.KeyboardChanged += Instance_KeyboardChanged;
            this.disposables.Register(BackButtonInterceptor.Instance.InterceptBackButton(delegate {
                this.Eval("androidPressBackButton && androidPressBackButton()");
            }));
            var webViewHandler = (WebViewHandler)this.Handler;
            this.PlatformView = webViewHandler.PlatformView;
            PlatformView.SetWebChromeClient(new NativeWebViewChromeClient(webViewHandler));
            PlatformView.SetWebViewClient(new NativeWebViewClient(webViewHandler, this));
            this.PlatformView.AddJavascriptInterface(new JSBridge(this), "androidBridge");
            
        }

        private void Instance_KeyboardChanged(object? sender, AndroidKeyboardEventArgs e)
        {
            if (e.IsOpen)
            {
                var height = e.Height;
                // get main..
                var main = Application.Current.MainPage;
                this.Margin = new Thickness(0, 0, 0, main.Height * height);
                try
                {
                    this.Eval($"document.body.dataset.keyboard = 'shown'; document.body.dataset.keyboardHeight = {height};");
                }
                catch { }
                return;
            }
            try
            {
                this.Margin = new Thickness(0, 0, 0, 0);
                this.Eval($"document.body.dataset.keyboard = 'hidden'; document.body.dataset.keyboardHeight = 0;");
            }
            catch { }
        }
    }
}
