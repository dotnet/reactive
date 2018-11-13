// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class AsyncEnumerableTests
    {
        protected static readonly IAsyncEnumerable<int> Return42 = new[] { 42 }.ToAsyncEnumerable();
        private static Func<Exception, bool> SingleInnerExceptionMatches(Exception ex) => e => ((AggregateException)e).Flatten().InnerExceptions.Single() == ex;

        private const int WaitTimeoutMs = 5000;

#pragma warning disable xUnit1013 // Public method should be marked as test
        public void AssertThrowsAsync<TException>(Task t)
        {
            AssertThrows<AggregateException>(() => t.Wait(WaitTimeoutMs), ex => ex.Flatten().InnerExceptions.Single() is TException);
        }

        public void AssertThrowsAsync(Task t, Exception e)
        {
            AssertThrows(() => t.Wait(WaitTimeoutMs), SingleInnerExceptionMatches(e));
        }

        public void AssertThrowsAsync<T>(ValueTask<T> t, Exception e)
        {
            AssertThrows(() => t.Wait(WaitTimeoutMs), SingleInnerExceptionMatches(e));
        }

        public async Task NoNextAsync<T>(IAsyncEnumerator<T> e)
        {
            Assert.False(await e.MoveNextAsync());
        }

        public async Task HasNextAsync<T>(IAsyncEnumerator<T> e, T value)
        {
            Assert.True(await e.MoveNextAsync());
            Assert.Equal(value, e.Current);
        }

        public async Task SequenceIdentity<T>(IAsyncEnumerable<T> enumerable)
        {
            var en1 = enumerable.GetAsyncEnumerator();
            var en2 = enumerable.GetAsyncEnumerator();

            Assert.Equal(en1.GetType(), en2.GetType());

            await en1.DisposeAsync();
            await en2.DisposeAsync();

            var res1 = await enumerable.ToList();
            var res2 = await enumerable.ToList();

            res1.ShouldAllBeEquivalentTo(res2);
        }
#pragma warning restore xUnit1013 // Public method should be marked as test

        protected static IAsyncEnumerable<TValue> Throw<TValue>(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

#if NO_TASK_FROMEXCEPTION
            var tcs = new TaskCompletionSource<bool>();
            tcs.TrySetException(exception);
            var moveNextThrows = new ValueTask<bool>(tcs.Task);
#else
            var moveNextThrows = new ValueTask<bool>(Task.FromException<bool>(exception));
#endif

            return AsyncEnumerable.CreateEnumerable(
                _ => AsyncEnumerable.CreateEnumerator<TValue>(
                    () => moveNextThrows,
                    current: null,
                    dispose: null)
            );
        }

        private void AssertThrows<E>(Action a, Func<E, bool> assert)
            where E : Exception
        {
            var hasFailed = false;

            try
            {
                a();
            }
            catch (E e)
            {
                Assert.True(assert(e));
                hasFailed = true;
            }

            if (!hasFailed)
            {
                Assert.True(false);
            }
        }
    }
}
