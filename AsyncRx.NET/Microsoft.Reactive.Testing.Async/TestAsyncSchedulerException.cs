// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Signals a defect the virtual-time pump detected in how a test's asynchronous work
/// executed — work that never completed, work that escaped to a real thread, and similar.
/// These are always reported as failures with a diagnosis; the pump never hangs and never
/// silently drops work.
/// </summary>
public class TestAsyncSchedulerException : Exception
{
    /// <summary>
    /// Creates a <see cref="TestAsyncSchedulerException"/> with the given message.
    /// </summary>
    /// <param name="message">The message describing the error.</param>
    public TestAsyncSchedulerException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a <see cref="TestAsyncSchedulerException"/> with the given message and inner
    /// exception.
    /// </summary>
    /// <param name="message">The message describing the error.</param>
    /// <param name="innerException">The inner exception that caused this exception.</param>
    public TestAsyncSchedulerException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
