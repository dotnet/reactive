// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        private sealed class CombinedPredicates2<TSource> : ICombinedPredicates<TSource>
        {
            private readonly Func<TSource, bool> _predicate1;
            private readonly Func<TSource, bool> _predicate2;

            public CombinedPredicates2(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
            }

            public ICombinedPredicates<TSource> And(Func<TSource, bool> predicate) =>
                new CombinedPredicates3<TSource>(
                    _predicate1,
                    _predicate2,
                    predicate
                );

            public bool Invoke(TSource x) => _predicate1(x) && _predicate2(x);
        }

        private sealed class CombinedPredicates3<TSource> : ICombinedPredicates<TSource>
        {
            private readonly Func<TSource, bool> _predicate1;
            private readonly Func<TSource, bool> _predicate2;
            private readonly Func<TSource, bool> _predicate3;

            public CombinedPredicates3(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2, Func<TSource, bool> predicate3)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
                _predicate3 = predicate3;
            }

            public ICombinedPredicates<TSource> And(Func<TSource, bool> predicate) =>
                new CombinedPredicates4<TSource>(
                    _predicate1,
                    _predicate2,
                    _predicate3,
                    predicate
                );

            public bool Invoke(TSource x) => _predicate1(x) && _predicate2(x) && _predicate3(x);
        }

        private sealed class CombinedPredicates4<TSource> : ICombinedPredicates<TSource>
        {
            private readonly Func<TSource, bool> _predicate1;
            private readonly Func<TSource, bool> _predicate2;
            private readonly Func<TSource, bool> _predicate3;
            private readonly Func<TSource, bool> _predicate4;

            public CombinedPredicates4(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2, Func<TSource, bool> predicate3, Func<TSource, bool> predicate4)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
                _predicate3 = predicate3;
                _predicate4 = predicate4;
            }

            public ICombinedPredicates<TSource> And(Func<TSource, bool> predicate) =>
                new CombinedPredicatesN<TSource>(
                    _predicate1,
                    _predicate2,
                    _predicate3,
                    _predicate4,
                    predicate
                );

            public bool Invoke(TSource x) => _predicate1(x) && _predicate2(x) && _predicate3(x) && _predicate4(x);
        }

        private sealed class CombinedAsyncPredicates2<TSource> : ICombinedAsyncPredicates<TSource>
        {
            private readonly Func<TSource, ValueTask<bool>> _predicate1;
            private readonly Func<TSource, ValueTask<bool>> _predicate2;

            public CombinedAsyncPredicates2(Func<TSource, ValueTask<bool>> predicate1, Func<TSource, ValueTask<bool>> predicate2)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
            }

            public ICombinedAsyncPredicates<TSource> And(Func<TSource, ValueTask<bool>> predicate) =>
                new CombinedAsyncPredicates3<TSource>(
                    _predicate1,
                    _predicate2,
                    predicate
                );

            public async ValueTask<bool> Invoke(TSource x) => await _predicate1(x).ConfigureAwait(false) && await _predicate2(x).ConfigureAwait(false);
        }

        private sealed class CombinedAsyncPredicates3<TSource> : ICombinedAsyncPredicates<TSource>
        {
            private readonly Func<TSource, ValueTask<bool>> _predicate1;
            private readonly Func<TSource, ValueTask<bool>> _predicate2;
            private readonly Func<TSource, ValueTask<bool>> _predicate3;

            public CombinedAsyncPredicates3(Func<TSource, ValueTask<bool>> predicate1, Func<TSource, ValueTask<bool>> predicate2, Func<TSource, ValueTask<bool>> predicate3)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
                _predicate3 = predicate3;
            }

            public ICombinedAsyncPredicates<TSource> And(Func<TSource, ValueTask<bool>> predicate) =>
                new CombinedAsyncPredicates4<TSource>(
                    _predicate1,
                    _predicate2,
                    _predicate3,
                    predicate
                );

            public async ValueTask<bool> Invoke(TSource x) => await _predicate1(x).ConfigureAwait(false) && await _predicate2(x).ConfigureAwait(false) && await _predicate3(x).ConfigureAwait(false);
        }

        private sealed class CombinedAsyncPredicates4<TSource> : ICombinedAsyncPredicates<TSource>
        {
            private readonly Func<TSource, ValueTask<bool>> _predicate1;
            private readonly Func<TSource, ValueTask<bool>> _predicate2;
            private readonly Func<TSource, ValueTask<bool>> _predicate3;
            private readonly Func<TSource, ValueTask<bool>> _predicate4;

            public CombinedAsyncPredicates4(Func<TSource, ValueTask<bool>> predicate1, Func<TSource, ValueTask<bool>> predicate2, Func<TSource, ValueTask<bool>> predicate3, Func<TSource, ValueTask<bool>> predicate4)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
                _predicate3 = predicate3;
                _predicate4 = predicate4;
            }

            public ICombinedAsyncPredicates<TSource> And(Func<TSource, ValueTask<bool>> predicate) =>
                new CombinedAsyncPredicatesN<TSource>(
                    _predicate1,
                    _predicate2,
                    _predicate3,
                    _predicate4,
                    predicate
                );

            public async ValueTask<bool> Invoke(TSource x) => await _predicate1(x).ConfigureAwait(false) && await _predicate2(x).ConfigureAwait(false) && await _predicate3(x).ConfigureAwait(false) && await _predicate4(x).ConfigureAwait(false);
        }

#if !NO_DEEP_CANCELLATION
        private sealed class CombinedAsyncPredicatesWithCancellation2<TSource> : ICombinedAsyncPredicatesWithCancellation<TSource>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate1;
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate2;

            public CombinedAsyncPredicatesWithCancellation2(Func<TSource, CancellationToken, ValueTask<bool>> predicate1, Func<TSource, CancellationToken, ValueTask<bool>> predicate2)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
            }

            public ICombinedAsyncPredicatesWithCancellation<TSource> And(Func<TSource, CancellationToken, ValueTask<bool>> predicate) =>
                new CombinedAsyncPredicatesWithCancellation3<TSource>(
                    _predicate1,
                    _predicate2,
                    predicate
                );

            public async ValueTask<bool> Invoke(TSource x, CancellationToken ct) => await _predicate1(x, ct).ConfigureAwait(false) && await _predicate2(x, ct).ConfigureAwait(false);
        }

        private sealed class CombinedAsyncPredicatesWithCancellation3<TSource> : ICombinedAsyncPredicatesWithCancellation<TSource>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate1;
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate2;
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate3;

            public CombinedAsyncPredicatesWithCancellation3(Func<TSource, CancellationToken, ValueTask<bool>> predicate1, Func<TSource, CancellationToken, ValueTask<bool>> predicate2, Func<TSource, CancellationToken, ValueTask<bool>> predicate3)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
                _predicate3 = predicate3;
            }

            public ICombinedAsyncPredicatesWithCancellation<TSource> And(Func<TSource, CancellationToken, ValueTask<bool>> predicate) =>
                new CombinedAsyncPredicatesWithCancellation4<TSource>(
                    _predicate1,
                    _predicate2,
                    _predicate3,
                    predicate
                );

            public async ValueTask<bool> Invoke(TSource x, CancellationToken ct) => await _predicate1(x, ct).ConfigureAwait(false) && await _predicate2(x, ct).ConfigureAwait(false) && await _predicate3(x, ct).ConfigureAwait(false);
        }

        private sealed class CombinedAsyncPredicatesWithCancellation4<TSource> : ICombinedAsyncPredicatesWithCancellation<TSource>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate1;
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate2;
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate3;
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate4;

            public CombinedAsyncPredicatesWithCancellation4(Func<TSource, CancellationToken, ValueTask<bool>> predicate1, Func<TSource, CancellationToken, ValueTask<bool>> predicate2, Func<TSource, CancellationToken, ValueTask<bool>> predicate3, Func<TSource, CancellationToken, ValueTask<bool>> predicate4)
            {
                _predicate1 = predicate1;
                _predicate2 = predicate2;
                _predicate3 = predicate3;
                _predicate4 = predicate4;
            }

            public ICombinedAsyncPredicatesWithCancellation<TSource> And(Func<TSource, CancellationToken, ValueTask<bool>> predicate) =>
                new CombinedAsyncPredicatesWithCancellationN<TSource>(
                    _predicate1,
                    _predicate2,
                    _predicate3,
                    _predicate4,
                    predicate
                );

            public async ValueTask<bool> Invoke(TSource x, CancellationToken ct) => await _predicate1(x, ct).ConfigureAwait(false) && await _predicate2(x, ct).ConfigureAwait(false) && await _predicate3(x, ct).ConfigureAwait(false) && await _predicate4(x, ct).ConfigureAwait(false);
        }

#endif
    }
}
