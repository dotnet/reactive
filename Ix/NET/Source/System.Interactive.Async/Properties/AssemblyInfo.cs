using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("System.Interactive.Async")]
// Notice: same description as in the .nuspec files; see Source/Interactive Extensions/Setup/NuGet
[assembly: AssemblyDescription("Interactive Extensions Async Library used to express queries over asynchronous enumerable sequences.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Retail")]
#endif
[assembly: AssemblyCompany("Microsoft Corporation")]
#if STABLE
[assembly: AssemblyProduct("Interactive Extensions")]
#else
[assembly: AssemblyProduct("Interactive Extensions")]
#endif
[assembly: AssemblyCopyright("\x00a9 Microsoft Corporation.  All rights reserved.")]
[assembly: NeutralResourcesLanguage("en-US")]

#if !PLIB
[assembly: ComVisible(false)]
#endif

[assembly: CLSCompliant(true)]

#if DESKTOPCLR && NO_CODECOVERAGE
[assembly: AllowPartiallyTrustedCallers]
#endif

//
// Note: Assembly (file) version numbers get inserted by the build system on the fly. Inspect the Team Build workflows
//       and the custom activity in Build/Source/Activities/AppendVersionInfo.cs for more information.
//
