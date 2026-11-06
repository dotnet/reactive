// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // A port of Rx.NET's TailRecursiveSink, the sink under Concat and Catch. It keeps a stack of
    // enumerators of sources and subscribes to each source with itself as the observer, so that
    // a chain of concatenations (or catches), however it was built, is one subscription deep at
    // any moment: a source that is itself such a chain is flattened into the stack rather than
    // subscribed to (Extract), and a Defer is evaluated to see whether it yields one. A
    // trampoline keeps a source that completes (or fails) synchronously within the subscribe
    // call from re-entering the drain, so that neither the observer chain nor the call stack
    // grows with the length of the chain. The derived sink says what a source's error and
    // completion mean (move on, or end the result), what to do when the stack runs dry, and
    // whether a Defer that throws ends the result or counts as a failed source.
    internal abstract class TailRecursiveAsyncSink<TSource> : IAsyncObserver<TSource>, IAsyncDisposable
    {
        private readonly Stack<IEnumerator<IAsyncObservable<TSource>>> _stack = new();
        private readonly SerialAsyncDisposable _current = new();
        private int _trampoline;
        private bool _isDisposed;

        protected TailRecursiveAsyncSink(IAsyncObserver<TSource> observer)
        {
            Observer = observer;
        }

        protected IAsyncObserver<TSource> Observer { get; }

        public async ValueTask RunAsync(IEnumerable<IAsyncObservable<TSource>> sources)
        {
            IEnumerator<IAsyncObservable<TSource>> enumerator;

            try
            {
                enumerator = sources.GetEnumerator();
            }
            catch (Exception ex)
            {
                await FailAsync(ex).ConfigureAwait(false);
                return;
            }

            _stack.Push(enumerator);

            await DrainAsync().ConfigureAwait(false);
        }

        public virtual ValueTask OnNextAsync(TSource value) => Observer.OnNextAsync(value);

        public abstract ValueTask OnErrorAsync(Exception error);

        public abstract ValueTask OnCompletedAsync();

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

        // The sources a chain of the sink's own kind stands for, or null for any other source.
        protected abstract IEnumerable<IAsyncObservable<TSource>> Extract(IAsyncObservable<TSource> source);

        // Called once the stack has run dry; the sink ends the result as it sees fit.
        protected abstract ValueTask DoneAsync();

        // Called when a source fails to evaluate (a Defer that throws). By default that ends the
        // result with the error; a sink may count it as a failed source and carry on.
        protected virtual ValueTask EvaluationFailedAsync(Exception error) => FailAsync(error);

        // How the sink subscribes to a source; by default with itself as the observer.
        protected virtual ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObservable<TSource> source) =>
            source.SubscribeSafeAsync(this);

        // Moves on to the next source (the current one's completion or failure, by the derived
        // sink's lights, has been handled).
        protected ValueTask RecurseAsync() => DrainAsync();

        // Ends the result with an error.
        protected ValueTask FailAsync(Exception error)
        {
            Volatile.Write(ref _isDisposed, true);
            return Observer.OnErrorAsync(error);
        }

        // Ends the result with its completion.
        protected ValueTask CompleteAsync()
        {
            Volatile.Write(ref _isDisposed, true);
            return Observer.OnCompletedAsync();
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
                        await FailAsync(ex).ConfigureAwait(false);
                        continue;
                    }

                    if (next == null)
                    {
                        _stack.Pop();
                        currentEnumerator.Dispose();
                        continue;
                    }

                    IEnumerable<IAsyncObservable<TSource>> inner;

                    try
                    {
                        while (next is IEvaluatableAsyncObservable<TSource> evaluatable)
                        {
                            next = await evaluatable.EvalAsync().ConfigureAwait(false);
                        }

                        inner = Extract(next);
                    }
                    catch (Exception ex)
                    {
                        await EvaluationFailedAsync(ex).ConfigureAwait(false);
                        continue;
                    }

                    if (inner != null)
                    {
                        _stack.Push(inner.GetEnumerator());
                        continue;
                    }

                    // A source that ends within the subscribe call re-enters the sink, which the
                    // trampoline turns into one more turn of this loop rather than a nested drain.
                    var subscription = await SubscribeAsync(next).ConfigureAwait(false);

                    await _current.AssignAsync(subscription).ConfigureAwait(false);
                }
                else
                {
                    Volatile.Write(ref _isDisposed, true);
                    await DoneAsync().ConfigureAwait(false);
                }

                if (Interlocked.Decrement(ref _trampoline) == 0)
                {
                    break;
                }
            }
        }
    }
}
