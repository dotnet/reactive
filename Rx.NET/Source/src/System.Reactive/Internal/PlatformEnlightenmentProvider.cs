// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
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
        /// (Infrastructure) Tries to gets the specified service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="args">Optional set of arguments.</param>
        /// <returns>Service instance or null if not found.</returns>
        T? GetService<T>(params object[] args) where T : class;
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
        private static IPlatformEnlightenmentProvider _current = CreatePlatformProvider();

        //
        // NOTE TO MAINTAINERS
        //
        //  Do *NOT* remove this mechanism which has been used beyond its original goal of supporting dependency injection when we were unifying
        //  the different Rx implementations as one Portable Library with platform-specific PlatformServices assemblies on top. Besides this, it
        //  has been used (and is still being used) for hosting scenarios where the host wants control over thread creation (cf. CAL), or other
        //  services (e.g. interception of query operators for logging, debugging, quota management, policy injection, etc.). Compat matters and
        //  lack thereof results in users getting stuck on older versions or having to resort to forking.
        //

        /// <summary>
        /// (Infrastructure) Gets the current enlightenment provider. If none is loaded yet, accessing this property triggers provider resolution.
        /// </summary>
        /// <remarks>
        /// This member is used by the Rx infrastructure and not meant for public consumption or implementation.
        /// </remarks>
        public static IPlatformEnlightenmentProvider Current
        {
            get => _current;
            set => _current = value ?? throw new ArgumentNullException(nameof(value));
        }

        private static IPlatformEnlightenmentProvider CreatePlatformProvider() => new CurrentPlatformEnlightenmentProvider();
    }
}
