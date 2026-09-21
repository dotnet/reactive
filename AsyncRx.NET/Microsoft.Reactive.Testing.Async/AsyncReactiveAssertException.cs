// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Microsoft.Reactive.Testing.Async;

/// <summary>Thrown when an async trace assertion fails.</summary>
public class AsyncReactiveAssertException : Exception
{
    public AsyncReactiveAssertException(string message)
        : base(message)
    {
    }
}
