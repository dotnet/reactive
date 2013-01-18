// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive
{
    // We can't make those based on the Strings_WindowsThreading.resx file, because the ObsoleteAttribute needs a compile-time constant.

    static class Constants_WindowsThreading
    {
#if !WINDOWS
        public const string OBSOLETE_INSTANCE_PROPERTY = "Use the Current property to retrieve the DispatcherScheduler instance for the current thread's Dispatcher object. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
#endif
    }
}
