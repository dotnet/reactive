// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// Provides access to the platform enlightenments used by other Rx libraries to improve system performance and
    /// runtime efficiency. While Rx can run without platform enlightenments loaded, it's recommended to deploy the
    /// System.Reactive.PlatformServices assembly with your application and call <see cref="EnsureLoaded"/> during
    /// application startup to ensure enlightenments are properly loaded.
    /// </summary>
    public static class EnlightenmentProvider
    {
        /// <summary>
        /// Ensures that the calling assembly has a reference to the System.Reactive.PlatformServices assembly with
        /// platform enlightenments. If no reference is made from the user code, it's possible for the build process
        /// to drop the deployment of System.Reactive.PlatformServices, preventing its runtime discovery.
        /// </summary>
        /// <returns>
        /// true if the loaded enlightenment provider matches the provided defined in the current assembly; false
        /// otherwise. When a custom enlightenment provider is installed by the host, false will be returned.
        /// </returns>
        public static bool EnsureLoaded()
        {
#pragma warning disable CS0618
            return PlatformEnlightenmentProvider.Current is CurrentPlatformEnlightenmentProvider;
#pragma warning restore CS0618
        }
    }
}
