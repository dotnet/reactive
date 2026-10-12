// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{

    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Concat<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new ConcatAsyncObservable<TSource>(new[] { first, second });
        }

        public static IAsyncObservable<TSource> Concat<TSource>(params IAsyncObservable<TSource>[] sources) => Concat((IEnumerable<IAsyncObservable<TSource>>)sources);

        public static IAsyncObservable<TSource> Concat<TSource>(this IAsyncObservable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            // As in Rx.NET: a merge that subscribes to one inner sequence at a time.
            return sources.Merge(1);
        }

        public static IAsyncObservable<TSource> Concat<TSource>(this IAsyncObservable<Task<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            // As in Rx.NET: each task becomes a sequence of its result, concatenated in order.
            return sources.Select(static task => task.ToAsyncObservable()).Concat();
        }

        public static IAsyncObservable<TSource> Concat<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new ConcatAsyncObservable<TSource>(sources);
        }
    }

    // A port of Rx.NET's Concat over its TailRecursiveSink. The sink keeps a stack of enumerators
    // of sources and subscribes to each source with itself as the observer, so that a chain of
    // concatenations, however it was built, is one subscription deep at any moment: a source
    // that is itself a Concat is flattened into the stack rather than subscribed to, and a Defer
    // is evaluated to see whether it yields one. A trampoline keeps a source that completes
    // synchronously from re-entering the drain, so that neither the observer chain nor the call
    // stack grows with the length of the chain, which is what lets a recursively defined
    // sequence (Defer(() => xs.Concat(f())) ) run without exhausting the stack.
    internal sealed class ConcatAsyncObservable<TSource> : AsyncObservableBase<TSource>, IConcatenatableAsyncObservable<TSource>
    {
        private readonly IEnumerable<IAsyncObservable<TSource>> _sources;

        public ConcatAsyncObservable(IEnumerable<IAsyncObservable<TSource>> sources)
        {
            _sources = sources;
        }

        public IEnumerable<IAsyncObservable<TSource>> GetSources() => _sources;

        protected override async ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TSource> observer)
        {
            var sink = new ConcatSink<TSource>(observer);

            await sink.RunAsync(_sources).ConfigureAwait(false);

            return sink;
        }
    }

    internal sealed class ConcatSink<TSource> : IAsyncObserver<TSource>, IAsyncDisposable
    {
        private readonly IAsyncObserver<TSource> _observer;
        private readonly Stack<IEnumerator<IAsyncObservable<TSource>>> _stack = new();
        private readonly SerialAsyncDisposable _current = new();
        private int _trampoline;
        private bool _isDisposed;

        public ConcatSink(IAsyncObserver<TSource> observer)
        {
            _observer = observer;
        }

        public async ValueTask RunAsync(IEnumerable<IAsyncObservable<TSource>> sources)
        {
            IEnumerator<IAsyncObservable<TSource>> enumerator;

            try
            {
                enumerator = sources.GetEnumerator();
            }
            catch (Exception ex)
            {
                Volatile.Write(ref _isDisposed, true);
                await _observer.OnErrorAsync(ex).ConfigureAwait(false);
                return;
            }

            _stack.Push(enumerator);

            await DrainAsync().ConfigureAwait(false);
        }

        public ValueTask OnNextAsync(TSource value) => _observer.OnNextAsync(value);

        public ValueTask OnErrorAsync(Exception error)
        {
            Volatile.Write(ref _isDisposed, true);
            return _observer.OnErrorAsync(error);
        }

        // The completed source's subscription has been released by the time this is called (see
        // the subscribe in DrainAsync); the drain moves on to the next source.
        public ValueTask OnCompletedAsync() => DrainAsync();

        public async ValueTask DisposeAsync()
        {
            Volatile.Write(ref _isDisposed, true);

            if (Interlocked.Increment(ref _trampoline) == 1)
            {
                DisposeStack();
            }

            Interlocked.Decrement(ref _trampoline);

            await _current.DisposeAsync().ConfigureAwait(false);
        }

        private void DisposeStack()
        {
            while (_stack.Count != 0)
            {
                _stack.Pop().Dispose();
            }
        }

        private async ValueTask DrainAsync()
        {
            if (Interlocked.Increment(ref _trampoline) != 1)
            {
                return;
            }

            for (; ; )
            {
                if (Volatile.Read(ref _isDisposed))
                {
                    DisposeStack();
                }
                else if (_stack.Count != 0)
                {
                    var currentEnumerator = _stack.Peek();
                    var next = default(IAsyncObservable<TSource>);

                    try
                    {
                        if (currentEnumerator.MoveNext())
                        {
                            next = currentEnumerator.Current;
                        }
                    }
                    catch (Exception ex)
                    {
                        currentEnumerator.Dispose();
                        Volatile.Write(ref _isDisposed, true);
                        await _observer.OnErrorAsync(ex).ConfigureAwait(false);
                        continue;
                    }

                    if (next == null)
                    {
                        _stack.Pop();
                        currentEnumerator.Dispose();
                        continue;
                    }

                    IEnumerator<IAsyncObservable<TSource>> inner;

                    try
                    {
                        while (next is IEvaluatableAsyncObservable<TSource> evaluatable)
                        {
                            next = await evaluatable.EvalAsync().ConfigureAwait(false);
                        }

                        if (next is IConcatenatableAsyncObservable<TSource> concatenatable)
                        {
                            inner = concatenatable.GetSources().GetEnumerator();
                            _stack.Push(inner);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Volatile.Write(ref _isDisposed, true);
                        await _observer.OnErrorAsync(ex).ConfigureAwait(false);
                        continue;
                    }

                    // Subscribe with this sink as the observer, holding the subscription only
                    // until the source completes. A source that completes within the subscribe
                    // call re-enters OnCompletedAsync, which the trampoline turns into one more
                    // turn of this loop rather than a nested drain.
                    var subscription = await AsyncObservable.SubscribeReleasingOnCompletedAsync(next, this).ConfigureAwait(false);

                    await _current.AssignAsync(subscription).ConfigureAwait(false);
                }
                else
                {
                    Volatile.Write(ref _isDisposed, true);
                    await _observer.OnCompletedAsync().ConfigureAwait(false);
                }

                if (Interlocked.Decrement(ref _trampoline) == 0)
                {
                    break;
                }
            }
        }
    }
}
