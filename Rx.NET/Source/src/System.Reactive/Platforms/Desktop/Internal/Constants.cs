// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    // We can't make those based on the Strings_WindowsThreading.resx file, because the ObsoleteAttribute needs a compile-time constant.

    internal static class Constants_WindowsThreading
    {
#if !WINDOWS
        public const string OBSOLETE_INSTANCE_PROPERTY = "Use the Current property to retrieve the DispatcherScheduler instance for the current thread's Dispatcher object.";
#endif
    }
}
