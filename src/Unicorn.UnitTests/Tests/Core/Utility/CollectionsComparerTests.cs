using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Utility;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests.Core.Utility
{
    [TestFixture]
    public class CollectionsComparerTests : NUnitTestRunner
    {
        private CollectionsComparer<string> stringComparer = new CollectionsComparer<string>();
        private CollectionsComparer<int> intComparer = new CollectionsComparer<int>();

        [Test]
        public void TestCollectionsComparerHasSameItemsEqualPositive() =>
            Assert.IsTrue(stringComparer.AreTheSame(
                StringCollectionFromRange(0, 25), StringCollectionFromRange(0, 25)));

        [Test]
        public void TestCollectionsComparerHasSameItemsNegative() =>
            Assert.IsFalse(stringComparer.AreTheSame(
                StringCollectionFromRange(0, 23), StringCollectionFromRange(0, 25)));

        [Test]
        public void TestCollectionsComparerHasSameItemsReverseNegative() =>
                Assert.IsFalse(stringComparer.AreTheSame(
                StringCollectionFromRange(0, 23), StringCollectionFromRange(0, 25).Reverse()));

        [Test]
        public void TestCollectionsComparerHasSameItemsEqualReversedWithDuplicatesIntNegative() =>
            Assert.IsFalse(intComparer.AreTheSame(
                new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5, 2 }));

        [Test]
        public void TestCollectionsComparerHasSameItemsEqualReversedWithDuplicatesViceVersaIntNegative() =>
            Assert.IsFalse(intComparer.AreTheSame(
                new[] { 1, 2, 3, 4, 5, 2 }, new[] { 1, 2, 3, 4, 5 }));

        [Test]
        public void TestCollectionsComparerHasSameItemsEqualReversedPositive() =>
            Assert.IsTrue(stringComparer.AreTheSame(
                StringCollectionFromRange(0, 25), StringCollectionFromRange(0, 25).Reverse()));

        [Test]
        public void TestCollectionsComparerHasSameItemsEqualReversedIntPositive() =>
            Assert.IsTrue(intComparer.AreTheSame(
                IntCollectionFromRange(0, 25), IntCollectionFromRange(0, 25)));

        [Test]
        public void TestCollectionsComparerContainsEqualIntPositive() =>
            Assert.IsTrue(intComparer.Contains(
                IntCollectionFromRange(0, 25), IntCollectionFromRange(0, 25)));

        [Test]
        public void TestCollectionsComparerContainsEqualStringReversedPositive() =>
            Assert.IsTrue(stringComparer.Contains(
                StringCollectionFromRange(0, 25), StringCollectionFromRange(0, 25).Reverse()));

        [Test]
        public void TestCollectionsComparerContainsStringReversedPositive() =>
            Assert.IsTrue(stringComparer.Contains(
                StringCollectionFromRange(0, 26), StringCollectionFromRange(1, 23).Reverse()));

        [Test]
        public void TestCollectionsComparerContainsNegative() =>
                Assert.IsFalse(stringComparer.Contains(
                StringCollectionFromRange(0, 23), StringCollectionFromRange(0, 25).Reverse()));

        [Test]
        public void TestCollectionsComparerContainsNoInterceptionNegative() =>
                Assert.IsFalse(stringComparer.Contains(
                StringCollectionFromRange(0, 5), StringCollectionFromRange(6, 7).Reverse()));

        [Test]
        public void TestCollectionsComparerNotContainsEqualStringPositive() =>
            Assert.IsTrue(stringComparer.NotContains(
                StringCollectionFromRange(0, 25), StringCollectionFromRange(26, 27)));

        [Test]
        public void TestCollectionsComparerNotContainsEqualStringReversePositive() =>
            Assert.IsTrue(stringComparer.NotContains(
                StringCollectionFromRange(0, 25), StringCollectionFromRange(26, 27).Reverse()));


        [Test]
        public void TestCollectionsComparerNotContainsNoInterceptionEqualsNegative() =>
                Assert.IsFalse(stringComparer.NotContains(
                StringCollectionFromRange(0, 5), StringCollectionFromRange(0, 5).Reverse()));

        [Test]
        public void TestCollectionsComparerNotContainsNoInterceptionIntersectionNegative() =>
                Assert.IsFalse(stringComparer.NotContains(
                StringCollectionFromRange(0, 5), StringCollectionFromRange(2, 3).Reverse()));

        private static IEnumerable<string> StringCollectionFromRange(int from, int to) =>
            Enumerable.Range(from, to - from).Select(i => "string" + i);

        private static List<int> IntCollectionFromRange(int from, int to) =>
            Enumerable.Range(from, to - from).ToList();
    }
}
