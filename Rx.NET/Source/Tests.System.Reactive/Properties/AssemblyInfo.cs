using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Tests.System.Reactive")]
[assembly: AssemblyDescription("Unit tests for Reactive Extensions product assemblies.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Retail")]
#endif
[assembly: AssemblyCompany(".NET Foundation and Contributors.")]
[assembly: AssemblyProduct("Reactive Extensions")]
[assembly: AssemblyCopyright("\x00a9 .NET Foundation and Contributors.  All rights reserved.")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: ComVisible(false)]
