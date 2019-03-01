// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Do : Tests
    {
        [Fact]
        public void Do_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, _ => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, _ => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, _ => { }, _ => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, _ => { }, _ => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(Action<int>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(Action<int>), () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, _ => { }, default(Action)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(Action<int>), _ => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, _ => { }, default(Action<Exception>), () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, _ => { }, _ => { }, default(Action)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(Action<int>), _ => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, _ => { }, default(Action<Exception>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, new MyObserver()));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(IObserver<int>)));
        }

        [Fact]
        public void Do1()
        {
            var n = 0;
            Enumerable.Range(0, 10).Do(x => n += x).ForEach(_ => { });
            Assert.Equal(45, n);
        }

        [Fact]
        public void Do2()
        {
            var n = 0;
            Enumerable.Range(0, 10).Do(x => n += x, () => n *= 2).ForEach(_ => { });
            Assert.Equal(90, n);
        }

        [Fact]
        public void Do3()
        {
            var ex = new MyException();
            var ok = false;
            AssertThrows<MyException>(() =>
                EnumerableEx.Throw<int>(ex).Do(x => { Assert.True(false); }, e => { Assert.Equal(ex, e); ok = true; }).ForEach(_ => { })
            );
            Assert.True(ok);
        }

        [Fact]
        public void Do4()
        {
            var obs = new MyObserver();
            Enumerable.Range(0, 10).Do(obs).ForEach(_ => { });

            Assert.True(obs.Done);
            Assert.Equal(45, obs.Sum);
        }

        [Fact]
        public void Do5()
        {
            var sum = 0;
            var done = false;
            Enumerable.Range(0, 10).Do(x => sum += x, ex => { throw ex; }, () => done = true).ForEach(_ => { });

            Assert.True(done);
            Assert.Equal(45, sum);
        }

        private sealed class MyObserver : IObserver<int>
        {
            public int Sum;
            public bool Done;

            public void OnCompleted()
            {
                Done = true;
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(int value)
            {
                Sum += value;
            }
        }

        private sealed class MyException : Exception
        {
        }
    }
}
