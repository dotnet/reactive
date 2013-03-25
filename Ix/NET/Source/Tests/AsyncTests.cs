// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if !NO_TPL

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public partial class AsyncTests
    {
        public void AssertThrows<E>(Action a)
            where E : Exception
        {
            try
            {
                a();
                Assert.Fail();
            }
            catch (E)
            {
            }
        }

        public void AssertThrows<E>(Action a, Func<E, bool> assert)
            where E : Exception
        {
            try
            {
                a();
                Assert.Fail();
            }
            catch (E e)
            {
                Assert.IsTrue(assert(e));
            }
        }

        public void NoNext<T>(IAsyncEnumerator<T> e)
        {
            Assert.IsFalse(e.MoveNext().Result);
        }

        public void HasNext<T>(IAsyncEnumerator<T> e, T value)
        {
            Assert.IsTrue(e.MoveNext().Result);
            Assert.AreEqual(value, e.Current);
        }
    }
}

#endif