// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// (Infrastructure) Interface for enlightenment providers.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IPlatformEnlightenmentProvider
    {
        /// <summary>
        /// (Infastructure) Tries to gets the specified service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="args">Optional set of arguments.</param>
        /// <returns>Service instance or null if not found.</returns>
        T GetService<T>(params object[] args) where T : class;
    }

    /// <summary>
    /// (Infrastructure) Provider for platform-specific framework enlightenments.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PlatformEnlightenmentProvider
    {
        private static IPlatformEnlightenmentProvider s_current = CreatePlatformProvider();

        /// <summary>
        /// (Infrastructure) Gets the current enlightenment provider. If none is loaded yet, accessing this property triggers provider resolution.
        /// </summary>
        /// <remarks>
        /// This member is used by the Rx infrastructure and not meant for public consumption or implementation.
        /// </remarks>
        [Obsolete("This mechanism will be removed in the next major version", false)]
        public static IPlatformEnlightenmentProvider Current
        {
            get
            {
                return s_current;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                s_current = value;
            }
            
        }

        private static IPlatformEnlightenmentProvider CreatePlatformProvider() => new CurrentPlatformEnlightenmentProvider();
    }
}
