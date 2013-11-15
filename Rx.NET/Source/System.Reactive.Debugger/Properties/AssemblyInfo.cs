using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("System.Reactive.Debugger")]
// Notice: same description as in the .nuspec files; see Source/Rx/Setup/NuGet
[assembly: AssemblyDescription("Reactive Extensions Debugger Library containing runtime hooks for query operators to allow debugging and tracing.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Retail")]
#endif
[assembly: AssemblyCompany("Microsoft Open Technologies, Inc.")]
[assembly: AssemblyProduct("Reactive Extensions")]
[assembly: AssemblyCopyright("\x00a9 Microsoft Open Technologies, Inc.  All rights reserved.")]
[assembly: NeutralResourcesLanguage("en-US")]

#if !PLIB
[assembly: ComVisible(false)]
#endif

[assembly: CLSCompliant(true)]

#if HAS_APTCA && NO_CODECOVERAGE
[assembly: AllowPartiallyTrustedCallers]
#endif

// ===========================================================================
//  DO NOT EDIT OR REMOVE ANYTHING BELOW THIS COMMENT.
//  Version numbers are automatically generated in the msbuild files based on regular expressions 
// ===========================================================================

[assembly: AssemblyVersion("2.2.0.0")]
[assembly: AssemblyFileVersion("2.2.0.0")]
[assembly: AssemblyInformationalVersion("2.2.0.0")]
