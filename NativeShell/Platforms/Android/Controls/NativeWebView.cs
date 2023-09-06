using NativeShell.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeShell.Controls
{
    partial class NativeWebView
    {

        partial void OnAndroidInit()
        {
            KeyboardService.Instance.KeyboardChanged += Instance_KeyboardChanged;
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
                    this.EvaluateJavaScriptAsync($"document.body.dataset.keyboard = 'shown'; document.body.dataset.keyboardHeight = {height};");
                }
                catch { }
                return;
            }
            try
            {
                this.Margin = new Thickness(0, 0, 0, 0);
                this.EvaluateJavaScriptAsync($"document.body.dataset.keyboard = 'hidden'; document.body.dataset.keyboardHeight = 0;");
            }
            catch { }
        }
    }
}
