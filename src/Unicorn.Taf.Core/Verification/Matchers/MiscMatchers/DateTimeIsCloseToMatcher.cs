using System;

namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="DateTime"/> is close enough to other <see cref="DateTime"/> with epsilon.
    /// </summary>
    public class DateTimeIsCloseToMatcher : TypeSafeMatcher<DateTime>
    {
        private readonly DateTime _compareTo;
        private readonly TimeSpan _epsilon;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeIsCloseToMatcher"/> class with specified object to compare.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <param name="epsilon">epsilon to compare with</param>
        public DateTimeIsCloseToMatcher(DateTime compareTo, TimeSpan epsilon)
        {
            _compareTo = compareTo;
            _epsilon = epsilon;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Is close to {_compareTo} (with epsilon {_epsilon})";

        /// <summary>
        /// Checks if <see cref="DateTime"/> is close enough to other considering epsilon.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if <see cref="DateTime"/> is close to other (with epsilon); otherwise - false</returns>
        public override bool Matches(DateTime actual)
        {
            DescribeMismatch(actual.ToString());
            return (actual - _compareTo).Duration() < _epsilon;
        }
    }
}
