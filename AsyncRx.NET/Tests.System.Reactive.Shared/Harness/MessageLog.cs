// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

/// <summary>Recorded notifications, assertable in the shared (sync) vocabulary; a failure is prefixed with the query.</summary>
public readonly struct MessageLog<T>(IRxTarget target, object native, string query)
{
    public object Native => native;

    public void AssertEqual(params Recorded<Notification<T>>[] expected)
    {
        try
        {
            target.AssertEqual(this, expected);
        }
        catch (Exception ex) when (query != "")
        {
            throw new AssertFailedException($"Messages of {query}: {ex.Message}", ex);
        }
    }

    public void AssertEqual(IEnumerable<Recorded<Notification<T>>> expected) => AssertEqual(expected.ToArray());
}
