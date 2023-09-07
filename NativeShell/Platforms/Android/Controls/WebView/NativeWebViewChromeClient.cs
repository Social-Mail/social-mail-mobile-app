using Android.Content;
using Android.Locations;
using Android.Webkit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace NativeShell.Controls
{
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
                        var state = await Permissions.RequestAsync<AndroidCorseLocationPermission>();
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
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                    callback?.Invoke(origin, false, false);
                }
            });
        }
    }
}
