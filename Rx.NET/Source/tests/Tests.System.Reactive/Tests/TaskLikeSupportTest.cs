// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.System.Reactive.Tests
{
    public class TaskLikeSupportTest
    {
        [Fact]
        public async Task Return()
        {
            Assert.Equal(42, await ManOrBoy_Return());
        }

#pragma warning disable 1998
        private async ITaskObservable<int> ManOrBoy_Return()
        {
            return 42;
        }
#pragma warning restore 1998

        [Fact]
        public async Task Throw()
        {
            await Assert.ThrowsAsync<DivideByZeroException>(async () => await ManOrBoy_Throw(42, 0));
        }

#pragma warning disable 1998
        private async ITaskObservable<int> ManOrBoy_Throw(int n, int d)
        {
            return n / d;
        }
#pragma warning restore 1998

        [Fact]
        public async Task Basics()
        {
            Assert.Equal(45, await ManOrBoy_Basics());
        }

#pragma warning disable 1998
        private async ITaskObservable<int> ManOrBoy_Basics()
        {
            var res = 0;

            for (var i = 0; i < 10; i++)
            {
                switch (i % 4)
                {
                    case 0:
                        res += await Observable.Return(i);
                        break;
                    case 1:
                        res += await Observable.Return(i).Delay(TimeSpan.FromMilliseconds(50));
                        break;
                    case 2:
                        res += await Task.FromResult(i);
                        break;
                    case 3:
                        res += await Task.Run(() => { Task.Delay(50).Wait(); return i; });
                        break;
                }
            }

            return res;
        }
#pragma warning restore 1998
    }
}
