// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>An enumerable whose <c>GetEnumerator</c> throws.</summary>
/// <typeparam name="T">The type of the elements it would have had.</typeparam>
/// <param name="ex">The exception to throw.</param>
/// <remarks>
/// Rx.NET's <c>RogueEnumerable</c>, a top-level type in its test assembly, for the scenarios
/// that check an operator ends its result with the exception an enumerable throws on
/// enumeration.
/// </remarks>
public sealed class RogueEnumerable<T>(Exception ex) : IEnumerable<T>
{
    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => throw ex;

    IEnumerator IEnumerable.GetEnumerator() =>
        throw new NotImplementedException();
}
