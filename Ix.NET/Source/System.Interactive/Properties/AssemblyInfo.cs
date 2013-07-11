using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("System.Interactive")]
// Notice: same description as in the .nuspec files; see Source/Interactive Extensions/Setup/NuGet
[assembly: AssemblyDescription("Interactive Extensions Main Library used to express queries over enumerable sequences.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Retail")]
#endif
[assembly: AssemblyCompany("Microsoft Open Technologies, Inc.")]
[assembly: AssemblyProduct("Interactive Extensions")]
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

[assembly: AssemblyVersion("0.9.0.0")] 
[assembly: AssemblyFileVersion("0.9.0.0")]

