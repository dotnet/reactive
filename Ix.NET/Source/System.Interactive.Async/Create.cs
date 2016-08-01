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
        public static IAsyncEnumerable<T> CreateEnumerable<T>(Func<IAsyncEnumerator<T>> getEnumerator)
        {
            return new AnonymousAsyncEnumerable<T>(getEnumerator);
        }

        public static IAsyncEnumerator<T> CreateEnumerator<T>(Func<CancellationToken, Task<bool>> moveNext, Func<T> current, Action dispose)
        {
            return new AnonymousAsyncEnumerator<T>(moveNext, current, dispose);
        }

        private static IAsyncEnumerator<T> CreateEnumerator<T>(Func<CancellationToken, Task<bool>> moveNext, Func<T> current,
                                                               Action dispose, IDisposable enumerator)
        {
            return CreateEnumerator(
                async ct =>
                {
                    using (ct.Register(dispose))
                    {
                        try
                        {
                            var result = await moveNext(ct)
                                             .ConfigureAwait(false);
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

        private static IAsyncEnumerator<T> CreateEnumerator<T>(Func<CancellationToken, TaskCompletionSource<bool>, Task<bool>> moveNext, Func<T> current, Action dispose)
        {
            var self = default(IAsyncEnumerator<T>);
            self = new AnonymousAsyncEnumerator<T>(
                async ct =>
                {
                    var tcs = new TaskCompletionSource<bool>();

                    var stop = new Action(
                        () =>
                        {
                            self.Dispose();
                            tcs.TrySetCanceled();
                        });

                    using (ct.Register(stop))
                    {
                        return await moveNext(ct, tcs)
                                   .ConfigureAwait(false);
                    }
                },
                current,
                dispose
            );
            return self;
        }

        

        private class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly Func<IAsyncEnumerator<T>> getEnumerator;

            public AnonymousAsyncEnumerable(Func<IAsyncEnumerator<T>> getEnumerator)
            {
                this.getEnumerator = getEnumerator;
            }

            public IAsyncEnumerator<T> GetEnumerator()
            {
                return getEnumerator();
            }
        }

        private class AnonymousAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly Func<T> _current;
            private readonly Action _dispose;
            private readonly Func<CancellationToken, Task<bool>> _moveNext;
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

            public T Current => _current();

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _dispose();
                }
            }
        }
    }
}