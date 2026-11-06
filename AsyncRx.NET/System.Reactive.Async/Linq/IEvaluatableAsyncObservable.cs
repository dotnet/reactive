// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // A sequence defined by a factory, which a concatenating sink can evaluate to see what it
    // stands for (a further concatenation, say) before subscribing; Rx.NET's
    // IEvaluatableObservable, which Defer implements for the same purpose.
    internal interface IEvaluatableAsyncObservable<TSource>
    {
        ValueTask<IAsyncObservable<TSource>> EvalAsync();
    }
}
