// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Switch</c> operator, as an extension method on nested sequences.</summary>
public static class SwitchExtensions
{
    /// <summary>Describes <c>sources.Switch()</c> over a nested sequence.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequence of inner observable sequences.</param>
    public static Seq<T> Switch<T>(this Seq<Seq<T>> sources) => new SwitchSeq<T>(sources);
}
