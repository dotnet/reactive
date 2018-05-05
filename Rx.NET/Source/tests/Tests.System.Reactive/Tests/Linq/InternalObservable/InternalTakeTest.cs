using Microsoft.Reactive.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Reactive.Linq;
using System.Reactive.Linq.InternalObservable;

namespace Tests.System.Reactive.Tests.Linq.InternalObservable
{
    public class InternalTakeTest : ReactiveTest
    {
        [Fact]
        public void InternalTake_Basic()
        {
            var actual = new InternalTake<int>(new InternalRange(1, 5), 3)
                .ToList().First();

            Assert.Equal(new List<int>() { 1, 2, 3 }, actual);
        }

        [Fact]
        public void InternalTake_Take_Twice()
        {
            var actual = new InternalTake<int>(new InternalTake<int>(new InternalRange(1, 5), 4), 2)
                .ToList().First();

            Assert.Equal(new List<int>() { 1, 2 }, actual);
        }
    }
}
