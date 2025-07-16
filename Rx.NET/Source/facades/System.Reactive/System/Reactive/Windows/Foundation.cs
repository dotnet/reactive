// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// Seem comments in Reactive\Linq.cs for why we exclude forwarders for these types in some reference assemblies.
#if !(BUILDING_REFERENCE_ASSEMBLY && NET8_0_OR_GREATER)

#if WINDOWS

using System.Runtime.CompilerServices;

[assembly:TypeForwardedToAttribute(typeof(System.Reactive.Windows.Foundation.AsyncInfoObservableExtensions))]

#endif

#endif
