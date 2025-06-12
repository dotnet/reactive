// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Observable.Aliases;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{

    [TestClass]
    public partial class AliasesTest : ReactiveTest
    {
        [TestMethod]
        public void Qbservable_Aliases()
        {
            var xs = Observable.Return(1).AsQbservable();

            Assert.True(xs.Filter(x => true).ToEnumerable().SequenceEqual([1]), "Filter");
            Assert.True(xs.Filter(x => true).Concat(xs.Filter(x => false)).ToEnumerable().SequenceEqual([1]), "Concat/Filter");
            Assert.True(xs.Map(x => x.ToString()).ToEnumerable().SequenceEqual(["1"]), "Map");
            Assert.True(xs.FlatMap(x => xs).ToEnumerable().SequenceEqual([1]), "FlatMap");
        }
    }
}
