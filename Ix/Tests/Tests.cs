// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public partial class Tests
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

        public void NoNext<T>(IEnumerator<T> e)
        {
            Assert.IsFalse(e.MoveNext());
        }

        public void HasNext<T>(IEnumerator<T> e, T value)
        {
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(value, e.Current);
        }
    }
}
