// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Tests
{
    public class Create : Tests
    {
        [Fact]
        public void Create_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Create<int>(default(Func<IEnumerator<int>>)));
        }

        [Fact]
        public void Create1()
        {
            var hot = false;
            var res = EnumerableEx.Create<int>(() =>
            {
                hot = true;
                return MyEnumerator();
            });

            Assert.False(hot);

            var e = res.GetEnumerator();
            Assert.True(hot);

            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);

            hot = false;
            var f = ((IEnumerable)res).GetEnumerator();
            Assert.True(hot);
        }

        [Fact]
        public void CreateYield()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var xs = EnumerableEx.Create<int>(async yield =>
            {
                var i = 0;
                while (i < 10)
                {
                    await yield.Return(i++);
                }
            });

            var j = 0;
            foreach (var elem in xs)
            {
                Assert.Equal(j, elem);
                j++;
            }

            Assert.Equal(10, j);
        }

        [Fact]
        public void CreateYieldBreak()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var xs = EnumerableEx.Create<int>(async yield =>
            {
                var i = 0;
                while (true)
                {
                    if (i == 10)
                    {
                        await yield.Break();
                        return;
                    }

                    await yield.Return(i++);
                }
            });

            var j = 0;
            foreach (var elem in xs)
            {
                Assert.Equal(elem, j);
                j++;
            }

            Assert.Equal(10, j);
        }

        [Fact]
        public void YielderNoReset()
        {
            var xs = EnumerableEx.Create<int>(async yield =>
            {
                await yield.Break();
            });

            AssertThrows<NotSupportedException>(() => xs.GetEnumerator().Reset());
        }

        private static IEnumerator<int> MyEnumerator()
        {
            yield return 1;
            yield return 2;
        }
    }
}
