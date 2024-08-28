// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.


using System.Reflection;


#if WINDOWS_UWP
[assembly: AssemblyVersion("3.0.4000.0")]
#elif NET472 || NETSTANDARD2_0
[assembly: AssemblyVersion("3.0.6000.0")]
#else // this is here to prevent the build system from complaining. It should never be hit
[assembly: AssemblyVersion("invalid")]
#endif

