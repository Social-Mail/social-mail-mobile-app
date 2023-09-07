using Android.Webkit;
using Java.Interop;
using Microsoft.Maui.Handlers;
using NativeShell.Keyboard;
using NativeShell.Platforms.Android.Controls;
using NativeShell.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Views.ViewGroup;

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

        static partial void OnStaticPlatformInit()
        {
            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
        }

        private Android.Webkit.WebView PlatformView;

        partial void OnPlatformInit()
        {
            KeyboardService.Instance.KeyboardChanged += Instance_KeyboardChanged;
            this.disposables.Register(BackButtonInterceptor.Instance.InterceptBackButton(delegate {
                this.Eval("androidPressBackButton && androidPressBackButton()");
            }));
            
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (this.Handler != null)
            {
                var webViewHandler = (WebViewHandler)this.Handler;
                this.PlatformView = webViewHandler.PlatformView;
                PlatformView.SetWebChromeClient(new NativeWebViewChromeClient(webViewHandler));
                PlatformView.SetWebViewClient(new NativeWebViewClient(webViewHandler, this));
                this.PlatformView.AddJavascriptInterface(new JSBridge(this), "androidBridge");

                //this.PlatformView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

                //this.Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), delegate
                //{

                //    if (PlatformView.Parent is global::Android.Views.View v)
                //    {
                //        v.LayoutParameters = PlatformView.LayoutParameters;
                //        v.Invalidate();
                //        if (v.Parent is global::Android.Views.View vp) {
                //            vp.Invalidate();
                //        }
                //    }

                //});
            }
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
