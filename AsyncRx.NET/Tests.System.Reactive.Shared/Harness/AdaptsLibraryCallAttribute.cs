// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Marks an <c>*Impl</c> whose signature is not the library call's own.</summary>
/// <remarks>
/// Most <c>*Impl</c>s mirror one library call, and <see cref="ImplSignatures"/> checks each
/// against the call it makes. A few adapt instead: the harness's subscribe helpers wrap the
/// scenario's handlers in an observer before the call, and some read a property or build an
/// object and call nothing. For these the check falls back to the rule that no observable in
/// the signature is closed over anything but a type parameter.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class AdaptsLibraryCallAttribute : Attribute;
