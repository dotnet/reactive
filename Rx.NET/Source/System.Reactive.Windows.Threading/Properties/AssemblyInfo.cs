using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("System.Reactive.Windows.Threading")]
#if !USE_SL_DISPATCHER
// Notice: same description as in the .nuspec files; see Source/Rx/Setup/NuGet
[assembly: AssemblyDescription("Windows Presentation Foundation extensions library for Rx. Contains scheduler functionality for the WPF Dispatcher.")]
#else
// Notice: same description as in the .nuspec files; see Source/Rx/Setup/NuGet
[assembly: AssemblyDescription("Silverlight extensions library for Rx. Contains scheduler functionality for the Silverlight Dispatcher.")]
#endif
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

#if !WINDOWS
[assembly: CLSCompliant(true)]
#endif

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
