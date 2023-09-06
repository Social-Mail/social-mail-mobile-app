using NativeShell.Platforms.Android.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YantraJS.Core;

[assembly: Dependency(typeof(YantraContextFactory))]
namespace NativeShell.Platforms.Android.Engine
{
    public class YantraContextFactory : IJSContextFactory
    {
        public IJSContext Create()
        {
            return new JSContext();
        }

        public IJSContext Create(Uri inverseWebSocketUri)
        {
            return new JSContext();
        }
    }
}
