using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicorn.Taf.Core.Utility
{
    /// <summary>
    /// Utility for collections comparison. Available modes:
    ///  - equality ignoring order
    ///  - sequential equality
    ///  - contains
    ///  - does not contain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionsComparer<T>
    {
        private const string NotExpected = "Not expected items >>";
        private const string Absent = "Absent items >>";
        private const string Diff = "Diff >>";

        private string delimiter = Environment.NewLine;
        private int trimLength = int.MaxValue;

        /// <summary>
        /// Trims diff output by specified symbols count.
        /// </summary>
        /// <param name="limit">output length limit</param>
        /// <returns></returns>
        public CollectionsComparer<T> TrimOutputTo(int limit)
        {
            trimLength = limit;
            return this;
        }

        /// <summary>
        /// Adds custom bullet when appending items in diff output.
        /// </summary>
        /// <param name="bullet">bullet value</param>
        /// <returns></returns>
        public CollectionsComparer<T> UseItemsBulletsInOutput(string bullet)
        {
            delimiter = Environment.NewLine + bullet + " ";
            return this;
        }

        /// <summary>
        /// Gets detailed diff results as string
        /// </summary>
        public string Output { get; private set; }

        /// <summary>
        /// Checks whether actual collection is sequence equal to another.
        /// </summary>
        /// <param name="actual">actual items collection</param>
        /// <param name="expected">expected items collection</param>
        /// <returns>true if actual collection is sequence equal to another; otherwise - false</returns>
        public bool AreSequenceEqualTo(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            StringBuilder diff = new StringBuilder();

            int actualCount = actual.Count();
            int expectedCount = expected.Count();

            diff.Append(Diff);

            for (int i = 0; i < Math.Min(actualCount, expectedCount); i++)
            {
                if (!actual.ElementAt(i).Equals(expected.ElementAt(i)))
                {
                    diff.Append(delimiter).AppendFormat("[Index {0}]", i).AppendLine()
                        .Append("    Expected >> ").Append(expected.ElementAt(i)).AppendLine()
                        .Append("      Actual >> ").Append(actual.ElementAt(i));
                }
            }

            if (diff.Length == Diff.Length)
            {
                diff.Clear();
            }
            else
            {
                diff.AppendLine().AppendLine();
            }

            if (expectedCount > actualCount)
            {
                FillDiffWith(diff, Absent + $" (starting from index {actualCount})", expected.Skip(actualCount));
            }

            if (actualCount > expectedCount)
            {
                FillDiffWith(diff, NotExpected + $" (starting from index {expectedCount})", actual.Skip(expectedCount));
            }
            Truncate(diff);

            bool areSequenceEqual = diff.Length == 0;
            Output = areSequenceEqual ? "Collections are sequence equal" : diff.ToString();
            return areSequenceEqual;
        }

        /// <summary>
        /// Checks whether actual collection has the same items as expected collection ignoring order.
        /// </summary>
        /// <param name="actual">actual items collection</param>
        /// <param name="expected">expected items collection</param>
        /// <returns>true if actual collection has the same items as expected collection ignoring order; otherwise - false</returns>
        public bool AreTheSame(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            IEnumerable<T> intersection = IntersectWithDuplicates(actual, expected);

            List<T> absent = expected.ToList();
            List<T> redundant = actual.ToList();

            for (var i = 0; i < intersection.Count(); i++)
            {
                absent.Remove(intersection.ElementAt(i));
                redundant.Remove(intersection.ElementAt(i));
            }

            StringBuilder diff = new StringBuilder();

            if (absent.Any())
            {
                FillDiffWith(diff, Absent, absent);
                diff.AppendLine();
            }

            if (redundant.Any())
            {
                FillDiffWith(diff, NotExpected, redundant);
            }

            Truncate(diff);

            bool areTheSame = !absent.Any() && !redundant.Any();

            Output = areTheSame ? "Collections are the same" : diff.ToString();
            return areTheSame;
        }

        /// <summary>
        /// Checks whether actual collection contains all items from expected collection
        /// </summary>
        /// <param name="actual">actual items collection</param>
        /// <param name="expected">expected items collection</param>
        /// <returns>true if actual collection contains all items from expected collection; otherwise - false</returns>
        public bool Contains(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            IEnumerable<T> intersection = actual.Intersect(expected);

            if (intersection.Count() == expected.Count())
            {
                Output = "Collection contains all specified items";
                return true;
            }

            List<T> absent = expected.ToList();

            for (var i = 0; i < intersection.Count(); i++)
            {
                absent.Remove(intersection.ElementAt(i));
            }

            StringBuilder diff = new StringBuilder();
            FillDiffWith(diff, "Absent items >>", absent);
            Truncate(diff);

            Output = diff.ToString();

            return false;
        }

        /// <summary>
        /// Checks whether actual collection does not contain all items from expected collection.
        /// </summary>
        /// <param name="actual">actual items collection</param>
        /// <param name="expected">expected items collection</param>
        /// <returns>true if actual collection does not contain all items from expected collection; otherwise - false</returns>
        public bool NotContains(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            IEnumerable<T> intersection = actual.Intersect(expected);

            if (intersection.Any())
            {
                StringBuilder diff = new StringBuilder();
                FillDiffWith(diff, NotExpected, intersection);
                Truncate(diff);
                
                Output = diff.ToString();
                return false;
            }

            Output = "Collection does not contain all specified items";
            return true;
        }

        private void Truncate(StringBuilder data)
        {
            if (data.Length > trimLength)
            {
                data.Length = trimLength;
                data.AppendLine().AppendLine().AppendFormat("<< truncated by {0} symbols", trimLength);
            }
        }

        private void FillDiffWith(StringBuilder diff, string title, IEnumerable<T> items)
        {
            diff.Append(title);

            foreach(T item in items)
            {
                diff.Append(delimiter).Append(item);
            }
        }

        private static IEnumerable<T> IntersectWithDuplicates(IEnumerable<T> first, IEnumerable<T> second)
        {
            var dict = new Dictionary<T, int>();

            foreach (T item in second)
            {
                int hits;
                dict.TryGetValue(item, out hits);
                dict[item] = hits + 1;
            }

            foreach (T item in first)
            {
                int hits;
                dict.TryGetValue(item, out hits);
                if (hits > 0)
                {
                    yield return item;
                    dict[item] = hits - 1;
                }
            }
        }
    }
}
