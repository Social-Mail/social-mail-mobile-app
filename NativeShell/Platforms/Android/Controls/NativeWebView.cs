using Android.Content;
using Android.Webkit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using NativeShell.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeShell.Controls
{
    public class AndroidAudioRecorderPermission : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new (string, bool)[] {
            (Android.Manifest.Permission.RecordAudio, true),
            (Android.Manifest.Permission.ModifyAudioSettings, true)
        };
    }

    public class AndroidCorseLocationPermission : Permissions.LocationWhenInUse
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new (string, bool)[]
        {
            (Android.Manifest.Permission.AccessCoarseLocation, true),
        };
    }


    class NativeWebViewChromeClient : MauiWebChromeClient
    {
        private IDispatcher dispatcher;
        private Context context;

        public NativeWebViewChromeClient(WebViewHandler handler) : base(handler)
        {
            this.dispatcher = Dispatcher.GetForCurrentThread()!;
            this.context = handler.Context;
        }

        public override void OnPermissionRequest(PermissionRequest? request)
        {
            dispatcher.DispatchTask(async () =>
            {
                // request?.Grant(request.GetResources());
                var resources = request?.GetResources();
                if (resources == null)
                {
                    return;
                }

                foreach (var resource in resources)
                {
                    if (resource.EndsWith(".VIDEO_CAPTURE"))
                    {
                        await Permissions.RequestAsync<Permissions.Camera>();
                        continue;
                    }
                    if (resource.EndsWith(".AUDIO_CAPTURE"))
                    {
                        await Permissions.RequestAsync<AndroidAudioRecorderPermission>();
                        continue;
                    }
                }
                request.Grant(resources);
            });
        }

        public override void OnGeolocationPermissionsShowPrompt(string? origin, GeolocationPermissions.ICallback? callback)
        {
            dispatcher.DispatchTask(async () =>
            {
                try
                {

                    // check if not enabled...
                    if (Platform.AppContext.GetSystemService(Context.LocationService) is LocationManager lm && lm.IsProviderEnabled(LocationManager.GpsProvider))
                    {
                        var state = await Xamarin.Essentials.Permissions.RequestAsync<AndroidCorseLocationPermission>();
                        if (state != PermissionStatus.Granted && state != PermissionStatus.Restricted)
                        {
                            throw new InvalidOperationException($"Permission not granted {state}");
                        }
                        callback?.Invoke(origin, true, true);
                        return;
                    }

                    // throw new InvalidOperationException("Location Service is not enabled");
                    callback?.Invoke(origin, false, false);

                }
                catch (Exception ex)
                {
                    await HybridApplication.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                    callback?.Invoke(origin, false, false);
                }
            });
        }
    }

    partial class NativeWebView
    {

        partial void OnAndroidInit()
        {
            KeyboardService.Instance.KeyboardChanged += Instance_KeyboardChanged;
            this.disposables.Register(BackButtonInterceptor.Instance.InterceptBackButton(delegate {
                this.Eval("androidPressBackButton && androidPressBackButton()");
            }));

            ((IWebViewHandler)this.Handler).PlatformView.SetWebChromeClient(new NativeWebViewChromeClient((WebViewHandler)this.Handler));
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
