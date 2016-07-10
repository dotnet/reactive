// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        private static IAsyncEnumerable<T> Create<T>(Func<IAsyncEnumerator<T>> getEnumerator)
        {
            return new AnonymousAsyncEnumerable<T>(getEnumerator);
        }

        private class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private Func<IAsyncEnumerator<T>> getEnumerator;

            public AnonymousAsyncEnumerable(Func<IAsyncEnumerator<T>> getEnumerator)
            {
                this.getEnumerator = getEnumerator;
            }

            public IAsyncEnumerator<T> GetEnumerator()
            {
                return getEnumerator();
            }
        }

        private static IAsyncEnumerator<T> Create<T>(Func<CancellationToken, Task<bool>> moveNext, Func<T> current,
            Action dispose, IDisposable enumerator)
        {
            return Create(async ct =>
            {
                using (ct.Register(dispose))
                {
                    try
                    {
                        var result = await moveNext(ct).ConfigureAwait(false);
                        if (!result)
                        {
                            enumerator?.Dispose();
                        }
                        return result;
                    }
                    catch
                    {
                        enumerator?.Dispose();
                        throw;
                    }
                }
            }, current, dispose);
        }

        private static IAsyncEnumerator<T> Create<T>(Func<CancellationToken, Task<bool>> moveNext, Func<T> current, Action dispose)
        {
            return new AnonymousAsyncEnumerator<T>(moveNext, current, dispose);
        }

        private static IAsyncEnumerator<T> Create<T>(Func<CancellationToken, TaskCompletionSource<bool>, Task<bool>> moveNext, Func<T> current, Action dispose)
        {
            var self = default(IAsyncEnumerator<T>);
            self = new AnonymousAsyncEnumerator<T>(
                async ct =>
                {
                    var tcs = new TaskCompletionSource<bool>();

                    var stop = new Action(() =>
                    {
                        self.Dispose();
                        tcs.TrySetCanceled();
                    });

                    using (ct.Register(stop))
                    {
                        return await moveNext(ct, tcs);
                    }
                },
                current,
                dispose
            );
            return self;
        }

        private class AnonymousAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly Func<CancellationToken, Task<bool>> _moveNext;
            private readonly Func<T> _current;
            private readonly Action _dispose;
            private bool _disposed;

            public AnonymousAsyncEnumerator(Func<CancellationToken, Task<bool>> moveNext, Func<T> current, Action dispose)
            {
                _moveNext = moveNext;
                _current = current;
                _dispose = dispose;
            }

            public Task<bool> MoveNext(CancellationToken cancellationToken)
            {
                if (_disposed)
                    return TaskExt.False;

                return _moveNext(cancellationToken);
            }

            public T Current
            {
                get
                {
                    return _current();
                }
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _dispose();
                }
            }
        }

        public static IAsyncEnumerable<TValue> Return<TValue>(TValue value)
        {
            return new[] { value }.ToAsyncEnumerable();
        }

        public static IAsyncEnumerable<TValue> Throw<TValue>(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return Create(() => Create<TValue>(
                ct => TaskExt.Throw<bool>(exception),
                () => { throw new InvalidOperationException(); },
                () => { })
            );
        }

        public static IAsyncEnumerable<TValue> Never<TValue>()
        {
            return Create(() => Create<TValue>(
                (ct, tcs) => tcs.Task,
                () => { throw new InvalidOperationException(); },
                () => { })
            );
        }

        public static IAsyncEnumerable<TValue> Empty<TValue>()
        {
            return Create(() => Create<TValue>(
                ct => TaskExt.False,
                () => { throw new InvalidOperationException(); },
                () => { })
            );
        }

        public static IAsyncEnumerable<int> Range(int start, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Enumerable.Range(start, count).ToAsyncEnumerable();
        }

        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Enumerable.Repeat(element, count).ToAsyncEnumerable();
        }

        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element)
        {
            return Create(() =>
            {
                return Create(
                    ct => TaskExt.True,
                    () => element,
                    () => { }
                );
            });
        }

        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<IAsyncEnumerable<TSource>> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return Create(() => factory().GetEnumerator());
        }

        public static IAsyncEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Create(() =>
            {
                var i = initialState;
                var started = false;
                var current = default(TResult);

                return Create(
                    ct =>
                    {
                        var b = false;
                        try
                        {
                            if (started)
                                i = iterate(i);

                            b = condition(i);

                            if (b)
                                current = resultSelector(i);
                        }
                        catch (Exception ex)
                        {
                            return TaskExt.Throw<bool>(ex);
                        }

                        if (!b)
                            return TaskExt.False;

                        if (!started)
                            started = true;

                        return TaskExt.True;
                    },
                    () => current,
                    () => { }
                );
            });
        }

        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw new ArgumentNullException(nameof(enumerableFactory));

            return Create(() =>
            {
                var resource = resourceFactory();
                var e = default(IAsyncEnumerator<TSource>);

                try
                {
                    e = enumerableFactory(resource).GetEnumerator();
                }
                catch (Exception)
                {
                    resource.Dispose();
                    throw;
                }

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, resource, e);

                var current = default(TSource);

                return Create(
                    async ct =>
                    {
                        bool res;
                        try
                        {
                            res = await e.MoveNext(cts.Token).ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                            d.Dispose();
                            throw;
                        }
                        if (res)
                        {
                            current = e.Current;
                            return true;
                        }
                        d.Dispose();
                        return false;
                    },
                    () => current,
                    d.Dispose,
                    null
                );
            });
        }
    }
}
