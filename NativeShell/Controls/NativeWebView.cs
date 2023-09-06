using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeShell.Controls
{
    public class NativeWebView: WebView
    {

        public IJSContext Context { get;set; }

        public NativeWebView()
        {
            Context = DependencyService.Get<IJSContextFactory>().Create();

            // setup channel...

            
        }


    }
}
