using NativeShell.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeShell.Controls
{

    public partial class NativeWebView : WebView
    {

        static NativeWebView() {
            OnStaticPlatformInit();
        }

        private DisposableList disposables = new DisposableList();

        public IJSContext Context { get;set; }

        partial void OnPlatformInit();

        static  partial void OnStaticPlatformInit();

        public GlobalClr Clr { get; }

        public NativeWebView()
        {
            Context = DependencyService.Get<IJSContextFactory>().Create();
            this.Clr = new GlobalClr();
            Context["clr"] = Context.Marshal(Clr);

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
            Dispatcher.DispatchTask( async () => {
                try
                {
                    string serialized = "{ \"value\": null }";
                    var result = Context.Evaluate(script);
                    if (result.IsWrapped)
                    {
                        var unwrapped = result.Unwrap<object>();
                        if (unwrapped is Task task)
                        {
                            await task;

                            var taskType = task.GetType();

                            if (taskType != typeof(Task)) {
                                var p = taskType.GetProperty("Result");
                                if (p != null)
                                {
                                    var value = p.GetValue(task);
                                    serialized = this.Clr.Serialize(value);
                                }
                            }
                            

                        }
                    }
                    callback(serialized);
                } catch (Exception ex) {
                    callback(this.Clr.Serialize(ex));
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
