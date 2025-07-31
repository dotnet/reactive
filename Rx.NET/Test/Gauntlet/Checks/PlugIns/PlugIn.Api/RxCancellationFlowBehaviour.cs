// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace PlugIn.Api;

/// <summary>
/// Describes whether a cancellation token used to cancel a <c>ForEachAsync</c> is visible
/// in the <see cref="OperationCanceledException"/> thrown by the task returned by
/// <c>ForEachAsync</c>.
/// </summary>
/// <remarks>
/// <para>
/// This is the only observable difference between the .NET 4.5 and .NET 4.6 builds of Rx.NET 3.0.
/// It is somewhat obscure, but it does provide a way to test which Rx.NET target is in use when
/// we're using a version that has both .NET 4.5 and .NET 4.6 targets.
/// </para>
/// </remarks>
public enum RxCancellationFlowBehaviour
{
    /// <summary>
    /// The <see cref="OperationCanceledException.CancellationToken"/> is not set to the
    /// one that as passed to Rx. (This is the expected behaviour on .NET 4.5.)
    /// </summary>
    NotFlowedToOperationCanceledException,

    /// <summary>
    /// The <see cref="OperationCanceledException.CancellationToken"/> is set to the
    /// one that as passed to Rx. (This is the expected behaviour on .NET 4.6 and later.)
    /// </summary>
    FlowedToOperationCanceledException
}
