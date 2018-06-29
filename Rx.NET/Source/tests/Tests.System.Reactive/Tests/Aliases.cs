// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Observable.Aliases;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public partial class AliasesTest : ReactiveTest
    {
        [Fact]
        public void Qbservable_Aliases()
        {
            var xs = Observable.Return(1).AsQbservable();

            Assert.True(xs.Filter(x => true).ToEnumerable().SequenceEqual(new[] { 1 }), "Filter");
            Assert.True(xs.Filter(x => true).Concat(xs.Filter(x => false)).ToEnumerable().SequenceEqual(new[] { 1 }), "Concat/Filter");
            Assert.True(xs.Map(x => x.ToString()).ToEnumerable().SequenceEqual(new[] { "1" }), "Map");
            Assert.True(xs.FlatMap(x => xs).ToEnumerable().SequenceEqual(new[] { 1 }), "FlatMap");
        }
    }
}
