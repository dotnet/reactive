// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;


namespace Tests
{
    public partial class AsyncTests
    {
#pragma warning disable xUnit1013 // Public method should be marked as test        
        public void AssertThrows<E>(Action a)
            where E : Exception
        {
            Assert.Throws<E>(a);
        }

        [Obsolete("Don't use this, use Assert.ThrowsAsync and await it", true)]
        public Task AssertThrows<E>(Func<Task> func)
            where E : Exception
        {
            return Assert.ThrowsAsync<E>(func);
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
            Assert.False(e.MoveNext().Result);
        }

        public void HasNext<T>(IAsyncEnumerator<T> e, T value)
        {
            Assert.True(e.MoveNext().Result);
            Assert.Equal(value, e.Current);
        }

        public async Task SequenceIdentity<T>(IAsyncEnumerable<T> enumerable)
        {
            var en1 = enumerable.GetEnumerator();
            var en2 = enumerable.GetEnumerator();

            Assert.Equal(en1.GetType(), en2.GetType());

            en1.Dispose();
            en2.Dispose();

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