﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public partial class Tests
    {
        public void AssertThrows<E>(Action a)
            where E : Exception
        {
            Assert.Throws<E>(a);
        }

        public void AssertThrows<E>(Action a, Func<E, bool> assert)
            where E : Exception
        {
            try
            {
                a();
                Assert.True(false);
            }
            catch (E e)
            {
                Assert.True(assert(e));
            }
        }

        public void NoNext<T>(IEnumerator<T> e)
        {
            Assert.False(e.MoveNext());
        }

        public void HasNext<T>(IEnumerator<T> e, T value)
        {
            Assert.True(e.MoveNext());
            Assert.Equal(value, e.Current);
        }
    }
}
