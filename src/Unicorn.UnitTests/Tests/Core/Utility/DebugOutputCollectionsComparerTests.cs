using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Utility;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UnitTests.Util;
using Verify = Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests.Tests.Core.Utility
{
    [TestFixture]
    public class DebugOutputCollectionsComparerTests : NUnitTestRunner
    {
        //[Test] // Need to debug output
        public void TestCollectionsComparerOutput()
        {
            CollectionsComparer<int> comparer = new CollectionsComparer<int>()
                .TrimOutputTo(500)
                .UseItemsBulletsInOutput(">");

            CollectionsComparer<string> strComparer = new CollectionsComparer<string>()
                .TrimOutputTo(50)
                .UseItemsBulletsInOutput(" ");

            string diff = "AreSequenceEqualTo" + Environment.NewLine;
            comparer.AreSequenceEqual(new[] { 1, 2, 3, 4, }, new[] { 1, 2, 5, 4, 6 });
            diff += comparer.Output + Environment.NewLine + new string('-', 60) + Environment.NewLine + Environment.NewLine;

            diff += "AreTheSame" + Environment.NewLine;
            comparer.AreTheSame(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 5, 4, 6 });
            diff += comparer.Output + Environment.NewLine + new string('-', 60) + Environment.NewLine + Environment.NewLine;

            diff += "Contains" + Environment.NewLine;
            strComparer.Contains(StringsCollection(2, 5), StringsCollection(1, 6));
            diff += strComparer.Output + Environment.NewLine + new string('-', 60) + Environment.NewLine + Environment.NewLine;

            diff += "NotContains" + Environment.NewLine;
            comparer.NotContains(IntsCollection(2, 5), IntsCollection(1, 6));
            diff += comparer.Output;

            Assert.Fail(diff);
        }

        //[Test] // Need to debug output
        public void TestCollectionsComparerOutput1()
        {
            string diff = MatchersUtils.GetStringsDiff(
                "edkfj@asldkfj#sdfsdfsdf",
                "edkfj@asldkfj#1sdfsdfsdf");

            diff += Environment.NewLine;

            diff += MatchersUtils.GetStringsDiff(
                "edkfj@asldkfj#sdfsdfsdf",
                "edkfj@asldkfj#1");

            diff += Environment.NewLine;

            diff += MatchersUtils.GetStringsDiff(
                "edkfj@asldkfj#s",
                "edkfj@asldkfj#1esrgdlhlkdfjsgh90845gh945gh0345698gh9453g8h");

            diff += Environment.NewLine;

            diff += MatchersUtils.GetStringsDiff(
                "sdfgdflsgkhjf8943tdg5667dfklvjh569ghbedkfj@asldkfj#ssdfgdflsgkhjf8943tdg5667dfklvjh569ghb",
                "sdfgdflsgkhjf8943tdg5667dfklvjh569ghbedkfj@asldkfj#1sdfgdflsgkhjf8943tdg5667dfklvjh569ghb");

            Assert.Fail(diff);
        }

        //[Test] // Need to debug output
        public void SequenceEqualTo() =>
            Verify.Assert.That(StringsCollection(2, 7),
                Collection.IsSequenceEqualTo(StringsCollection(2, 9)));

        //[Test] // Need to debug output
        public void IsTheSameAs() =>
            Verify.Assert.That(new[] { 1, 2, 3, 4 },
                Collection.IsTheSameAs(new[] { 1, 2, 5, 4, 6 }));

        //[Test] // Need to debug output
        public void Contains() =>
            Verify.Assert.That(StringsCollection(2, 5),
                Collection.HasItems(StringsCollection(1, 6)));

        //[Test] // Need to debug output
        public void NotContains() =>
            Verify.Assert.That(StringsCollection(2, 5),
                Verify.Matchers.Is.Not(Collection.HasItems(StringsCollection(1, 6))));

        private static IEnumerable<string> StringsCollection(int from, int to) =>
            Enumerable.Range(from, to - from + 1).Select(i => "string" + i);

        private static List<int> IntsCollection(int from, int to) =>
            Enumerable.Range(from, to - from + 1).ToList();
    }
}
