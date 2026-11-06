// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.Cast<TResult>(CastSeq<TResult> seq) =>
        _bridge.Run<Seq<TResult>>(CastImpl<TResult>, seq.Source);

    Realized<Seq<TResult>> ISeqVisitor.OfType<TResult>(OfTypeSeq<TResult> seq) =>
        _bridge.Run<Seq<TResult>>(OfTypeImpl<TResult>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DefaultIfEmpty<T>(DefaultIfEmptySeq<T> seq) =>
        _bridge.Run<Seq<T>>(DefaultIfEmptyImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DefaultIfEmptyValue<T>(DefaultIfEmptyValueSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DefaultIfEmptyValueImpl<T>, seq.Source, seq.DefaultValue);

    private static IAsyncObservable<TResult> CastImpl<TResult>(IAsyncObservable<object> source) =>
        source.Cast<TResult>();

    private static IAsyncObservable<TResult> OfTypeImpl<TResult>(
        IAsyncObservable<object?> source) =>
        source.OfType<TResult>();

    private static IAsyncObservable<T> DefaultIfEmptyImpl<T>(
        IAsyncObservable<T> source) =>
        source.DefaultIfEmpty();

    private static IAsyncObservable<T> DefaultIfEmptyValueImpl<T>(
        IAsyncObservable<T> source,
        T defaultValue) =>
        source.DefaultIfEmpty(defaultValue);
}
