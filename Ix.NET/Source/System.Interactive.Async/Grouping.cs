// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateEnumerable(() =>
                          {
                              var gate = new object();

                              var e = source.GetEnumerator();
                              var count = 1;

                              var map = new Dictionary<TKey, AsyncGrouping<TKey, TElement>>(comparer);
                              var list = new List<IAsyncGrouping<TKey, TElement>>();

                              var index = 0;

                              var current = default(IAsyncGrouping<TKey, TElement>);
                              var faulted = default(ExceptionDispatchInfo);

                              var res = default(bool?);

                              var cts = new CancellationTokenDisposable();
                              var refCount = new Disposable(
                                  () =>
                                  {
                                      if (Interlocked.Decrement(ref count) == 0)
                                          e.Dispose();
                                  }
                              );
                              var d = Disposable.Create(cts, refCount);

                              var iterateSource = default(Func<CancellationToken, Task<bool>>);
                              iterateSource = async ct =>
                                              {
                                                  lock (gate)
                                                  {
                                                      if (res != null)
                                                      {
                                                          return res.Value;
                                                      }
                                                      res = null;
                                                  }

                                                  faulted?.Throw();

                                                  try
                                                  {
                                                      res = await e.MoveNext(ct)
                                                                   .ConfigureAwait(false);
                                                      if (res == true)
                                                      {
                                                          var key = default(TKey);
                                                          var element = default(TElement);

                                                          var cur = e.Current;
                                                          try
                                                          {
                                                              key = keySelector(cur);
                                                              element = elementSelector(cur);
                                                          }
                                                          catch (Exception exception)
                                                          {
                                                              foreach (var v in map.Values)
                                                                  v.Error(exception);

                                                              throw;
                                                          }

                                                          var group = default(AsyncGrouping<TKey, TElement>);
                                                          if (!map.TryGetValue(key, out group))
                                                          {
                                                              group = new AsyncGrouping<TKey, TElement>(key, iterateSource, refCount);
                                                              map.Add(key, group);
                                                              lock (list)
                                                                  list.Add(group);

                                                              Interlocked.Increment(ref count);
                                                          }
                                                          group.Add(element);
                                                      }

                                                      return res.Value;
                                                  }
                                                  catch (Exception ex)
                                                  {
                                                      foreach (var v in map.Values)
                                                          v.Error(ex);

                                                      faulted = ExceptionDispatchInfo.Capture(ex);
                                                      throw;
                                                  }
                                                  finally
                                                  {
                                                      res = null;
                                                  }
                                              };

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      var result = await iterateSource(ct)
                                                       .ConfigureAwait(false);

                                      current = null;
                                      lock (list)
                                      {
                                          if (index < list.Count)
                                              current = list[index++];
                                      }

                                      if (current != null)
                                      {
                                          return true;
                                      }
                                      return result && await f(ct)
                                                 .ConfigureAwait(false);
                                  };

                              return CreateEnumerator(
                                  f,
                                  () => current,
                                  d.Dispose,
                                  e
                              );
                          });
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer, CancellationToken.None);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedAsyncEnumerable<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, CancellationToken.None);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.GroupBy(keySelector, elementSelector, comparer)
                         .Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default)
                         .Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.GroupBy(keySelector, x => x, comparer)
                         .Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.GroupBy(keySelector, x => x, EqualityComparer<TKey>.Default)
                         .Select(g => resultSelector(g.Key, g));
        }

        private static IEnumerable<IGrouping<TKey, TElement>> GroupUntil<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IComparer<TKey> comparer)
        {
            var group = default(EnumerableGrouping<TKey, TElement>);
            foreach (var x in source)
            {
                var key = keySelector(x);
                if (group == null || comparer.Compare(group.Key, key) != 0)
                {
                    group = new EnumerableGrouping<TKey, TElement>(key);
                    yield return group;
                }
                group.Add(elementSelector(x));
            }
        }

        internal sealed class GroupedAsyncEnumerable<TSource, TKey> : IIListProvider<IAsyncGrouping<TKey, TSource>>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, TKey> keySelector;
            private readonly IEqualityComparer<TKey> comparer;
            private readonly CancellationToken cancellationToken;

            public GroupedAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
            {
                if (source == null) throw new ArgumentNullException(nameof(source));
                if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
                this.cancellationToken = cancellationToken;
            }


            public IAsyncEnumerator<IAsyncGrouping<TKey, TSource>> GetEnumerator()
            {
                Internal.Lookup<TKey, TSource> lookup = null;
                IAsyncGrouping<TKey, TSource> current = null;
                IEnumerator<IGrouping<TKey, TSource>> enumerator = null;

                return CreateEnumerator(
                    async ct =>
                    {
                        if (lookup == null)
                        {
                            lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, ct).ConfigureAwait(false);
                            enumerator = lookup.GetEnumerator();
                        }

                        // By the time we get here, the lookup is sync
                        if (ct.IsCancellationRequested)
                            return false;

                        return enumerator?.MoveNext() ?? false;
                    },
                    () => (IAsyncGrouping<TKey, TSource>)enumerator?.Current,
                    () =>
                        {
                            if (enumerator != null)
                            {
                                enumerator.Dispose();
                                enumerator = null;
                            }
                        });
            }

            public async Task<IAsyncGrouping<TKey, TSource>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TSource>> lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await lookup.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TSource>>> ToListAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TSource>> lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await lookup.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);

                return lookup.Count;
            }
        }

        private class AsyncGrouping<TKey, TElement> : IAsyncGrouping<TKey, TElement>
        {
            private readonly List<TElement> elements = new List<TElement>();
            private readonly Func<CancellationToken, Task<bool>> iterateSource;
            private readonly IDisposable sourceDisposable;
            private bool done;
            private ExceptionDispatchInfo exception;

            public AsyncGrouping(TKey key, Func<CancellationToken, Task<bool>> iterateSource, IDisposable sourceDisposable)
            {
                this.iterateSource = iterateSource;
                this.sourceDisposable = sourceDisposable;
                Key = key;
            }

            public TKey Key { get; }

            public IAsyncEnumerator<TElement> GetEnumerator()
            {
                var index = -1;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, sourceDisposable);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                    {
                        var size = 0;
                        lock (elements)
                            size = elements.Count;

                        if (index < size)
                        {
                            return true;
                        }
                        if (done)
                        {
                            exception?.Throw();
                            return false;
                        }
                        if (await iterateSource(ct)
                                .ConfigureAwait(false))
                        {
                            return await f(ct)
                                       .ConfigureAwait(false);
                        }
                        return false;
                    };

                return CreateEnumerator(
                    ct =>
                    {
                        ++index;
                        return f(cts.Token);
                    },
                    () => elements[index],
                    d.Dispose,
                    null
                );
            }

            public void Add(TElement element)
            {
                lock (elements)
                    elements.Add(element);
            }

            public void Error(Exception exception)
            {
                done = true;
                this.exception = ExceptionDispatchInfo.Capture(exception);
            }
        }
    }
}

// Note: The type here has to be internal as System.Linq has it's own public copy we're not using

namespace System.Linq.Internal
{
    /// Adapted from System.Linq.Grouping from .NET Framework
    /// Source: https://github.com/dotnet/corefx/blob/b90532bc97b07234a7d18073819d019645285f1c/src/System.Linq/src/System/Linq/Grouping.cs#L64
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>, IAsyncGrouping<TKey, TElement>
    {
        internal int _count;
        internal TElement[] _elements;
        internal int _hashCode;
        internal Grouping<TKey, TElement> _hashNext;
        internal TKey _key;
        internal Grouping<TKey, TElement> _next;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                yield return _elements[i];
            }
        }

        // DDB195907: implement IGrouping<>.Key implicitly
        // so that WPF binding works on this property.
        public TKey Key
        {
            get { return _key; }
        }

        int ICollection<TElement>.Count
        {
            get { return _count; }
        }

        bool ICollection<TElement>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection<TElement>.Add(TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        void ICollection<TElement>.Clear()
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        bool ICollection<TElement>.Contains(TElement item)
        {
            return Array.IndexOf(_elements, item, 0, _count) >= 0;
        }

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
        {
            Array.Copy(_elements, 0, array, arrayIndex, _count);
        }

        bool ICollection<TElement>.Remove(TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        int IList<TElement>.IndexOf(TElement item)
        {
            return Array.IndexOf(_elements, item, 0, _count);
        }

        void IList<TElement>.Insert(int index, TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        void IList<TElement>.RemoveAt(int index)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        TElement IList<TElement>.this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return _elements[index];
            }

            set { throw new NotSupportedException(Strings.NOT_SUPPORTED); }
        }

        internal void Add(TElement element)
        {
            if (_elements.Length == _count)
            {
                Array.Resize(ref _elements, checked(_count*2));
            }

            _elements[_count] = element;
            _count++;
        }

        internal void Trim()
        {
            if (_elements.Length != _count)
            {
                Array.Resize(ref _elements, _count);
            }
        }

        IAsyncEnumerator<TElement> IAsyncEnumerable<TElement>.GetEnumerator()
        {
            var adapter = new AsyncEnumerable.AsyncEnumerableAdapter<TElement>(this);
            return adapter.GetEnumerator();
        }
    }
}