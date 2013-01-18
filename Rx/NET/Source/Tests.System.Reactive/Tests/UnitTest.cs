// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Unit()
        {
            var u1 = new Unit();
            var u2 = new Unit();
            Assert.IsTrue(u1.Equals(u2));
            Assert.IsFalse(u1.Equals(""));
            Assert.IsFalse(u1.Equals(null));
            Assert.IsTrue(u1 == u2);
            Assert.IsFalse(u1 != u2);
            Assert.AreEqual(0, u1.GetHashCode());
            Assert.AreEqual("()", u1.ToString());
        }
    }
}
