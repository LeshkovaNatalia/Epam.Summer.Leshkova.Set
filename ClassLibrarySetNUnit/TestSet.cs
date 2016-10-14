using ClassLibraryLogicSet;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace ClassLibrarySetNUnit
{
    [TestFixture]
    public class TestSet
    {
        private readonly Set<string> firstSet = new Set<string>(new[] { "One", "Two" });
        private readonly Set<string> secondSet = new Set<string>(new[] { "Two", "Three", "One", "Five" });

        [Test]
        public void TestSet_CountString_ReturnCountSet()
        {
            int expected = 2;

            int actual = firstSet.Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSet_AddString_ReturnNewSet()
        {
            Set<string> expected = new Set<string>(new[] { "One", "Two", "Seven" }); ;

            firstSet.Add("Seven");
            Set<string> actual = firstSet;

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [Test]
        public void TestSet_RemoveString_ReturnNewSet()
        {
            Set<string> expected = new Set<string>(new[] { "Two", "One", "Five" }); ;

            firstSet.Remove("Three");
            Set<string> actual = firstSet;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSet_UnionString_ReturnUnionSet()
        {
            Set<string> expected = new Set<string>(new[] { "Two", "Three", "One", "Five" });

            Set<string> actual = Set<string>.Union(firstSet, secondSet);

            Assert.AreEqual(expected.SetElements, actual.SetElements);
        }
    }
}
