﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using NativeShell;

namespace SocialMailApp;

[IntentFilter(
    new[] {
        Intent.ActionMain
    },
    Categories = new[] {
        Intent.CategoryDefault,
        Intent.CategoryLauncher,
        Intent.CategoryAppEmail,
    }
)]
[IntentFilter(
    new[] {
        Intent.ActionView
    },
    Categories = new[] {
        Intent.CategoryDefault,
        Intent.CategoryBrowsable
    },
    DataScheme = "mailto"
)]
[IntentFilter(
    new[] {
        Intent.ActionSendto
    },
    Categories = new[] {
        Intent.CategoryDefault,
    },
    DataScheme = "mailto"
)]
[IntentFilter(
    new[] {
        Intent.ActionSend,
        Intent.ActionSendMultiple
    },
    Categories = new[] {
        Intent.CategoryDefault
    },
    DataMimeType = "*/*"
)]
[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    Exported = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : NativeShellMainActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }
}
