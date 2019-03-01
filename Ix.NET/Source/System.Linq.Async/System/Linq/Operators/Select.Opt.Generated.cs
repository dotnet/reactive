// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        private sealed class CombinedSelectors2<TSource, TMiddle1, TResult> : ICombinedSelectors<TSource, TResult>
        {
            private readonly Func<TSource, TMiddle1> _selector1;
            private readonly Func<TMiddle1, TResult> _selector2;

            public CombinedSelectors2(Func<TSource, TMiddle1> selector1, Func<TMiddle1, TResult> selector2)
            {
                _selector1 = selector1;
                _selector2 = selector2;
            }

            public ICombinedSelectors<TSource, TNewResult> Combine<TNewResult>(Func<TResult, TNewResult> selector) =>
                new CombinedSelectors3<TSource, TMiddle1, TResult, TNewResult>(
                    _selector1,
                    _selector2,
                    selector
                );

            public TResult Invoke(TSource x) => _selector2(_selector1(x));
        }

        private sealed class CombinedSelectors3<TSource, TMiddle1, TMiddle2, TResult> : ICombinedSelectors<TSource, TResult>
        {
            private readonly Func<TSource, TMiddle1> _selector1;
            private readonly Func<TMiddle1, TMiddle2> _selector2;
            private readonly Func<TMiddle2, TResult> _selector3;

            public CombinedSelectors3(Func<TSource, TMiddle1> selector1, Func<TMiddle1, TMiddle2> selector2, Func<TMiddle2, TResult> selector3)
            {
                _selector1 = selector1;
                _selector2 = selector2;
                _selector3 = selector3;
            }

            public ICombinedSelectors<TSource, TNewResult> Combine<TNewResult>(Func<TResult, TNewResult> selector) =>
                new CombinedSelectors4<TSource, TMiddle1, TMiddle2, TResult, TNewResult>(
                    _selector1,
                    _selector2,
                    _selector3,
                    selector
                );

            public TResult Invoke(TSource x) => _selector3(_selector2(_selector1(x)));
        }

        private sealed class CombinedSelectors4<TSource, TMiddle1, TMiddle2, TMiddle3, TResult> : ICombinedSelectors<TSource, TResult>
        {
            private readonly Func<TSource, TMiddle1> _selector1;
            private readonly Func<TMiddle1, TMiddle2> _selector2;
            private readonly Func<TMiddle2, TMiddle3> _selector3;
            private readonly Func<TMiddle3, TResult> _selector4;

            public CombinedSelectors4(Func<TSource, TMiddle1> selector1, Func<TMiddle1, TMiddle2> selector2, Func<TMiddle2, TMiddle3> selector3, Func<TMiddle3, TResult> selector4)
            {
                _selector1 = selector1;
                _selector2 = selector2;
                _selector3 = selector3;
                _selector4 = selector4;
            }

            public ICombinedSelectors<TSource, TNewResult> Combine<TNewResult>(Func<TResult, TNewResult> selector) =>
                new CombinedSelectors2<TSource, TResult, TNewResult>(this.Invoke, selector);

            public TResult Invoke(TSource x) => _selector4(_selector3(_selector2(_selector1(x))));
        }

        private sealed class CombinedAsyncSelectors2<TSource, TMiddle1, TResult> : ICombinedAsyncSelectors<TSource, TResult>
        {
            private readonly Func<TSource, ValueTask<TMiddle1>> _selector1;
            private readonly Func<TMiddle1, ValueTask<TResult>> _selector2;

            public CombinedAsyncSelectors2(Func<TSource, ValueTask<TMiddle1>> selector1, Func<TMiddle1, ValueTask<TResult>> selector2)
            {
                _selector1 = selector1;
                _selector2 = selector2;
            }

            public ICombinedAsyncSelectors<TSource, TNewResult> Combine<TNewResult>(Func<TResult, ValueTask<TNewResult>> selector) =>
                new CombinedAsyncSelectors3<TSource, TMiddle1, TResult, TNewResult>(
                    _selector1,
                    _selector2,
                    selector
                );

            public async ValueTask<TResult> Invoke(TSource x) => await _selector2(await _selector1(x).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private sealed class CombinedAsyncSelectors3<TSource, TMiddle1, TMiddle2, TResult> : ICombinedAsyncSelectors<TSource, TResult>
        {
            private readonly Func<TSource, ValueTask<TMiddle1>> _selector1;
            private readonly Func<TMiddle1, ValueTask<TMiddle2>> _selector2;
            private readonly Func<TMiddle2, ValueTask<TResult>> _selector3;

            public CombinedAsyncSelectors3(Func<TSource, ValueTask<TMiddle1>> selector1, Func<TMiddle1, ValueTask<TMiddle2>> selector2, Func<TMiddle2, ValueTask<TResult>> selector3)
            {
                _selector1 = selector1;
                _selector2 = selector2;
                _selector3 = selector3;
            }

            public ICombinedAsyncSelectors<TSource, TNewResult> Combine<TNewResult>(Func<TResult, ValueTask<TNewResult>> selector) =>
                new CombinedAsyncSelectors4<TSource, TMiddle1, TMiddle2, TResult, TNewResult>(
                    _selector1,
                    _selector2,
                    _selector3,
                    selector
                );

            public async ValueTask<TResult> Invoke(TSource x) => await _selector3(await _selector2(await _selector1(x).ConfigureAwait(false)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private sealed class CombinedAsyncSelectors4<TSource, TMiddle1, TMiddle2, TMiddle3, TResult> : ICombinedAsyncSelectors<TSource, TResult>
        {
            private readonly Func<TSource, ValueTask<TMiddle1>> _selector1;
            private readonly Func<TMiddle1, ValueTask<TMiddle2>> _selector2;
            private readonly Func<TMiddle2, ValueTask<TMiddle3>> _selector3;
            private readonly Func<TMiddle3, ValueTask<TResult>> _selector4;

            public CombinedAsyncSelectors4(Func<TSource, ValueTask<TMiddle1>> selector1, Func<TMiddle1, ValueTask<TMiddle2>> selector2, Func<TMiddle2, ValueTask<TMiddle3>> selector3, Func<TMiddle3, ValueTask<TResult>> selector4)
            {
                _selector1 = selector1;
                _selector2 = selector2;
                _selector3 = selector3;
                _selector4 = selector4;
            }

            public ICombinedAsyncSelectors<TSource, TNewResult> Combine<TNewResult>(Func<TResult, ValueTask<TNewResult>> selector) =>
                new CombinedAsyncSelectors2<TSource, TResult, TNewResult>(this.Invoke, selector);

            public async ValueTask<TResult> Invoke(TSource x) => await _selector4(await _selector3(await _selector2(await _selector1(x).ConfigureAwait(false)).ConfigureAwait(false)).ConfigureAwait(false)).ConfigureAwait(false);
        }

#if !NO_DEEP_CANCELLATION
        private sealed class CombinedAsyncSelectorsWithCancellation2<TSource, TMiddle1, TResult> : ICombinedAsyncSelectorsWithCancellation<TSource, TResult>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<TMiddle1>> _selector1;
            private readonly Func<TMiddle1, CancellationToken, ValueTask<TResult>> _selector2;

            public CombinedAsyncSelectorsWithCancellation2(Func<TSource, CancellationToken, ValueTask<TMiddle1>> selector1, Func<TMiddle1, CancellationToken, ValueTask<TResult>> selector2)
            {
                _selector1 = selector1;
                _selector2 = selector2;
            }

            public ICombinedAsyncSelectorsWithCancellation<TSource, TNewResult> Combine<TNewResult>(Func<TResult, CancellationToken, ValueTask<TNewResult>> selector) =>
                new CombinedAsyncSelectorsWithCancellation3<TSource, TMiddle1, TResult, TNewResult>(
                    _selector1,
                    _selector2,
                    selector
                );

            public async ValueTask<TResult> Invoke(TSource x, CancellationToken ct) => await _selector2(await _selector1(x, ct).ConfigureAwait(false), ct).ConfigureAwait(false);
        }

        private sealed class CombinedAsyncSelectorsWithCancellation3<TSource, TMiddle1, TMiddle2, TResult> : ICombinedAsyncSelectorsWithCancellation<TSource, TResult>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<TMiddle1>> _selector1;
            private readonly Func<TMiddle1, CancellationToken, ValueTask<TMiddle2>> _selector2;
            private readonly Func<TMiddle2, CancellationToken, ValueTask<TResult>> _selector3;

            public CombinedAsyncSelectorsWithCancellation3(Func<TSource, CancellationToken, ValueTask<TMiddle1>> selector1, Func<TMiddle1, CancellationToken, ValueTask<TMiddle2>> selector2, Func<TMiddle2, CancellationToken, ValueTask<TResult>> selector3)
            {
                _selector1 = selector1;
                _selector2 = selector2;
                _selector3 = selector3;
            }

            public ICombinedAsyncSelectorsWithCancellation<TSource, TNewResult> Combine<TNewResult>(Func<TResult, CancellationToken, ValueTask<TNewResult>> selector) =>
                new CombinedAsyncSelectorsWithCancellation4<TSource, TMiddle1, TMiddle2, TResult, TNewResult>(
                    _selector1,
                    _selector2,
                    _selector3,
                    selector
                );

            public async ValueTask<TResult> Invoke(TSource x, CancellationToken ct) => await _selector3(await _selector2(await _selector1(x, ct).ConfigureAwait(false), ct).ConfigureAwait(false), ct).ConfigureAwait(false);
        }

        private sealed class CombinedAsyncSelectorsWithCancellation4<TSource, TMiddle1, TMiddle2, TMiddle3, TResult> : ICombinedAsyncSelectorsWithCancellation<TSource, TResult>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<TMiddle1>> _selector1;
            private readonly Func<TMiddle1, CancellationToken, ValueTask<TMiddle2>> _selector2;
            private readonly Func<TMiddle2, CancellationToken, ValueTask<TMiddle3>> _selector3;
            private readonly Func<TMiddle3, CancellationToken, ValueTask<TResult>> _selector4;

            public CombinedAsyncSelectorsWithCancellation4(Func<TSource, CancellationToken, ValueTask<TMiddle1>> selector1, Func<TMiddle1, CancellationToken, ValueTask<TMiddle2>> selector2, Func<TMiddle2, CancellationToken, ValueTask<TMiddle3>> selector3, Func<TMiddle3, CancellationToken, ValueTask<TResult>> selector4)
            {
                _selector1 = selector1;
                _selector2 = selector2;
                _selector3 = selector3;
                _selector4 = selector4;
            }

            public ICombinedAsyncSelectorsWithCancellation<TSource, TNewResult> Combine<TNewResult>(Func<TResult, CancellationToken, ValueTask<TNewResult>> selector) =>
                new CombinedAsyncSelectorsWithCancellation2<TSource, TResult, TNewResult>(this.Invoke, selector);

            public async ValueTask<TResult> Invoke(TSource x, CancellationToken ct) => await _selector4(await _selector3(await _selector2(await _selector1(x, ct).ConfigureAwait(false), ct).ConfigureAwait(false), ct).ConfigureAwait(false), ct).ConfigureAwait(false);
        }

#endif
    }
}
