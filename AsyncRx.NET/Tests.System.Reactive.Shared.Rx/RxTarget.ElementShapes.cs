// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.Cast<TResult>(CastSeq<TResult> seq) =>
        _bridge.Run<Seq<TResult>>(CastImpl<TResult>, seq.Source);

    Realized<Seq<TResult>> ISeqVisitor.OfType<TResult>(OfTypeSeq<TResult> seq) =>
        _bridge.Run<Seq<TResult>>(OfTypeImpl<TResult>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DefaultIfEmpty<T>(DefaultIfEmptySeq<T> seq) =>
        _bridge.Run<Seq<T>>(DefaultIfEmptyImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DefaultIfEmptyValue<T>(DefaultIfEmptyValueSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DefaultIfEmptyValueImpl<T>, seq.Source, seq.DefaultValue);

    private static IObservable<TResult> CastImpl<TResult>(IObservable<object> source) =>
        source.Cast<TResult>();

    private static IObservable<TResult> OfTypeImpl<TResult>(
        IObservable<object?> source) =>
        source.OfType<TResult>();

    private static IObservable<T?> DefaultIfEmptyImpl<T>(
        IObservable<T> source) =>
        source.DefaultIfEmpty();

    private static IObservable<T> DefaultIfEmptyValueImpl<T>(
        IObservable<T> source,
        T defaultValue) =>
        source.DefaultIfEmpty(defaultValue);
}
