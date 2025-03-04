using System;

namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="double"/> is close enough to other <see cref="double"/> with epsilon.
    /// </summary>
    public class DoubleIsCloseToMatcher : TypeSafeMatcher<double>
    {
        private readonly double _compareTo;
        private readonly double _epsilon;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleIsCloseToMatcher"/> class with specified object to compare.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <param name="epsilon">epsilon to compare with</param>
        public DoubleIsCloseToMatcher(double compareTo, double epsilon)
        {
            _compareTo = compareTo;
            _epsilon = epsilon;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Is close to {_compareTo} (with epsilon {_epsilon})";

        /// <summary>
        /// Checks if <see cref="double"/> is close enough to other considering epsilon.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if <see cref="double"/> is close to other (with epsilon); otherwise - false</returns>
        public override bool Matches(double actual)
        {
            DescribeMismatch(actual.ToString());
            return Math.Abs(actual - _compareTo) < _epsilon;
        }
    }
}
