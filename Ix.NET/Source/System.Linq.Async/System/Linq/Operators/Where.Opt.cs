// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        private static Func<TSource, bool> CombinePredicates<TSource>(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2)
        {
            if (predicate1.Target is ICombinedPredicates<TSource> c)
            {
                return c.And(predicate2).Invoke;
            }
            else
            {
                return new CombinedPredicates2<TSource>(predicate1, predicate2).Invoke;
            }
        }

        private interface ICombinedPredicates<TSource>
        {
            ICombinedPredicates<TSource> And(Func<TSource, bool> predicate);
            bool Invoke(TSource x);
        }

        private sealed class CombinedPredicatesN<TSource> : ICombinedPredicates<TSource>
        {
            private readonly Func<TSource, bool>[] _predicates;

            public CombinedPredicatesN(params Func<TSource, bool>[] predicates)
            {
                _predicates = predicates;
            }

            public ICombinedPredicates<TSource> And(Func<TSource, bool> predicate)
            {
                var predicates = new Func<TSource, bool>[_predicates.Length + 1];
                Array.Copy(_predicates, predicates, _predicates.Length);
                predicates[_predicates.Length] = predicate;

                return new CombinedPredicatesN<TSource>(predicates);
            }

            public bool Invoke(TSource x)
            {
                foreach (var predicate in _predicates)
                {
                    if (!predicate(x))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private static Func<TSource, ValueTask<bool>> CombinePredicates<TSource>(Func<TSource, ValueTask<bool>> predicate1, Func<TSource, ValueTask<bool>> predicate2)
        {
            if (predicate1.Target is ICombinedAsyncPredicates<TSource> c)
            {
                return c.And(predicate2).Invoke;
            }
            else
            {
                return new CombinedAsyncPredicates2<TSource>(predicate1, predicate2).Invoke;
            }
        }

        private interface ICombinedAsyncPredicates<TSource>
        {
            ICombinedAsyncPredicates<TSource> And(Func<TSource, ValueTask<bool>> predicate);
            ValueTask<bool> Invoke(TSource x);
        }

        private sealed class CombinedAsyncPredicatesN<TSource> : ICombinedAsyncPredicates<TSource>
        {
            private readonly Func<TSource, ValueTask<bool>>[] _predicates;

            public CombinedAsyncPredicatesN(params Func<TSource, ValueTask<bool>>[] predicates)
            {
                _predicates = predicates;
            }

            public ICombinedAsyncPredicates<TSource> And(Func<TSource, ValueTask<bool>> predicate)
            {
                var predicates = new Func<TSource, ValueTask<bool>>[_predicates.Length + 1];
                Array.Copy(_predicates, predicates, _predicates.Length);
                predicates[_predicates.Length] = predicate;

                return new CombinedAsyncPredicatesN<TSource>(predicates);
            }

            public async ValueTask<bool> Invoke(TSource x)
            {
                foreach (var predicate in _predicates)
                {
                    if (!await predicate(x).ConfigureAwait(false))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

#if !NO_DEEP_CANCELLATION
        private static Func<TSource, CancellationToken, ValueTask<bool>> CombinePredicates<TSource>(Func<TSource, CancellationToken, ValueTask<bool>> predicate1, Func<TSource, CancellationToken, ValueTask<bool>> predicate2)
        {
            if (predicate1.Target is ICombinedAsyncPredicatesWithCancellation<TSource> c)
            {
                return c.And(predicate2).Invoke;
            }
            else
            {
                return new CombinedAsyncPredicatesWithCancellation2<TSource>(predicate1, predicate2).Invoke;
            }
        }

        private interface ICombinedAsyncPredicatesWithCancellation<TSource>
        {
            ICombinedAsyncPredicatesWithCancellation<TSource> And(Func<TSource, CancellationToken, ValueTask<bool>> predicate);
            ValueTask<bool> Invoke(TSource x, CancellationToken ct);
        }

        private sealed class CombinedAsyncPredicatesWithCancellationN<TSource> : ICombinedAsyncPredicatesWithCancellation<TSource>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<bool>>[] _predicates;

            public CombinedAsyncPredicatesWithCancellationN(params Func<TSource, CancellationToken, ValueTask<bool>>[] predicates)
            {
                _predicates = predicates;
            }

            public ICombinedAsyncPredicatesWithCancellation<TSource> And(Func<TSource, CancellationToken, ValueTask<bool>> predicate)
            {
                var predicates = new Func<TSource, CancellationToken, ValueTask<bool>>[_predicates.Length + 1];
                Array.Copy(_predicates, predicates, _predicates.Length);
                predicates[_predicates.Length] = predicate;

                return new CombinedAsyncPredicatesWithCancellationN<TSource>(predicates);
            }

            public async ValueTask<bool> Invoke(TSource x, CancellationToken ct)
            {
                foreach (var predicate in _predicates)
                {
                    if (!await predicate(x, ct).ConfigureAwait(false))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
#endif
    }
}
