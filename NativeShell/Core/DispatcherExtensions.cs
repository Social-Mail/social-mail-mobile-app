using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace NativeShell
{
    internal static class DispatcherExtensions
    {

        public static void DispatchTask(this IDispatcher dispatcher, Func<Task> action)
        {
            dispatcher.Dispatch(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
        }

    }
}
