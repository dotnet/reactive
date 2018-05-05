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
    public class InternalRangeTest : ReactiveTest
    {
        [Fact]
        public void InternalRange_Basic()
        {
            var actual = new InternalRange(1, 5).ToList().First();

            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5 }, actual);
        }
    }
}
