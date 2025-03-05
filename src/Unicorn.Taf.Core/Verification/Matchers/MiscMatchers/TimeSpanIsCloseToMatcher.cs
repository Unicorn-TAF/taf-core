using System;

namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="TimeSpan"/> is close enough to other <see cref="TimeSpan"/> with epsilon.
    /// </summary>
    public class TimeSpanIsCloseToMatcher : TypeSafeMatcher<TimeSpan>
    {
        private readonly TimeSpan _compareTo;
        private readonly TimeSpan _epsilon;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanIsCloseToMatcher"/> class with specified object to compare.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <param name="epsilon">epsilon to compare with</param>
        public TimeSpanIsCloseToMatcher(TimeSpan compareTo, TimeSpan epsilon)
        {
            _compareTo = compareTo;
            _epsilon = epsilon;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"is close to {_compareTo} (with epsilon {_epsilon})";

        /// <summary>
        /// Checks if <see cref="TimeSpan"/> is close enough to other considering epsilon.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if <see cref="TimeSpan"/> is close to other (with epsilon); otherwise - false</returns>
        public override bool Matches(TimeSpan actual)
        {
            DescribeMismatch(actual.ToString());
            return (actual - _compareTo).Duration() < _epsilon;
        }
    }
}
