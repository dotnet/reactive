// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AsyncQueryableExTests
    {
        [Fact]
        public void AggregateAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, int>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, int>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(int), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, int>>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "accumulator");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(int), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(int), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAwaitAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitAsync<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAwaitAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitAsync<int, int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "accumulator");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.AggregateAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAwaitWithCancellationAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitWithCancellationAsync<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAwaitWithCancellationAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitWithCancellationAsync<int, int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "accumulator");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.AggregateAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AllAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AllAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AllAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AllAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AllAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AllAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AnyAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AnyAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AnyAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AnyAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AnyAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AnyAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AnyAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AnyAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Append1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Append<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryable.Append<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void AverageAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new decimal?[] { default(decimal?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<long>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new long[] { default(long) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<float>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new float[] { default(float) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<double>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new double[] { default(double) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new decimal[] { default(decimal) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<int?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new int?[] { default(int?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<long?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new long?[] { default(long?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<float?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new float?[] { default(float?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<double?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.AverageAsync(new double?[] { default(double?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync13()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(long), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(long), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(float), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(float), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync15()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(double), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(double), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync16()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(decimal), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(decimal), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync19()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAwaitWithCancellationAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Cast1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Cast<int>(default(IAsyncQueryable<object>)), ane => ane.ParamName == "source");

            var res = AsyncQueryable.Cast<int>(new object[] { default(object) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Concat1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Concat<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Concat<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "second");

            var res = AsyncQueryable.Concat<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void ContainsAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ContainsAsync<int>(default(IAsyncQueryable<int>), 1, CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.ContainsAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ContainsAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ContainsAsync<int>(default(IAsyncQueryable<int>), 1, EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.ContainsAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void CountAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.CountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void CountAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.CountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void CountAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.CountAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void CountAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.CountAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void DefaultIfEmpty1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.DefaultIfEmpty<int>(default(IAsyncQueryable<int>)), ane => ane.ParamName == "source");

            var res = AsyncQueryable.DefaultIfEmpty<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DefaultIfEmpty2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.DefaultIfEmpty<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryable.DefaultIfEmpty<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Distinct<int>(default(IAsyncQueryable<int>)), ane => ane.ParamName == "source");

            var res = AsyncQueryable.Distinct<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Distinct<int>(default(IAsyncQueryable<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");

            var res = AsyncQueryable.Distinct<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void ElementAtAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ElementAtAsync<int>(default(IAsyncQueryable<int>), 1, CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.ElementAtAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ElementAtOrDefaultAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ElementAtOrDefaultAsync<int>(default(IAsyncQueryable<int>), 1, CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.ElementAtOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Except1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Except<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Except<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "second");

            var res = AsyncQueryable.Except<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Except2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Except<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), EqualityComparer<int>.Default), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Except<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "second");

            var res = AsyncQueryable.Except<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void FirstAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.FirstAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstOrDefaultAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.FirstOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstOrDefaultAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstOrDefaultAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstOrDefaultAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstOrDefaultAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstOrDefaultAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void GroupBy1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>)), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, IAsyncEnumerable<int>, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, IAsyncEnumerable<int>, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), default(Expression<Func<int, IAsyncEnumerable<int>, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), default(Expression<Func<int, IAsyncEnumerable<int>, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupByAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupByAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwait3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwait4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwait5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwait6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupByAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwait7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupByAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwait8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupByAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupByAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupByAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwaitWithCancellation3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwaitWithCancellation4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwaitWithCancellation5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwaitWithCancellation6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupByAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwaitWithCancellation7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupByAwaitWithCancellation8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupByAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoin1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int>>), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), default(Expression<Func<int, int>>), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), default(Expression<Func<int, IAsyncEnumerable<int>, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoin2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int>>), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), default(Expression<Func<int, int>>), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), default(Expression<Func<int, IAsyncEnumerable<int>, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoinAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoinAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoinAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoinAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Intersect1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Intersect<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Intersect<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "second");

            var res = AsyncQueryable.Intersect<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Intersect2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Intersect<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), EqualityComparer<int>.Default), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Intersect<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "second");

            var res = AsyncQueryable.Intersect<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Join1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(int), (int arg0) => default(int), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int>>), (int arg0) => default(int), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), default(Expression<Func<int, int>>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Join2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, int arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(int), (int arg0) => default(int), (int arg0, int arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int>>), (int arg0) => default(int), (int arg0, int arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), default(Expression<Func<int, int>>), (int arg0, int arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), default(Expression<Func<int, int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(int), (int arg0) => default(int), (int arg0, int arg1) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void JoinAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void JoinAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, int, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.JoinAwait<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void JoinAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void JoinAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.JoinAwaitWithCancellation<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void LastAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.LastAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastOrDefaultAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.LastOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastOrDefaultAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastOrDefaultAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastOrDefaultAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastOrDefaultAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastOrDefaultAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LongCountAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.LongCountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LongCountAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LongCountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LongCountAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LongCountAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LongCountAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LongCountAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<int?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new int?[] { default(int?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<long>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new long[] { default(long) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<long?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new long?[] { default(long?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<float>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new float[] { default(float) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<float?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new float?[] { default(float?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<double>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new double[] { default(double) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<double?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new double?[] { default(double?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new decimal[] { default(decimal) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync(new decimal?[] { default(decimal?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync13()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(long), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(long), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync15()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync16()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(float), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(float), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(double), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(double), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync19()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(decimal), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(decimal), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync21()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync22()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAwaitWithCancellationAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<float>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new float[] { default(float) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<float?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new float?[] { default(float?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<double>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new double[] { default(double) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<double?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new double?[] { default(double?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new decimal[] { default(decimal) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new decimal?[] { default(decimal?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<int?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new int?[] { default(int?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<long>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new long[] { default(long) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<long?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new long?[] { default(long?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(float), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(float), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync13()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(double), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(double), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync15()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync16()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(decimal), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(decimal), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync19()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(long), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(long), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync21()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync22()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAwaitWithCancellationAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void OfType1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OfType<int>(default(IAsyncQueryable<object>)), ane => ane.ParamName == "source");

            var res = AsyncQueryable.OfType<int>(new object[] { default(object) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void OrderBy1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
        }

        [Fact]
        public void OrderBy2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default);
        }

        [Fact]
        public void OrderByAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByAwait<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
        }

        [Fact]
        public void OrderByAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByAwait<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void OrderByAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
        }

        [Fact]
        public void OrderByAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void OrderByDescending1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
        }

        [Fact]
        public void OrderByDescending2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default);
        }

        [Fact]
        public void OrderByDescendingAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescendingAwait<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescendingAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescendingAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
        }

        [Fact]
        public void OrderByDescendingAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescendingAwait<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescendingAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescendingAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void OrderByDescendingAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescendingAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescendingAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescendingAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
        }

        [Fact]
        public void OrderByDescendingAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescendingAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescendingAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescendingAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void Prepend1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Prepend<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryable.Prepend<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Reverse1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Reverse<int>(default(IAsyncQueryable<int>)), ane => ane.ParamName == "source");

            var res = AsyncQueryable.Reverse<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Select1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Select2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectAwait<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectAwait<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(default(IAsyncQueryable<int>), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, IAsyncEnumerable<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, IAsyncEnumerable<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, IAsyncEnumerable<int>>>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "collectionSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, IAsyncEnumerable<int>>>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "collectionSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectManyAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectManyAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectManyAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectManyAwait<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectManyAwait3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<IAsyncEnumerable<int>>>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "collectionSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectManyAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectManyAwait4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<IAsyncEnumerable<int>>>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "collectionSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectManyAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectManyAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectManyAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectManyAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectManyAwaitWithCancellation<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectManyAwaitWithCancellation3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "collectionSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectManyAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectManyAwaitWithCancellation4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "collectionSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectManyAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectManyAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SequenceEqualAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SequenceEqualAsync<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), CancellationToken.None), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SequenceEqualAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), CancellationToken.None), ane => ane.ParamName == "second");

            var res = AsyncQueryable.SequenceEqualAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SequenceEqualAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SequenceEqualAsync<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SequenceEqualAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "second");

            var res = AsyncQueryable.SequenceEqualAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SingleAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleOrDefaultAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SingleOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleOrDefaultAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0) => true, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleOrDefaultAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleOrDefaultAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleOrDefaultAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleOrDefaultAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Skip1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Skip<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryable.Skip<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipLast1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipLast<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SkipLast<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhile1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(default(IAsyncQueryable<int>), (int arg0) => true), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhile2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => true), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, bool>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => true);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhileAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhileAwait<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhileAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhileAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhileAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhileAwait<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhileAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhileAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhileAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhileAwaitWithCancellation<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhileAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhileAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhileAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhileAwaitWithCancellation<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhileAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhileAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SumAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new decimal[] { default(decimal) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<int?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new int?[] { default(int?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<long?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new long?[] { default(long?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<float?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new float?[] { default(float?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<double?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new double?[] { default(double?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new decimal?[] { default(decimal?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<long>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new long[] { default(long) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<float>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new float[] { default(float) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<double>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new double[] { default(double) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(decimal), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(decimal), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync13()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync15()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync16()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(long), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(long), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync19()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(float), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(float), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(double), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(double), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAwaitWithCancellationAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAwaitWithCancellationAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Take1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Take<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryable.Take<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeLast1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeLast<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryable.TakeLast<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhile1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(default(IAsyncQueryable<int>), (int arg0) => true), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhile2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => true), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, bool>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => true);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhileAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhileAwait<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhileAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhileAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhileAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhileAwait<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhileAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhileAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhileAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhileAwaitWithCancellation<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhileAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhileAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhileAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhileAwaitWithCancellation<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhileAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhileAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void ThenBy1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(int));
        }

        [Fact]
        public void ThenBy2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(int), Comparer<int>.Default);
        }

        [Fact]
        public void ThenByAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByAwait<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByAwait<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByAwait<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(ValueTask<int>));
        }

        [Fact]
        public void ThenByAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByAwait<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByAwait<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByAwait<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void ThenByAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByAwaitWithCancellation<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByAwaitWithCancellation<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByAwaitWithCancellation<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
        }

        [Fact]
        public void ThenByAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByAwaitWithCancellation<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByAwaitWithCancellation<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByAwaitWithCancellation<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void ThenByDescending1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(int));
        }

        [Fact]
        public void ThenByDescending2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(int), Comparer<int>.Default);
        }

        [Fact]
        public void ThenByDescendingAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescendingAwait<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescendingAwait<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescendingAwait<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(ValueTask<int>));
        }

        [Fact]
        public void ThenByDescendingAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescendingAwait<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescendingAwait<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescendingAwait<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void ThenByDescendingAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescendingAwaitWithCancellation<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescendingAwaitWithCancellation<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescendingAwaitWithCancellation<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
        }

        [Fact]
        public void ThenByDescendingAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescendingAwaitWithCancellation<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescendingAwaitWithCancellation<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescendingAwaitWithCancellation<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void ToArrayAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToArrayAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.ToArrayAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAwaitAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAwaitAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAwaitAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAwaitWithCancellationAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAwaitWithCancellationAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAwaitWithCancellationAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToHashSetAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToHashSetAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.ToHashSetAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToHashSetAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToHashSetAsync<int>(default(IAsyncQueryable<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.ToHashSetAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToListAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToListAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.ToListAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAwaitAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAwaitAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAwaitAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAwaitAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAwaitAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAwaitAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAwaitWithCancellationAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAwaitWithCancellationAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAwaitWithCancellationAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAwaitWithCancellationAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAwaitWithCancellationAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Union1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Union<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Union<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "second");

            var res = AsyncQueryable.Union<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Union2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Union<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), EqualityComparer<int>.Default), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Union<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "second");

            var res = AsyncQueryable.Union<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Where1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(default(IAsyncQueryable<int>), (int arg0) => true), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Where2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => true), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, bool>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => true);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void WhereAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.WhereAwait<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.WhereAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.WhereAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void WhereAwait2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.WhereAwait<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.WhereAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.WhereAwait<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void WhereAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.WhereAwaitWithCancellation<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.WhereAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.WhereAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void WhereAwaitWithCancellation2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.WhereAwaitWithCancellation<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.WhereAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.WhereAwaitWithCancellation<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Zip1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "second");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void ZipAwait1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ZipAwait<int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ZipAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "second");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ZipAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.ZipAwait<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void ZipAwaitWithCancellation1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ZipAwaitWithCancellation<int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ZipAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "second");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ZipAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.ZipAwaitWithCancellation<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

    }
}