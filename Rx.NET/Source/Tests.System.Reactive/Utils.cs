// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.Versioning;

namespace ReactiveTests
{
    static class Utils
    {
        public static bool IsRunningWithPortableLibraryBinaries()
        {
#if DESKTOPCLR20
            return false;
#else

            var a = typeof(ISubject<int>).GetTypeInfo().Assembly.GetCustomAttributes<TargetFrameworkAttribute>().SingleOrDefault();
            return a != null && a.FrameworkDisplayName == ".NET Portable Subset";
#endif
        }
    }
}
