// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.


using System.Reflection;

// TODO: should these packages continue to have V3.0.x.0 version numbers?
// I had wanted to remove the uap10 targets from most of these, and switch back from
// MSBuild.Sdk.Extras to Microsoft.NET.Sdk, but if these are going to continue to
// claim to be V3.0 that might cause problems.
// I suppose the questions are:
//  * who do we expect still to be using these packages?
//  * what do we want them to get?
// These packages became nothing more than legacy facades in Rx 4.0 (shipped May 2018).
// In the 6 months since Rx 6.0 shipped, System.Reactive has had 2,043,585 downloads,
// while the System.Reactive.Linq package (which nobody should be using) has had
// 270,216 downloads! For a package nobody is meant to be using, it remains remarkably
// popular.
// So apparently people are taking dependencies on this and are ending up with the
// current version. So whatever it is they are doing today we need that to continue
// to work.
// What would go wrong if we trimmed this down to netstandard2.0 and net6.0, the same
// targets as System.Reactive.Base itself now supports?
// Perhaps what we need to do for all of these facades is ask the following question:
// Do any of the forwarded types have a different API surface area in different target?
// If no, then there should be absolutely no problem with just targetting the same as
// System.Reactive.Base. (In theory, the only problematic case would be the UWP version
// of ThreadPoolScheduler, which we've already decided to break backwards compatibility
// on as a special case, because that one on its own prevents everything else from
// moving forwards otherwise.)


#if WINDOWS_UWP
[assembly: AssemblyVersion("3.0.4000.0")]
#elif NET472 || NETSTANDARD2_0
[assembly: AssemblyVersion("3.0.6000.0")]
#else // this is here to prevent the build system from complaining. It should never be hit
[assembly: AssemblyVersion("invalid")]
#endif

