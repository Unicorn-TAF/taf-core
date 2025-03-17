using System;
using System.Text;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Useful utilities for matchers and comparison
    /// </summary>
    public class MatchersUtils
    {
        private const string More = " . . . ";

        /// <summary>
        /// Gets enhanced result of strings comparison pointing on first exact diff place.
        /// </summary>
        /// <param name="expected">expected string</param>
        /// <param name="actual">actual string</param>
        /// <returns>60 chars window around first diff with pointer to the diff 
        /// or 'Strings are identical' if strings are identical</returns>
        public static string GetStringsDiff(string expected, string actual)
        {
            string expectedHeader = "Expected >> ";
            string actualHeader = "  Actual >> ";

            StringBuilder diff = new StringBuilder();

            int diffIndex = FindFirstDifference(expected, actual);

            if (diffIndex == -1)
            {
                return "Strings are identical";
            }

            int takeFrom = diffIndex - 30;
            int takeTill = diffIndex + 31;

            // Calculate windows boundaries
            int start = Math.Max(0, takeFrom);
            int actualEnd = Math.Min(actual.Length, takeTill);
            int expectedEnd = Math.Min(expected.Length, takeTill);

            if (start > 0)
            {
                expectedHeader += More;
                actualHeader += More;
            }

            diff.Append(expectedHeader).Append(expected.Substring(start, expectedEnd - start));
            diff.AppendLine(takeTill < expected.Length ? More : "");

            diff.Append(actualHeader).Append(actual.Substring(start, actualEnd - start));
            diff.AppendLine(takeTill < actual.Length ? More : "");

            // Print marker line
            for (int i = 0; i < diffIndex - start + expectedHeader.Length; i++)
            {
                diff.Append('-');
            }

            diff.Append("^ at index ").Append(diffIndex);

            return diff.ToString();
        }

        private static int FindFirstDifference(string str1, string str2)
        {
            int length = Math.Min(str1.Length, str2.Length);

            for (int i = 0; i < length; i++)
            {
                if (str1[i] != str2[i])
                {
                    return i;
                }
            }

            // If one string is longer than the other
            return str1.Length == str2.Length ? -1 : Math.Min(str1.Length, str2.Length);
        }
    }
}
