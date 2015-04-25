using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reactive;

namespace ReactiveTests.Tests
{
#if !SIGNED
    [TestClass]
    public class ImmutableListTest
    {
        [TestMethod]
        public void ImmutableList_Basics()
        {
            var list = new ImmutableList<int>();

            Assert.IsTrue(list.Data.SequenceEqual(new int[] { }));

            list = list.Add(42);

            Assert.IsTrue(list.Data.SequenceEqual(new int[] { 42 }));

            list = list.Remove(42);

            Assert.IsTrue(list.Data.SequenceEqual(new int[] { }));

            list = list.Remove(42);

            Assert.IsTrue(list.Data.SequenceEqual(new int[] { }));

            list = list.Add(43);
            list = list.Add(44);
            list = list.Add(43);

            Assert.IsTrue(list.Data.SequenceEqual(new int[] { 43, 44, 43 }));

            list = list.Remove(43);

            Assert.IsTrue(list.Data.SequenceEqual(new int[] { 44, 43 }));

            list = list.Remove(43);

            Assert.IsTrue(list.Data.SequenceEqual(new int[] { 44 }));

            list = list.Remove(44);

            Assert.IsTrue(list.Data.SequenceEqual(new int[] { }));
        }

        [TestMethod]
        public void ImmutableList_Nulls()
        {
            var list = new ImmutableList<string>();

            Assert.IsTrue(list.Data.SequenceEqual(new string[] { }));

            list = list.Add(null);

            Assert.IsTrue(list.Data.SequenceEqual(new string[] { null }));

            list = list.Remove(null);

            Assert.IsTrue(list.Data.SequenceEqual(new string[] { }));
        }
        }
#endif
}
