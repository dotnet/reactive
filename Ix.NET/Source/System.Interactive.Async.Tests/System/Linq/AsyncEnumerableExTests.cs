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
    public class AsyncEnumerableExTests
    {
        protected static readonly IAsyncEnumerable<int> Return42 = AsyncEnumerableEx.Return(42);
        protected static IAsyncEnumerable<T> Throw<T>(Exception exception) => AsyncEnumerable.Throw<T>(exception);
        protected static Func<Exception, bool> SingleInnerExceptionMatches(Exception ex) => e => ((AggregateException)e).Flatten().InnerExceptions.Single() == ex;

        protected const int WaitTimeoutMs = 5000;

#pragma warning disable xUnit1013 // Public method should be marked as test
        public void AssertThrows<E>(Action a)
            where E : Exception
        {
            Assert.Throws<E>(a);
        }

        public void AssertThrows<E>(Action a, Func<E, bool> assert)
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

        public void NoNext<T>(IAsyncEnumerator<T> e)
        {
            Assert.False(e.MoveNextAsync().Result);
        }

        public void HasNext<T>(IAsyncEnumerator<T> e, T value)
        {
            Assert.True(e.MoveNextAsync().Result);
            Assert.Equal(value, e.Current);
        }

        public async Task SequenceIdentity<T>(IAsyncEnumerable<T> enumerable)
        {
            var en1 = enumerable.GetAsyncEnumerator();
            var en2 = enumerable.GetAsyncEnumerator();

            Assert.Equal(en1.GetType(), en2.GetType());

            await en1.DisposeAsync();
            await en2.DisposeAsync();

            var e1t = enumerable.ToList();
            var e2t = enumerable.ToList();

            await Task.WhenAll(e1t, e2t);


            var e1Result = e1t.Result;
            var e2Result = e2t.Result;

            e1Result.ShouldAllBeEquivalentTo(e2Result);
        }
#pragma warning restore xUnit1013 // Public method should be marked as test
    }
}
