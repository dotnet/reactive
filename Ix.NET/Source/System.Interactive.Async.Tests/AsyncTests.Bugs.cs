// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public partial class AsyncTests
    {
        public AsyncTests()
        {
            TaskScheduler.UnobservedTaskException += (o, e) =>
            {
            };
        }

        [Fact]
        public async void CorrectDispose()
        {
            var disposed = new TaskCompletionSource<bool>();

            var xs = new[] { 1, 2, 3 }.WithDispose(() =>
            {
                disposed.TrySetResult(true);
            }).ToAsyncEnumerable();

            var ys = xs.Select(x => x + 1);

            var e = ys.GetAsyncEnumerator();

            // We have to call move next because otherwise the internal enumerator is never allocated
            await e.MoveNextAsync();
            await e.DisposeAsync();

            await disposed.Task;

            Assert.True(await disposed.Task);

            Assert.False(await e.MoveNextAsync());

            var next = await e.MoveNextAsync();
            Assert.False(next);
        }

        [Fact]
        public async Task DisposesUponError()
        {
            var disposed = new TaskCompletionSource<bool>();

            var xs = new[] { 1, 2, 3 }.WithDispose(() =>
            {
                disposed.SetResult(true);
            }).ToAsyncEnumerable();

            var ex = new Exception("Bang!");
            var ys = xs.Select(x => { if (x == 1) throw ex; return x; });

            var e = ys.GetAsyncEnumerator();
            await AssertX.ThrowsAsync<Exception>(() => e.MoveNextAsync());

            var result = await disposed.Task;
            Assert.True(result);
        }

        [Fact]
        public async Task TakeOneFromSelectMany()
        {
            var ret0 = new[] { 0 }.ToAsyncEnumerable();
            var retCheck = new[] { "Check" }.ToAsyncEnumerable();

            var enumerable =
                ret0
                .SelectMany(_ => retCheck)
                .Take(1)
                .Do(_ => { });

            Assert.Equal("Check", await enumerable.FirstAsync());
        }

        [Fact]
        public async Task SelectManyDisposeInvokedOnlyOnceAsync()
        {
            var disposeCounter = new DisposeCounter();

            var result = await new[] { 1 }.ToAsyncEnumerable().SelectMany(i => disposeCounter).Select(i => i).ToListAsync();

            Assert.Empty(result);
            Assert.Equal(1, disposeCounter.DisposeCount);
        }

        [Fact]
        public async Task SelectManyInnerDisposeAsync()
        {
            var disposes = Enumerable.Range(0, 10).Select(_ => new DisposeCounter()).ToList();

            var result = await AsyncEnumerable.Range(0, 10).SelectMany(i => disposes[i]).Select(i => i).ToListAsync();

            Assert.Empty(result);
            Assert.True(disposes.All(d => d.DisposeCount == 1));
        }

        [Fact]
        public void DisposeAfterCreation()
        {
            var enumerable = new[] { 1 }.ToAsyncEnumerable() as IDisposable;
            enumerable?.Dispose();
        }

        private class DisposeCounter : IAsyncEnumerable<object?>
        {
            public int DisposeCount { get; private set; }

            public IAsyncEnumerator<object?> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                return new Enumerator(this);
            }

            private class Enumerator : IAsyncEnumerator<object?>
            {
                private readonly DisposeCounter _disposeCounter;

                public Enumerator(DisposeCounter disposeCounter)
                {
                    _disposeCounter = disposeCounter;
                }

                public ValueTask DisposeAsync()
                {
                    _disposeCounter.DisposeCount++;
                    return default;
                }

                public ValueTask<bool> MoveNextAsync()
                {
                    return new ValueTask<bool>(Task.Factory.StartNew(() => false));
                }

                public object? Current { get; private set; }
            }
        }
    }

    internal static class MyExt
    {
        public static IEnumerable<T> WithDispose<T>(this IEnumerable<T> source, Action a)
        {
            return new Enumerable<T>(() =>
            {
                var e = source.GetEnumerator();
                return new Enumerator<T>(e.MoveNext, () => e.Current, () => { e.Dispose(); a(); });
            });
        }

        private sealed class Enumerable<T> : IEnumerable<T>
        {
            private readonly Func<IEnumerator<T>> _getEnumerator;

            public Enumerable(Func<IEnumerator<T>> getEnumerator)
            {
                _getEnumerator = getEnumerator;
            }

            public IEnumerator<T> GetEnumerator() => _getEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class Enumerator<T> : IEnumerator<T>
        {
            private readonly Func<bool> _moveNext;
            private readonly Func<T> _current;
            private readonly Action _dispose;

            public Enumerator(Func<bool> moveNext, Func<T> current, Action dispose)
            {
                _moveNext = moveNext;
                _current = current;
                _dispose = dispose;
            }

            public T Current => _current();

            public void Dispose() => _dispose();

            object IEnumerator.Current => Current;

            public bool MoveNext() => _moveNext();

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
