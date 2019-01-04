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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, int>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryable.AggregateAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(int), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, int>>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "accumulator");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(int), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(int), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "accumulator");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AggregateAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "accumulator");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.AggregateAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
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
        public void AllAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AllAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AllAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AllAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AllAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
        public void AnyAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AnyAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AnyAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AnyAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.AnyAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync13()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync15()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync16()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync19()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(long), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(long), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync21()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync22()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync23()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(float), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(float), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync24()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync25()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync26()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(double), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(double), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync27()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync28()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync29()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(decimal), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(decimal), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync30()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync31()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync32()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync33()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync34()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync35()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync36()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync37()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync38()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync39()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void AverageAsync40()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.AverageAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None);
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
        public void CountAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.CountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void CountAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.CountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.CountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
        public void FirstAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
        public void FirstOrDefaultAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void FirstOrDefaultAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.FirstOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.FirstOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.GroupBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>)), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0, IAsyncEnumerable<int> arg1) => default(int)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, IAsyncEnumerable<int>, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy13()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy15()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy16()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, IAsyncEnumerable<int>, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0, IAsyncEnumerable<int> arg1) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy19()
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
        public void GroupBy20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy21()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy22()
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
        public void GroupBy23()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupBy24()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "elementSelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupBy<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoin3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoin4()
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
        public void GroupJoin5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void GroupJoin6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.GroupJoin<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, IAsyncEnumerable<int> arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Join3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Join4()
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
        public void Join5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, int, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), (int arg0, int arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Join6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outer");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "inner");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "outerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "innerKeySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.Join<int, int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>), EqualityComparer<int>.Default);
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
        public void LastAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
        public void LastOrDefaultAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LastOrDefaultAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LastOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LastOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
        public void LongCountAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LongCountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void LongCountAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.LongCountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.LongCountAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync15()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync16()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(long), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(long), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync19()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync21()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync22()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync23()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync24()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(float), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(float), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync25()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync26()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync27()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync28()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync29()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync30()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(double), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(double), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync31()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync32()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync33()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync34()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync35()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync36()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(decimal), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(decimal), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync37()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync38()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync39()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync40()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync41()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync42()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync43()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync44()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MaxAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MaxAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<long>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new long[] { default(long) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<long?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new long?[] { default(long?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<float>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new float[] { default(float) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<float?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new float?[] { default(float?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<double>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new double[] { default(double) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<double?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new double?[] { default(double?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new decimal[] { default(decimal) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new decimal?[] { default(decimal?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<int?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.MinAsync(new int?[] { default(int?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync13()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(long), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(long), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync15()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync16()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync19()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(float), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(float), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync21()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync22()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync23()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync24()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync25()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync26()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(double), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(double), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync27()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync28()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync29()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync30()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync31()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync32()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(decimal), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(decimal), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync33()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync34()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync35()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync36()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync37()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync38()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync39()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync40()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync41()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync42()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync43()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinAsync44()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.MinAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.MinAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
        }

        [Fact]
        public void OrderBy3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
        }

        [Fact]
        public void OrderBy4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default);
        }

        [Fact]
        public void OrderBy5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void OrderBy6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
        }

        [Fact]
        public void OrderByDescending3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
        }

        [Fact]
        public void OrderByDescending4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default);
        }

        [Fact]
        public void OrderByDescending5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void OrderByDescending6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.OrderByDescending<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Select2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Select3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Select4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Select5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Select6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Select<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, IAsyncEnumerable<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, IAsyncEnumerable<int>>>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "selector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<IAsyncEnumerable<int>>>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "selector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "selector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, IAsyncEnumerable<int>>>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "selector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<IAsyncEnumerable<int>>>>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "selector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "selector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "resultSelector");

            var res = AsyncQueryable.SelectMany<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<IAsyncEnumerable<int>>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
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
        public void SingleAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
        public void SingleOrDefaultAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SingleOrDefaultAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SingleOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>), CancellationToken.None), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SingleOrDefaultAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>), CancellationToken.None);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => true), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, bool>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => true);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhile2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhile3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhile4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhile5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SkipWhile6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(default(IAsyncQueryable<int>), (int arg0) => true), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, bool>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.SkipWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => true);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SumAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<float?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new float?[] { default(float?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<double?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new double?[] { default(double?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new decimal?[] { default(decimal?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<long>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new long[] { default(long) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<float>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new float[] { default(float) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<double>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new double[] { default(double) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new decimal[] { default(decimal) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<int?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new int?[] { default(int?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<long?>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryable.SumAsync(new long?[] { default(long?) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync13()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync14()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float?>), CancellationToken.None);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync17()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync18()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync19()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync20()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync21()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync22()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync23()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync24()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(long), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(long), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync25()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync26()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<long>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync27()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(float), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, float>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(float), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync28()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync29()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<float>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<float>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync30()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(double), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, double>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(double), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync31()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync32()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<double>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<double>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync33()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(decimal), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, decimal>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(decimal), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync34()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync35()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<decimal>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<decimal>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync36()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync37()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync38()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int?>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync39()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(Nullable<long>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, long?>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Nullable<long>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void SumAsync40()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<long?>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<long?>>>), CancellationToken.None), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.SumAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<long?>), CancellationToken.None);
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
        public void TakeWhile3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhile4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhile5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void TakeWhile6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.TakeWhile<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>));
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(ValueTask<int>));
        }

        [Fact]
        public void ThenBy3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
        }

        [Fact]
        public void ThenBy4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(int), Comparer<int>.Default);
        }

        [Fact]
        public void ThenBy5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void ThenBy6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenBy<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void ThenByDescending1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(ValueTask<int>));
        }

        [Fact]
        public void ThenByDescending2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
        }

        [Fact]
        public void ThenByDescending3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(int));
        }

        [Fact]
        public void ThenByDescending4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(int), Comparer<int>.Default);
        }

        [Fact]
        public void ThenByDescending5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0) => default(ValueTask<int>), Comparer<int>.Default);
        }

        [Fact]
        public void ThenByDescending6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(default(IOrderedAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ThenByDescending<int, int>(new int[0].ToAsyncEnumerable().AsAsyncQueryable().OrderBy(x => x), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToDictionaryAsync12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToDictionaryAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryable.ToLookupAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(Expression<Func<int, int>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), (int arg0) => default(int), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void ToLookupAsync12()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "elementSelector");

            var res = AsyncQueryable.ToLookupAsync<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default, CancellationToken.None);
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
        public void Where3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Where4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Where5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<bool>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Where6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<bool>>>)), ane => ane.ParamName == "predicate");

            var res = AsyncQueryable.Where<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<bool>));
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
        public void Zip2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "second");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Zip3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "second");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryable.Zip<int, int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

    }
}