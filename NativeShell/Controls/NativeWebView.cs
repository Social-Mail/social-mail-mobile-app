using NativeShell.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeShell.Controls
{
    public class GlobalClr
    {
        public GlobalClr() {
        }


        public Type? ResolveType(string  typeName)
        {
            return Type.GetType(typeName);
        }
    }

    public partial class NativeWebView : WebView
    {

        static NativeWebView() {
            OnStaticPlatformInit();
        }

        private DisposableList disposables = new DisposableList();

        public IJSContext Context { get;set; }

        partial void OnPlatformInit();

        static  partial void OnStaticPlatformInit();

        public NativeWebView()
        {
            Context = DependencyService.Get<IJSContextFactory>().Create();

            Context["clr"] = Context.Marshal(new GlobalClr());

            // setup channel...
            OnPlatformInit();

            
        }

        /// <summary>
        /// This JavaScript will have access to entire CLR and will be able to execute
        /// everything in CLR.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="callback"></param>
        public void RunMainThreadJavaScript(string script, Action<string> callback)
        {
            Dispatcher.Dispatch(() => {
                try
                {
                    var result = Context.Evaluate(script);
                    callback(result?.ToString() ?? "null");
                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
        }

        ~NativeWebView()
        {
            disposables.Dispose();
        }


    }
}
