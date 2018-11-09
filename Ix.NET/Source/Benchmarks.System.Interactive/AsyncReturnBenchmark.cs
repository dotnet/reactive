// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Interactive
{
    [MemoryDiagnoser]
    public class AsyncReturnBenchmark
    {
        [Benchmark]
        public async ValueTask ToAsyncEnumerable()
        {
            await new[] { 1 }.ToAsyncEnumerable().ForEachAsync(v => { }).ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask Direct()
        {
            await AsyncEnumerableEx.Return(1).ForEachAsync(v => { }).ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask Iterator()
        {
            await new ReturnIterator<int>(1).ForEachAsync(v => { }).ConfigureAwait(false);
        }

    }

    internal sealed class ReturnIterator<T> : AsyncIterator<T>
    {
        private readonly T _value;

        public override AsyncIterator<T> Clone()
        {
            return new ReturnIterator<T>(_value);
        }

        public ReturnIterator(T value)
        {
            _value = value;
        }

        protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
        {
            if (state == AsyncIteratorState.Allocated)
            {
                current = _value;
                state = AsyncIteratorState.Disposed;
                return true;
            }

            await DisposeAsync().ConfigureAwait(false);
            return false;
        }
    }

    internal abstract class AsyncIterator<TSource> : IAsyncEnumerable<TSource>, IAsyncEnumerator<TSource>
    {
        private readonly int _threadId;

        private bool _currentIsInvalid = true;

        internal TSource current;
        internal AsyncIteratorState state = AsyncIteratorState.New;
        internal CancellationToken token;

        protected AsyncIterator()
        {
            _threadId = Environment.CurrentManagedThreadId;
        }

        public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken token)
        {
            var enumerator = state == AsyncIteratorState.New && _threadId == Environment.CurrentManagedThreadId
                ? this
                : Clone();

            enumerator.state = AsyncIteratorState.Allocated;
            enumerator.token = token;

            try
            {
                enumerator.OnGetEnumerator(token);
            }
            catch
            {
                enumerator.DisposeAsync(); // REVIEW: fire-and-forget?
                throw;
            }

            return enumerator;
        }

        public virtual ValueTask DisposeAsync()
        {
            current = default;
            state = AsyncIteratorState.Disposed;

            return new ValueTask();
        }

        public TSource Current
        {
            get
            {
                if (_currentIsInvalid)
                    throw new InvalidOperationException("Enumerator is in an invalid state");

                return current;
            }
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            // Note: MoveNext *must* be implemented as an async method to ensure
            // that any exceptions thrown from the MoveNextCore call are handled 
            // by the try/catch, whether they're sync or async

            if (state == AsyncIteratorState.Disposed)
            {
                return false;
            }

            try
            {
                var result = await MoveNextCore(token).ConfigureAwait(false);

                _currentIsInvalid = !result; // if move next is false, invalid otherwise valid

                return result;
            }
            catch
            {
                _currentIsInvalid = true;
                await DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }

        public abstract AsyncIterator<TSource> Clone();

        protected abstract ValueTask<bool> MoveNextCore(CancellationToken cancellationToken);

        protected virtual void OnGetEnumerator(CancellationToken cancellationToken)
        {
        }
    }

    internal enum AsyncIteratorState
    {
        New = 0,
        Allocated = 1,
        Iterating = 2,
        Disposed = -1,
    }
}
