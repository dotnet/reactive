// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Labs.Linq;
using System.Threading.Tasks;

namespace FasterLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            var N = 4;

            Sync(N);
            Async1(N).Wait();
            Async2(N).Wait();
        }

        static void Sync(int n)
        {
            Console.WriteLine("IEnumerable<T> and IFastEnumerable<T>");
            Console.WriteLine();

            var sw = new Stopwatch();

            var N = 10_000_000;

            var next = new Action<int>(_ => { });

            var slowRange = Enumerable.Range(0, N);
            var fastRange = FastEnumerable.Range(0, N);
            var brdgRange = slowRange.ToFastEnumerable();

            var slow = slowRange.Where(x => x % 2 == 0).Select(x => x + 1);
            var fast = fastRange.Where(x => x % 2 == 0).Select(x => x + 1);
            var brdg = brdgRange.Where(x => x % 2 == 0).Select(x => x + 1).ToEnumerable();

            Console.WriteLine("slow.Sum() = " + slow.Aggregate(0, (sum, x) => sum + x));
            Console.WriteLine("fast.Sum() = " + fast.Aggregate(0, (sum, x) => sum + x));
            Console.WriteLine("brdg.Sum() = " + brdg.Aggregate(0, (sum, x) => sum + x));
            Console.WriteLine();

            for (var i = 0; i < n; i++)
            {
                sw.Restart();
                {
                    slow.ForEach(next);
                }
                Console.WriteLine("SLOW " + sw.Elapsed);

                sw.Restart();
                {
                    fast.ForEach(next);
                }
                Console.WriteLine("FAST " + sw.Elapsed);

                sw.Restart();
                {
                    brdg.ForEach(next);
                }
                Console.WriteLine("BRDG " + sw.Elapsed);

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        static async Task Async1(int n)
        {
            Console.WriteLine("IAsyncEnumerable<T> and IAsyncFastEnumerable<T> - Synchronous query operators");
            Console.WriteLine();

            var sw = new Stopwatch();

            var N = 10_000_000;

            var next = new Func<int, Task>(_ => Task.CompletedTask);

            var slowRange = AsyncEnumerable.Range(0, N);
            var fastRange = AsyncFastEnumerable.Range(0, N);
            var brdgRange = slowRange.ToAsyncFastEnumerable();

            var slow = slowRange.Where(x => x % 2 == 0).Select(x => x + 1);
            var fast = fastRange.Where(x => x % 2 == 0).Select(x => x + 1);
            var brdg = brdgRange.Where(x => x % 2 == 0).Select(x => x + 1).ToAsyncEnumerable();

            Console.WriteLine("slow.Sum() = " + slow.Aggregate(0, (sum, x) => sum + x).Result);
            Console.WriteLine("fast.Sum() = " + fast.Aggregate(0, (sum, x) => sum + x).Result);
            Console.WriteLine("brdg.Sum() = " + brdg.Aggregate(0, (sum, x) => sum + x).Result);
            Console.WriteLine();

            for (var i = 0; i < n; i++)
            {
                sw.Restart();
                {
                    await slow.ForEachAsync(next);
                }
                Console.WriteLine("SLOW " + sw.Elapsed);

                sw.Restart();
                {
                    await fast.ForEachAsync(next);
                }
                Console.WriteLine("FAST " + sw.Elapsed);

                sw.Restart();
                {
                    await brdg.ForEachAsync(next);
                }
                Console.WriteLine("BRDG " + sw.Elapsed);

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        static async Task Async2(int n)
        {
            Console.WriteLine("IAsyncEnumerable<T> and IAsyncFastEnumerable<T> - Asynchronous query operators");
            Console.WriteLine();

            var sw = new Stopwatch();

            var N = 10_000_000;

            var next = new Func<int, Task>(_ => Task.CompletedTask);

            var slowRange = AsyncEnumerable.Range(0, N);
            var fastRange = AsyncFastEnumerable.Range(0, N);
            var brdgRange = slowRange.ToAsyncFastEnumerable();

            var slow = slowRange.Where(x => Task.FromResult(x % 2 == 0)).Select(x => Task.FromResult(x + 1));
            var fast = fastRange.Where(x => Task.FromResult(x % 2 == 0)).Select(x => Task.FromResult(x + 1));
            var brdg = brdgRange.Where(x => Task.FromResult(x % 2 == 0)).Select(x => Task.FromResult(x + 1)).ToAsyncEnumerable();

            Console.WriteLine("slow.Sum() = " + slow.Aggregate(0, (sum, x) => sum + x).Result);
            Console.WriteLine("fast.Sum() = " + fast.Aggregate(0, (sum, x) => sum + x).Result);
            Console.WriteLine("brdg.Sum() = " + brdg.Aggregate(0, (sum, x) => sum + x).Result);
            Console.WriteLine();

            for (var i = 0; i < n; i++)
            {
                sw.Restart();
                {
                    await slow.ForEachAsync(next);
                }
                Console.WriteLine("SLOW " + sw.Elapsed);

                sw.Restart();
                {
                    await fast.ForEachAsync(next);
                }
                Console.WriteLine("FAST " + sw.Elapsed);

                sw.Restart();
                {
                    await brdg.ForEachAsync(next);
                }
                Console.WriteLine("BRDG " + sw.Elapsed);

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}

namespace System
{
    public interface IAsyncDisposable
    {
        Task DisposeAsync();
    }
}

namespace System.Collections.Generic
{
    public interface IAsyncEnumerable<out T>
    {
        IAsyncEnumerator<T> GetAsyncEnumerator();
    }

    public interface IAsyncEnumerator<out T> : IAsyncDisposable
    {
        Task<bool> MoveNextAsync();
        T Current { get; }
    }

    public interface IFastEnumerable<out T>
    {
        IFastEnumerator<T> GetEnumerator();
    }

    public interface IFastEnumerator<out T> : IDisposable
    {
        T TryGetNext(out bool success);
    }

    public interface IAsyncFastEnumerable<out T>
    {
        IAsyncFastEnumerator<T> GetAsyncEnumerator();
    }

    public interface IAsyncFastEnumerator<out T> : IAsyncDisposable
    {
        Task<bool> WaitForNextAsync();
        T TryGetNext(out bool success);
    }
}

namespace System.Labs.Linq
{
    internal abstract class Iterator<T> : IEnumerable<T>, IEnumerator<T>
    {
        private readonly int _threadId;
        internal int _state;
        protected T _current;

        protected Iterator()
        {
            _threadId = Environment.CurrentManagedThreadId;
        }

        public abstract Iterator<T> Clone();

        public virtual void Dispose()
        {
            _state = -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Iterator<T> enumerator = _state == 0 && _threadId == Environment.CurrentManagedThreadId ? this : Clone();
            enumerator._state = 1;
            return enumerator;
        }

        public abstract bool MoveNext();

        public T Current => _current;

        object IEnumerator.Current => _current;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Reset() => throw new NotSupportedException();
    }

    internal abstract class AsyncIterator<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>
    {
        private readonly int _threadId;
        internal int _state;
        protected T _current;

        protected AsyncIterator()
        {
            _threadId = Environment.CurrentManagedThreadId;
        }

        public abstract AsyncIterator<T> Clone();

        public virtual Task DisposeAsync()
        {
            _state = -1;
            return Task.CompletedTask;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator()
        {
            AsyncIterator<T> enumerator = _state == 0 && _threadId == Environment.CurrentManagedThreadId ? this : Clone();
            enumerator._state = 1;
            return enumerator;
        }

        public abstract Task<bool> MoveNextAsync();

        public T Current => _current;
    }

    internal abstract class FastIterator<T> : IFastEnumerable<T>, IFastEnumerator<T>
    {
        private readonly int _threadId;
        internal int _state;

        protected FastIterator()
        {
            _threadId = Environment.CurrentManagedThreadId;
        }

        public abstract FastIterator<T> Clone();

        public virtual void Dispose()
        {
            _state = -1;
        }

        public IFastEnumerator<T> GetEnumerator()
        {
            FastIterator<T> enumerator = _state == 0 && _threadId == Environment.CurrentManagedThreadId ? this : Clone();
            enumerator._state = 1;
            return enumerator;
        }

        public abstract T TryGetNext(out bool success);
    }

    internal abstract class AsyncFastIterator<T> : IAsyncFastEnumerable<T>, IAsyncFastEnumerator<T>
    {
        private readonly int _threadId;
        internal int _state;

        protected AsyncFastIterator()
        {
            _threadId = Environment.CurrentManagedThreadId;
        }

        public abstract AsyncFastIterator<T> Clone();

        public virtual Task DisposeAsync()
        {
            _state = -1;
            return Task.CompletedTask;
        }

        public IAsyncFastEnumerator<T> GetAsyncEnumerator()
        {
            AsyncFastIterator<T> enumerator = _state == 0 && _threadId == Environment.CurrentManagedThreadId ? this : Clone();
            enumerator._state = 1;
            return enumerator;
        }

        public abstract Task<bool> WaitForNextAsync();

        public abstract T TryGetNext(out bool success);
    }

    public static class Enumerable
    {
        public static R Aggregate<T, R>(this IEnumerable<T> source, R seed, Func<R, T, R> aggregate)
        {
            var res = seed;

            foreach (var item in source)
            {
                res = aggregate(res, item);
            }

            return res;
        }

        public static IEnumerable<T> Empty<T>() => EmptyIterator<T>.Instance;

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> next)
        {
            foreach (var item in source)
            {
                next(item);
            }
        }

        public static IEnumerable<int> Range(int start, int count) => new RangeIterator(start, count);

        public static IEnumerable<R> Select<T, R>(this IEnumerable<T> source, Func<T, R> selector) => new SelectIterator<T, R>(source, selector);

        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate) => new WhereIterator<T>(source, predicate);

        private sealed class EmptyIterator<T> : IEnumerable<T>, IEnumerator<T>
        {
            public static readonly EmptyIterator<T> Instance = new EmptyIterator<T>();

            public T Current => default(T);

            object IEnumerator.Current => default(T);

            public void Dispose() { }

            public IEnumerator<T> GetEnumerator() => this;

            public bool MoveNext() => false;

            public void Reset() { }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class RangeIterator : Iterator<int>
        {
            private readonly int _start;
            private readonly int _end;
            private int _next;

            public RangeIterator(int start, int count)
            {
                _start = start;
                _end = start + count;
            }

            public override Iterator<int> Clone() => new RangeIterator(_start, _end - _start);

            public override bool MoveNext()
            {
                switch (_state)
                {
                    case 1:
                        _next = _start;
                        _state = 2;
                        goto case 2;

                    case 2:
                        if (_next < _end)
                        {
                            _current = _next++;
                            return true;
                        }
                        break;
                }

                return false;
            }
        }

        private sealed class SelectIterator<T, R> : Iterator<R>
        {
            private readonly IEnumerable<T> _source;
            private readonly Func<T, R> _selector;
            private IEnumerator<T> _enumerator;

            public SelectIterator(IEnumerable<T> source, Func<T, R> selector)
            {
                _source = source;
                _selector = selector;
            }

            public override Iterator<R> Clone() => new SelectIterator<T, R>(_source, _selector);

            public override bool MoveNext()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        if (_enumerator.MoveNext())
                        {
                            _current = _selector(_enumerator.Current);
                            return true;
                        }
                        break;
                }

                return false;
            }

            public override void Dispose() => _enumerator?.Dispose();
        }

        private sealed class WhereIterator<T> : Iterator<T>
        {
            private readonly IEnumerable<T> _source;
            private readonly Func<T, bool> _predicate;
            private IEnumerator<T> _enumerator;

            public WhereIterator(IEnumerable<T> source, Func<T, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            public override Iterator<T> Clone() => new WhereIterator<T>(_source, _predicate);

            public override bool MoveNext()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        while (_enumerator.MoveNext())
                        {
                            var item = _enumerator.Current;

                            if (_predicate(item))
                            {
                                _current = item;
                                return true;
                            }
                        }
                        break;
                }

                return false;
            }

            public override void Dispose() => _enumerator?.Dispose();
        }
    }

    public static class FastEnumerable
    {
        public static R Aggregate<T, R>(this IFastEnumerable<T> source, R seed, Func<R, T, R> aggregate)
        {
            var res = seed;

            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    var item = e.TryGetNext(out var success);

                    if (!success)
                    {
                        break;
                    }

                    res = aggregate(res, item);
                }
            }

            return res;
        }

        public static IFastEnumerable<T> Empty<T>() => EmptyIterator<T>.Instance;

        public static void ForEach<T>(this IFastEnumerable<T> source, Action<T> next)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    var item = e.TryGetNext(out var success);

                    if (!success)
                    {
                        break;
                    }

                    next(item);
                }
            }
        }

        public static IFastEnumerable<int> Range(int start, int count) => new RangeIterator(start, count);

        public static IFastEnumerable<R> Select<T, R>(this IFastEnumerable<T> source, Func<T, R> selector) => new SelectFastIterator<T, R>(source, selector);

        public static IFastEnumerable<T> ToFastEnumerable<T>(this IEnumerable<T> source) => new EnumerableToFastEnumerable<T>(source);

        public static IEnumerable<T> ToEnumerable<T>(this IFastEnumerable<T> source) => new FastEnumerableToEnumerable<T>(source);

        public static IFastEnumerable<T> Where<T>(this IFastEnumerable<T> source, Func<T, bool> predicate) => new WhereFastIterator<T>(source, predicate);

        private sealed class EmptyIterator<T> : IFastEnumerable<T>, IFastEnumerator<T>
        {
            public static readonly EmptyIterator<T> Instance = new EmptyIterator<T>();

            public void Dispose() { }

            public IFastEnumerator<T> GetEnumerator() => this;

            public T TryGetNext(out bool success)
            {
                success = false;
                return default(T);
            }
        }

        private sealed class EnumerableToFastEnumerable<T> : FastIterator<T>
        {
            private readonly IEnumerable<T> _source;
            private IEnumerator<T> _enumerator;

            public EnumerableToFastEnumerable(IEnumerable<T> source)
            {
                _source = source;
            }

            public override FastIterator<T> Clone() => new EnumerableToFastEnumerable<T>(_source);

            public override T TryGetNext(out bool success)
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        success = _enumerator.MoveNext();
                        if (success)
                        {
                            return _enumerator.Current;
                        }
                        break;
                }

                success = false;
                return default(T);
            }

            public override void Dispose() => _enumerator?.Dispose();
        }

        private sealed class FastEnumerableToEnumerable<T> : Iterator<T>
        {
            private readonly IFastEnumerable<T> _source;
            private IFastEnumerator<T> _enumerator;

            public FastEnumerableToEnumerable(IFastEnumerable<T> source)
            {
                _source = source;
            }

            public override Iterator<T> Clone() => new FastEnumerableToEnumerable<T>(_source);

            public override bool MoveNext()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        _current = _enumerator.TryGetNext(out var success);
                        return success;
                }

                return false;
            }

            public override void Dispose() => _enumerator?.Dispose();
        }

        private sealed class RangeIterator : FastIterator<int>
        {
            private readonly int _start;
            private readonly int _end;
            private int _next;

            public RangeIterator(int start, int count)
            {
                _start = start;
                _end = start + count;
            }

            public override FastIterator<int> Clone() => new RangeIterator(_start, _end - _start);

            public override int TryGetNext(out bool success)
            {
                switch (_state)
                {
                    case 1:
                        _next = _start;
                        _state = 2;
                        goto case 2;

                    case 2:
                        if (_next < _end)
                        {
                            success = true;
                            return _next++;
                        }
                        break;
                }

                success = false;
                return default(int);
            }
        }

        private sealed class SelectFastIterator<T, R> : FastIterator<R>
        {
            private readonly IFastEnumerable<T> _source;
            private readonly Func<T, R> _selector;
            private IFastEnumerator<T> _enumerator;

            public SelectFastIterator(IFastEnumerable<T> source, Func<T, R> selector)
            {
                _source = source;
                _selector = selector;
            }

            public override FastIterator<R> Clone() => new SelectFastIterator<T, R>(_source, _selector);

            public override R TryGetNext(out bool success)
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        var item = _enumerator.TryGetNext(out success);
                        if (success)
                        {
                            return _selector(item);
                        }
                        break;
                }

                success = false;
                return default(R);
            }

            public override void Dispose() => _enumerator?.Dispose();
        }

        private sealed class WhereFastIterator<T> : FastIterator<T>
        {
            private readonly IFastEnumerable<T> _source;
            private readonly Func<T, bool> _predicate;
            private IFastEnumerator<T> _enumerator;

            public WhereFastIterator(IFastEnumerable<T> source, Func<T, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            public override FastIterator<T> Clone() => new WhereFastIterator<T>(_source, _predicate);

            public override T TryGetNext(out bool success)
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        while (true)
                        {
                            var item = _enumerator.TryGetNext(out success);
                            if (!success)
                            {
                                break;
                            }

                            if (_predicate(item))
                            {
                                return item;
                            }
                        }
                        break;
                }

                success = false;
                return default(T);
            }

            public override void Dispose() => _enumerator?.Dispose();
        }
    }

    public static class AsyncEnumerable
    {
        private static readonly Task<bool> True = Task.FromResult(true);
        private static readonly Task<bool> False = Task.FromResult(false);

        public static async Task<R> Aggregate<T, R>(this IAsyncEnumerable<T> source, R seed, Func<R, T, R> aggregate)
        {
            var res = seed;

            var e = source.GetAsyncEnumerator();

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    res = aggregate(res, e.Current);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return res;
        }

        public static async Task<R> Aggregate<T, R>(this IAsyncEnumerable<T> source, R seed, Func<R, T, Task<R>> aggregate)
        {
            var res = seed;

            var e = source.GetAsyncEnumerator();

            try
            {
                while (true)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        res = await aggregate(res, e.Current).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return res;
        }

        public static IAsyncEnumerable<T> Empty<T>() => EmptyIterator<T>.Instance;

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> source, Func<T, Task> next)
        {
            var e = source.GetAsyncEnumerator();

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var item = e.Current;
                    await next(item).ConfigureAwait(false);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static IAsyncEnumerable<int> Range(int start, int count) => new RangeIterator(start, count);

        public static IAsyncEnumerable<R> Select<T, R>(this IAsyncEnumerable<T> source, Func<T, R> selector) => new SelectIterator<T, R>(source, selector);

        public static IAsyncEnumerable<R> Select<T, R>(this IAsyncEnumerable<T> source, Func<T, Task<R>> selector) => new SelectIteratorWithTask<T, R>(source, selector);

        public static IAsyncEnumerable<T> Where<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate) => new WhereIterator<T>(source, predicate);

        public static IAsyncEnumerable<T> Where<T>(this IAsyncEnumerable<T> source, Func<T, Task<bool>> predicate) => new WhereIteratorWithTask<T>(source, predicate);

        private sealed class EmptyIterator<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>
        {
            public static readonly EmptyIterator<T> Instance = new EmptyIterator<T>();

            public T Current => default(T);

            public Task DisposeAsync() => Task.CompletedTask;

            public IAsyncEnumerator<T> GetAsyncEnumerator() => this;

            public Task<bool> MoveNextAsync() => False;
        }

        private sealed class RangeIterator : AsyncIterator<int>
        {
            private readonly int _start;
            private readonly int _end;
            private int _next;

            public RangeIterator(int start, int count)
            {
                _start = start;
                _end = start + count;
            }

            public override AsyncIterator<int> Clone() => new RangeIterator(_start, _end - _start);

            public override Task<bool> MoveNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _next = _start;
                        _state = 2;
                        goto case 2;

                    case 2:
                        if (_next < _end)
                        {
                            _current = _next++;
                            return True;
                        }
                        break;
                }

                return False;
            }
        }

        private sealed class SelectIterator<T, R> : AsyncIterator<R>
        {
            private readonly IAsyncEnumerable<T> _source;
            private readonly Func<T, R> _selector;
            private IAsyncEnumerator<T> _enumerator;

            public SelectIterator(IAsyncEnumerable<T> source, Func<T, R> selector)
            {
                _source = source;
                _selector = selector;
            }

            public override AsyncIterator<R> Clone() => new SelectIterator<T, R>(_source, _selector);

            public override async Task<bool> MoveNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _selector(_enumerator.Current);
                            return true;
                        }
                        break;
                }

                return false;
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;
        }

        private sealed class SelectIteratorWithTask<T, R> : AsyncIterator<R>
        {
            private readonly IAsyncEnumerable<T> _source;
            private readonly Func<T, Task<R>> _selector;
            private IAsyncEnumerator<T> _enumerator;

            public SelectIteratorWithTask(IAsyncEnumerable<T> source, Func<T, Task<R>> selector)
            {
                _source = source;
                _selector = selector;
            }

            public override AsyncIterator<R> Clone() => new SelectIteratorWithTask<T, R>(_source, _selector);

            public override async Task<bool> MoveNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = await _selector(_enumerator.Current).ConfigureAwait(false);
                            return true;
                        }
                        break;
                }

                return false;
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;
        }

        private sealed class WhereIterator<T> : AsyncIterator<T>
        {
            private readonly IAsyncEnumerable<T> _source;
            private readonly Func<T, bool> _predicate;
            private IAsyncEnumerator<T> _enumerator;

            public WhereIterator(IAsyncEnumerable<T> source, Func<T, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            public override AsyncIterator<T> Clone() => new WhereIterator<T>(_source, _predicate);

            public override async Task<bool> MoveNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            if (_predicate(item))
                            {
                                _current = item;
                                return true;
                            }
                        }
                        break;
                }

                return false;
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;
        }

        private sealed class WhereIteratorWithTask<T> : AsyncIterator<T>
        {
            private readonly IAsyncEnumerable<T> _source;
            private readonly Func<T, Task<bool>> _predicate;
            private IAsyncEnumerator<T> _enumerator;

            public WhereIteratorWithTask(IAsyncEnumerable<T> source, Func<T, Task<bool>> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            public override AsyncIterator<T> Clone() => new WhereIteratorWithTask<T>(_source, _predicate);

            public override async Task<bool> MoveNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            if (await _predicate(item).ConfigureAwait(false))
                            {
                                _current = item;
                                return true;
                            }
                        }
                        break;
                }

                return false;
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;
        }
    }

    public static class AsyncFastEnumerable
    {
        private static readonly Task<bool> True = Task.FromResult(true);
        private static readonly Task<bool> False = Task.FromResult(false);

        public static async Task<R> Aggregate<T, R>(this IAsyncFastEnumerable<T> source, R seed, Func<R, T, R> aggregate)
        {
            var res = seed;

            var e = source.GetAsyncEnumerator();

            try
            {
                while (await e.WaitForNextAsync().ConfigureAwait(false))
                {
                    while (true)
                    {
                        var item = e.TryGetNext(out var success);

                        if (!success)
                        {
                            break;
                        }

                        res = aggregate(res, item);
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return res;
        }

        public static async Task<R> Aggregate<T, R>(this IAsyncFastEnumerable<T> source, R seed, Func<R, T, Task<R>> aggregate)
        {
            var res = seed;

            var e = source.GetAsyncEnumerator();

            try
            {
                while (await e.WaitForNextAsync().ConfigureAwait(false))
                {
                    while (true)
                    {
                        var item = e.TryGetNext(out var success);

                        if (!success)
                        {
                            break;
                        }

                        res = await aggregate(res, item).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return res;
        }

        public static IAsyncFastEnumerable<T> Empty<T>() => EmptyIterator<T>.Instance;

        public static async Task ForEachAsync<T>(this IAsyncFastEnumerable<T> source, Func<T, Task> next)
        {
            var e = source.GetAsyncEnumerator();

            try
            {
                while (await e.WaitForNextAsync().ConfigureAwait(false))
                {
                    while (true)
                    {
                        var item = e.TryGetNext(out var success);

                        if (!success)
                        {
                            break;
                        }

                        await next(item).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static IAsyncFastEnumerable<int> Range(int start, int count) => new RangeIterator(start, count);

        public static IAsyncFastEnumerable<R> Select<T, R>(this IAsyncFastEnumerable<T> source, Func<T, R> selector) => new SelectFastIterator<T, R>(source, selector);

        public static IAsyncFastEnumerable<R> Select<T, R>(this IAsyncFastEnumerable<T> source, Func<T, Task<R>> selector) => new SelectFastIteratorWithTask<T, R>(source, selector);

        public static IAsyncFastEnumerable<T> ToAsyncFastEnumerable<T>(this IAsyncEnumerable<T> source) => new AsyncEnumerableToAsyncFastEnumerable<T>(source);

        public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IAsyncFastEnumerable<T> source) => new AsyncFastEnumerableToAsyncEnumerable<T>(source);

        public static IAsyncFastEnumerable<T> Where<T>(this IAsyncFastEnumerable<T> source, Func<T, bool> predicate) => new WhereFastIterator<T>(source, predicate);

        public static IAsyncFastEnumerable<T> Where<T>(this IAsyncFastEnumerable<T> source, Func<T, Task<bool>> predicate) => new WhereFastIteratorWithTask<T>(source, predicate);

        private sealed class EmptyIterator<T> : IAsyncFastEnumerable<T>, IAsyncFastEnumerator<T>
        {
            public static readonly EmptyIterator<T> Instance = new EmptyIterator<T>();

            public Task DisposeAsync() => Task.CompletedTask;

            public IAsyncFastEnumerator<T> GetAsyncEnumerator() => this;

            public T TryGetNext(out bool success)
            {
                success = false;
                return default(T);
            }

            public Task<bool> WaitForNextAsync() => False;
        }

        private sealed class AsyncEnumerableToAsyncFastEnumerable<T> : AsyncFastIterator<T>
        {
            private readonly IAsyncEnumerable<T> _source;
            private IAsyncEnumerator<T> _enumerator;
            private bool _hasNext;

            public AsyncEnumerableToAsyncFastEnumerable(IAsyncEnumerable<T> source)
            {
                _source = source;
            }

            public override AsyncFastIterator<T> Clone() => new AsyncEnumerableToAsyncFastEnumerable<T>(_source);

            public override T TryGetNext(out bool success)
            {
                success = _hasNext;
                _hasNext = false;

                return success ? _enumerator.Current : default(T);
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;

            public override async Task<bool> WaitForNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        _hasNext = await _enumerator.MoveNextAsync().ConfigureAwait(false);
                        return _hasNext;
                }

                return false;
            }
        }

        private sealed class AsyncFastEnumerableToAsyncEnumerable<T> : AsyncIterator<T>
        {
            private readonly IAsyncFastEnumerable<T> _source;
            private IAsyncFastEnumerator<T> _enumerator;

            public AsyncFastEnumerableToAsyncEnumerable(IAsyncFastEnumerable<T> source)
            {
                _source = source;
            }

            public override AsyncIterator<T> Clone() => new AsyncFastEnumerableToAsyncEnumerable<T>(_source);

            public override async Task<bool> MoveNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        do
                        {
                            while (true)
                            {
                                var item = _enumerator.TryGetNext(out var success);

                                if (!success)
                                {
                                    break;
                                }
                                else
                                {
                                    _current = item;
                                    return true;
                                }
                            }
                        } while (await _enumerator.WaitForNextAsync().ConfigureAwait(false));

                        break;
                }

                return false;
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;
        }

        private sealed class RangeIterator : AsyncFastIterator<int>
        {
            private readonly int _start;
            private readonly int _end;
            private int _next;

            public RangeIterator(int start, int count)
            {
                _start = start;
                _end = start + count;
            }

            public override AsyncFastIterator<int> Clone() => new RangeIterator(_start, _end - _start);

            public override int TryGetNext(out bool success)
            {
                if (_state == 2 && _next < _end)
                {
                    success = true;
                    return _next++;
                }

                success = false;
                return default(int);
            }

            public override Task<bool> WaitForNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _next = _start;
                        _state = 2;
                        goto case 2;

                    case 2:
                        if (_next < _end)
                        {
                            return True;
                        }
                        break;
                }

                return False;
            }
        }

        private sealed class SelectFastIterator<T, R> : AsyncFastIterator<R>
        {
            private readonly IAsyncFastEnumerable<T> _source;
            private readonly Func<T, R> _selector;
            private IAsyncFastEnumerator<T> _enumerator;

            public SelectFastIterator(IAsyncFastEnumerable<T> source, Func<T, R> selector)
            {
                _source = source;
                _selector = selector;
            }

            public override AsyncFastIterator<R> Clone() => new SelectFastIterator<T, R>(_source, _selector);

            public override R TryGetNext(out bool success)
            {
                if (_enumerator != null)
                {
                    var item = _enumerator.TryGetNext(out success);
                    if (success)
                    {
                        return _selector(item);
                    }
                }

                success = false;
                return default(R);
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;

            public override Task<bool> WaitForNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        return _enumerator.WaitForNextAsync();
                }

                return False;
            }
        }

        private sealed class SelectFastIteratorWithTask<T, R> : AsyncFastIterator<R>
        {
            private readonly IAsyncFastEnumerable<T> _source;
            private readonly Func<T, Task<R>> _selector;
            private IAsyncFastEnumerator<T> _enumerator;
            private bool _hasNext;
            private R _next;

            public SelectFastIteratorWithTask(IAsyncFastEnumerable<T> source, Func<T, Task<R>> selector)
            {
                _source = source;
                _selector = selector;
            }

            public override AsyncFastIterator<R> Clone() => new SelectFastIteratorWithTask<T, R>(_source, _selector);

            public override R TryGetNext(out bool success)
            {
                success = _hasNext;
                _hasNext = false;

                return success ? _next : default(R);
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;

            public override async Task<bool> WaitForNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        do
                        {
                            while (true)
                            {
                                var item = _enumerator.TryGetNext(out var success);

                                if (!success)
                                {
                                    break;
                                }
                                else
                                {
                                    _hasNext = true;
                                    _next = await _selector(item).ConfigureAwait(false);
                                    return true;
                                }
                            }
                        }
                        while (await _enumerator.WaitForNextAsync().ConfigureAwait(false));

                        break;
                }

                _hasNext = false;
                _next = default(R);

                return false;
            }
        }

        private sealed class WhereFastIterator<T> : AsyncFastIterator<T>
        {
            private readonly IAsyncFastEnumerable<T> _source;
            private readonly Func<T, bool> _predicate;
            private IAsyncFastEnumerator<T> _enumerator;

            public WhereFastIterator(IAsyncFastEnumerable<T> source, Func<T, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            public override AsyncFastIterator<T> Clone() => new WhereFastIterator<T>(_source, _predicate);

            public override T TryGetNext(out bool success)
            {
                if (_enumerator != null)
                {
                    while (true)
                    {
                        var item = _enumerator.TryGetNext(out success);
                        if (!success)
                        {
                            break;
                        }

                        if (_predicate(item))
                        {
                            return item;
                        }
                    }
                }

                success = false;
                return default(T);
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;

            public override Task<bool> WaitForNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        return _enumerator.WaitForNextAsync();
                }

                return False;
            }
        }

        private sealed class WhereFastIteratorWithTask<T> : AsyncFastIterator<T>
        {
            private readonly IAsyncFastEnumerable<T> _source;
            private readonly Func<T, Task<bool>> _predicate;
            private IAsyncFastEnumerator<T> _enumerator;
            private bool _hasNext;
            private T _next;

            public WhereFastIteratorWithTask(IAsyncFastEnumerable<T> source, Func<T, Task<bool>> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            public override AsyncFastIterator<T> Clone() => new WhereFastIteratorWithTask<T>(_source, _predicate);

            public override T TryGetNext(out bool success)
            {
                success = _hasNext;
                _hasNext = false;

                return success ? _next : default(T);
            }

            public override Task DisposeAsync() => _enumerator?.DisposeAsync() ?? Task.CompletedTask;

            public override async Task<bool> WaitForNextAsync()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetAsyncEnumerator();
                        _state = 2;
                        goto case 2;

                    case 2:
                        do
                        {
                            while (true)
                            {
                                var item = _enumerator.TryGetNext(out var success);

                                if (!success)
                                {
                                    break;
                                }
                                else
                                {
                                    if (await _predicate(item).ConfigureAwait(false))
                                    {
                                        _hasNext = true;
                                        _next = item;
                                        return true;
                                    }
                                }
                            }
                        }
                        while (await _enumerator.WaitForNextAsync().ConfigureAwait(false));

                        break;
                }

                _hasNext = false;
                _next = default(T);

                return false;
            }
        }
    }
}
