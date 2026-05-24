// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Threading;

/// <summary>
/// Releases a lock acquired from <see cref="IAsyncGate.AcquireAsync"/>.
/// </summary>
/// <remarks>
/// <para>
/// Note that implementations of <see cref="IAsyncGate"/> may return a reference to themselves
/// as the <see cref="IAsyncGateReleaser"/>, so callers should not depend on each lock
/// acquisition returning a distinct <see cref="IAsyncGateReleaser"/>. (This enables gate
/// implementations to avoid unnecessary allocation during lock acquisition.)
/// </para>
/// </remarks>
public interface IAsyncGateReleaser
{
    /// <summary>
    /// Releases a lock acquired from <see cref="IAsyncGate.AcquireAsync"/>.
    /// </summary>
    void Release();
}
