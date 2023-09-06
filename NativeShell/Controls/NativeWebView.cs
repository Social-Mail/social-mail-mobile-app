using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeShell.Controls
{
    public partial class NativeWebView: WebView
    {

        public IJSContext Context { get;set; }

        partial void OnAndroidInit();

        public NativeWebView()
        {
            Context = DependencyService.Get<IJSContextFactory>().Create();

            // setup channel...
            OnAndroidInit();

            
        }


    }
}
