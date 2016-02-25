// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if !NO_TPL

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    
    public partial class AsyncTests
    {
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
            Assert.False(e.MoveNext().Result);
        }

        public void HasNext<T>(IAsyncEnumerator<T> e, T value)
        {
            Assert.True(e.MoveNext().Result);
            Assert.Equal(value, e.Current);
        }
    }
}

#endif