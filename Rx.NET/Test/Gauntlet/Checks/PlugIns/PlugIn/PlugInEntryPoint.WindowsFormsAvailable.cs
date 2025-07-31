// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace PlugInTest
{
    public partial class PlugInEntryPoint
    {
        /// <summary>
        /// Determine whether we're using the netstandard2.0 or the net472 version of Rx by looking
        /// for a Windows-Forms-specific type.
        /// </summary>
        /// <returns>True if Windows Forms support is available.</returns>
        /// <remarks>
        /// <para>
        /// The scenario this aims to test is when a .NET Framework plug-in host application loads
        /// two plug-ins that both depend on the same version of Rx.NET, but where one will want
        /// the .NET Standard 2.0 version and the other will want the .NET Framework 4.7.2 version.
        /// This occurs if one of the plug-ins uses a version of .NET Framework older than 4.7.2,
        /// but which supports .NET Standard 2.0 (e.g. .NET 4.6.2).
        /// </para>
        /// <para>
        /// This causes problems on Rx.NET 5.0 and 6.0. (So this is one of the problems that the
        /// Great Unification did not manage to solve.)
        /// </para>
        /// </remarks>
        public bool IsWindowsFormsSupportAvailable()
        {
            // Using typeof(Observable) means that when the CLR JIT compiles this method, it will
            // have to resolve the System.Reactive assembly.
            var asm = typeof(Observable).Assembly;

            // We look for Windows Forms support using reflection because this source file gets
            // compiled into multiple plug-in projects, some of which will resolve to the
            // netstandard2.0 version of Rx.NET, meaning that we'll get a compile-time error if we
            // refer to the Windows-Forms-specific type directly.
            // Even in projects where that happens, it's possible for this type lookup to succeed
            // at runtime because of the problem this test is designed to expose: if two plug-ins
            // use the same version of Rx.NET 4.0 or later, but they want different TFM-specific
            // Rx.NET DLLs, then whichever plug-in loads first ends up determining the DLL that
            // actually loads.
            // So even when this source file is built into a .NET 4.6.2 project, meaning the build
            // process will resolve to the netstandard2.0 Rx.NET DLLs, it's possible that some
            // other plug-in has already loaded the net472 DLL for the same version of Rx.NET.
            //
            // A similar thing happened up to Rx 3.0. Rx 3.1 stopped this problem by giving each
            // DLL within a single NuGet package a slightly different version number. However,
            // that practice was dropped when the Great Unification happened for Rx 4.0. Rx 4.0
            // supported netstandard2.0 and net46, and the .NET build system considers the net46
            // target to be a better match for all .NET Frameworks >= 4.6, even though there are
            // cases where a netstandard2.0 DLL might be able to offer features unavailable in the
            // net46 one. (The earliest version of .NET to support netstandard2.0 was .NET 4.6.1.)
            // So with Rx 4.0, netstandard2.0 didn't come into play for .NET Framework projects,
            // meaning it didn't cause the problem we're testing for here.
            //
            // However, Rx 5.0 changed that by offering netstandard2.0 and net472 DLLs. For the
            // first time since Rx 4.0 shipped, a single version of Rx.NET contained two different
            // DLLs either of which could be a viable candidate for use by a .NET Framework
            // project. A plug-in targetting net462 would copy the netstandard2.0 System.Reactive
            // assembly into its output directory, whereas a plug-in targetting net472 would copy
            // the net472 one.
            // This means that once again it became possible for a plug-in to encounter type load
            // exceptions or missing method exceptions because some other plug-in had loaded first
            // and had loaded the netstandard2.0 DLL, meaning that net472-specific features would
            // be missing.
            // One obvious question is: how come nobody seems to have complained? This problem was
            // fixed in Rx 3.1 because people had complained about it. We suspect the answer is
            // that the current situation is different in three important ways:
            //  1. newly-written plug-ins will all target .NET 4.7.2 or later, so they will all
            //      resolve to the net472 DLL.
            //  2. plug-ins older than Rx 5.0 will necessarily have been built against Rx 4.0 or
            //      earlier, so they will never attempt to load Rx 5.0's netstandard2.0 DLL.
            //  3. if someone maintaining an older component tries to upgrade it and runs into
            //      this problem, there are ways they can fix it: either they can drop support
            //      for older .NET Frameworks, or they can just continue to use Rx 4.0.
            // Whereas in the Rx 3.0 days you tended to run into this problem by default, the
            // default now is that the problem won't occur. The only situation in which a conflict
            // is likely to occur in practice is with plug-ins that are new enough to use Rx 5.0
            // but old enough to target .NET 4.7.1 or older. And even if a plug-in author finds it
            // necessary for some reason to continue to target such a version of .NET, they have
            // the option to avoid the problem by using Rx 4.0; no such fallback existed in the
            // Rx 3.0 days.
            // So although Rx 5.0 effectively reintroduced the problem that Rx 3.0 had fixed, it
            // was less of a problem than it had been before.
            var cs = asm.GetType("System.Reactive.Concurrency.ControlScheduler")!;
            return cs != null;
        }
    }
}
