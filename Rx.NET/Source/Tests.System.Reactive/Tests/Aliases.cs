// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;
using System.Reactive.Linq;
using System.Reactive.Observable.Aliases;

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
