// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq
{
    // A sequence that is a concatenation of others, which a concatenating sink can flatten into
    // its own list of sources rather than subscribing to as one more layer; Rx.NET's
    // IConcatenatable, which its tail-recursive sink uses for the same purpose.
    internal interface IConcatenatableAsyncObservable<TSource>
    {
        IEnumerable<IAsyncObservable<TSource>> GetSources();
    }
}
