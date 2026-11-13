// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Async
{
    /// <summary>
    /// Per-call cost of the <see cref="Unit"/>-returning <c>FromAsyncPattern</c> forms: the
    /// <c>Func&lt;IAsyncResult, Unit&gt;</c> end method at arity 0, and the <c>Action&lt;IAsyncResult&gt;</c> end
    /// method at arity 0 and 2. As in <c>FromAsyncPatternBenchmarks</c>, the <c>Begin</c> method completes
    /// synchronously (it invokes the callback inline with a pre-completed <see cref="IAsyncResult"/>), and each
    /// call creates an <c>AsyncAsyncSubject</c>. The end delegates are explicitly typed so that the generic
    /// <c>TResult</c> overloads cannot bind.
    /// </summary>
    [BenchmarkCategory("Async")]
    public class FromAsyncPatternUnitBenchmarks
    {
        private static readonly Func<IAsyncResult, Unit> EndUnit = static _ => Unit.Default;
        private static readonly Action<IAsyncResult> EndAction = static _ => { };

        private readonly Consumer _consumer = new();
        private Func<IAsyncObservable<Unit>> _unit0 = default!;
        private Func<IAsyncObservable<Unit>> _action0 = default!;
        private Func<int, int, IAsyncObservable<Unit>> _action2 = default!;

        [GlobalSetup]
        public void Setup()
        {
            _unit0 = AsyncObservable.FromAsyncPattern(ImmediateAsyncResult.BeginSynchronously, EndUnit);
            _action0 = AsyncObservable.FromAsyncPattern(ImmediateAsyncResult.BeginSynchronously, EndAction);
            _action2 = AsyncObservable.FromAsyncPattern<int, int>(static (a, b, callback, state) => ImmediateAsyncResult.BeginSynchronously(callback, state), EndAction);
        }

        [Benchmark]
        public ValueTask FromAsyncPattern_Unit_Arity0() => _unit0().SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask FromAsyncPattern_Action_Arity0() => _action0().SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask FromAsyncPattern_Action_Arity2() => _action2(1, 2).SubscribeConsumeAsync(_consumer);
    }
}
