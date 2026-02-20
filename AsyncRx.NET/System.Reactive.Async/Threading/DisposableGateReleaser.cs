// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Threading;

/// <summary>
/// Enables a <see langword="using" /> statement or declaration to be used to release an
/// <see cref="IAsyncGate"/>. Typically obtained through <see cref="AsyncGateExtensions.LockAsync(IAsyncGate)"/>
/// </summary>
/// <param name="gateReleaser"></param>
public struct DisposableGateReleaser(IAsyncGateReleaser gateReleaser) : IDisposable
{
    public void Dispose() => gateReleaser.Release();
}
