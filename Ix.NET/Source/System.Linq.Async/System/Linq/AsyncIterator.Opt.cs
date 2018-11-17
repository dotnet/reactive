// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    internal partial class AsyncIteratorBase<TSource>
    {
        public virtual IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
        {
            return new AsyncEnumerable.SelectEnumerableAsyncIterator<TSource, TResult>(this, selector);
        }

        public virtual IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, ValueTask<TResult>> selector)
        {
            return new AsyncEnumerable.SelectEnumerableAsyncIteratorWithTask<TSource, TResult>(this, selector);
        }

#if !NO_DEEP_CANCELLATION
        public virtual IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, CancellationToken, ValueTask<TResult>> selector)
        {
            return new AsyncEnumerable.SelectEnumerableAsyncIteratorWithTaskAndCancellation<TSource, TResult>(this, selector);
        }
#endif

        public virtual IAsyncEnumerable<TSource> Where(Func<TSource, bool> predicate)
        {
            return new AsyncEnumerable.WhereEnumerableAsyncIterator<TSource>(this, predicate);
        }

        public virtual IAsyncEnumerable<TSource> Where(Func<TSource, ValueTask<bool>> predicate)
        {
            return new AsyncEnumerable.WhereEnumerableAsyncIteratorWithTask<TSource>(this, predicate);
        }

#if !NO_DEEP_CANCELLATION
        public virtual IAsyncEnumerable<TSource> Where(Func<TSource, CancellationToken, ValueTask<bool>> predicate)
        {
            return new AsyncEnumerable.WhereEnumerableAsyncIteratorWithTaskAndCancellation<TSource>(this, predicate);
        }
#endif
    }
}
