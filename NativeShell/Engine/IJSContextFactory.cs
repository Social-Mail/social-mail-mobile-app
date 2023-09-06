using System;

namespace NativeShell
{
    /// <summary>
    /// Creates JavaScript Engine that implements IJSContext interface
    /// </summary>
    public interface IJSContextFactory
    {
        /// <summary>
        /// Creates new JavaScript Engine
        /// </summary>
        /// <returns></returns>
        IJSContext Create();

        IJSContext Create(Uri inverseWebSocketUri);
    }
}
