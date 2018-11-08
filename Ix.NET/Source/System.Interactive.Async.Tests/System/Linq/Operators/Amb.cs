// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Tests
{
    public class Amb : AsyncEnumerableExTests
    {
        [Fact]
        public void TwoArg_First_Wins_NonEmpty()
        {
            var en = AsyncEnumerableEx.Amb(
                    AsyncEnumerable.Range(1, 5),
                    AsyncEnumerable.Range(6, 5).Select(async v =>
                    {
                        if (v == 6)
                        {
                            await Task.Delay(200);
                        }
                        return v;
                    })
                ).GetAsyncEnumerator();

            try
            {
                HasNext(en, 1);
                HasNext(en, 2);
                HasNext(en, 3);
                HasNext(en, 4);
                HasNext(en, 5);
                NoNext(en);

            }
            finally
            {
                en.DisposeAsync().AsTask().Wait();
            }
        }

        [Fact]
        public void TwoArg_Second_Wins_NonEmpty()
        {
            var en = AsyncEnumerableEx.Amb(
                    AsyncEnumerable.Range(1, 5).Select(async v =>
                    {
                        if (v == 1)
                        {
                            await Task.Delay(200);
                        }
                        return v;
                    }),
                    AsyncEnumerable.Range(6, 5)
                ).GetAsyncEnumerator();

            try
            {
                HasNext(en, 6);
                HasNext(en, 7);
                HasNext(en, 8);
                HasNext(en, 9);
                HasNext(en, 10);
                NoNext(en);
            }
            finally
            {
                en.DisposeAsync().AsTask().Wait();
            }
        }

        [Fact]
        public void TwoArg_First_Wins_Empty()
        {
            var en = AsyncEnumerableEx.Amb(
                    AsyncEnumerable.Empty<int>(),
                    AsyncEnumerable.Range(6, 5).Select(async v =>
                    {
                        if (v == 6)
                        {
                            await Task.Delay(200);
                        }
                        return v;
                    })
                ).GetAsyncEnumerator();

            try
            {
                NoNext(en);
            }
            finally
            {
                en.DisposeAsync().AsTask().Wait();
            }
        }

        [Fact]
        public void TwoArg_Second_Wins_Empty()
        {
            var en = AsyncEnumerableEx.Amb(
                    AsyncEnumerable.Range(1, 5).Select(async v =>
                    {
                        if (v == 1)
                        {
                            await Task.Delay(200);
                        }
                        return v;
                    }),
                    AsyncEnumerable.Empty<int>()
                ).GetAsyncEnumerator();

            try
            {
                NoNext(en);
            }
            finally
            {
                en.DisposeAsync().AsTask().Wait();
            }
        }

        [Fact]
        public void TwoArg_Cancel()
        {
            var cts = new CancellationTokenSource();

            var en = AsyncEnumerableEx.Amb(
                    AsyncEnumerable.Range(1, 5).Select(async (v, t) =>
                    {
                        if (v == 1)
                        {
                            await Task.Delay(2000, t);
                        }
                        return v;
                    }),
                    AsyncEnumerable.Range(6, 5).Select(async (v, t) =>
                    {
                        if (v == 6)
                        {
                            await Task.Delay(2000, t);
                        }
                        return v;
                    })
                ).GetAsyncEnumerator(cts.Token);

            try
            {
                en.MoveNextAsync();

                Task.Delay(200).Wait();

                cts.Cancel();

                Task.Delay(200).Wait();
            }
            finally
            {
                Assert.True(en.DisposeAsync().AsTask().Wait(1000));
            }
        }
    }

    internal static class AsyncTestEx
    {
        internal static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> mapper)
        {
            return new SelectTokenTask<TSource, TResult>(source, mapper);
        }

        private sealed class SelectTokenTask<TSource, TResult> : IAsyncEnumerable<TResult>
        {
            private readonly IAsyncEnumerable<TSource> _source;

            private readonly Func<TSource, CancellationToken, Task<TResult>> _mapper;

            public SelectTokenTask(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> mapper)
            {
                _source = source;
                _mapper = mapper;
            }

            public IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new SelectTokenTaskEnumerator(_source.GetAsyncEnumerator(cancellationToken), _mapper, cancellationToken);
            }

            private sealed class SelectTokenTaskEnumerator : IAsyncEnumerator<TResult>
            {
                private readonly IAsyncEnumerator<TSource> _source;

                private readonly Func<TSource, CancellationToken, Task<TResult>> _mapper;

                private readonly CancellationToken _token;

                public SelectTokenTaskEnumerator(IAsyncEnumerator<TSource> source, Func<TSource, CancellationToken, Task<TResult>> mapper, CancellationToken token)
                {
                    _source = source;
                    _mapper = mapper;
                    _token = token;
                }

                public TResult Current { get; private set; }

                public ValueTask DisposeAsync()
                {
                    return _source.DisposeAsync();
                }

                public async ValueTask<bool> MoveNextAsync()
                {
                    if (await _source.MoveNextAsync())
                    {
                        Current = await _mapper(_source.Current, _token);
                        return true;
                    }
                    return false;
                }
            }
        }
    }

}
