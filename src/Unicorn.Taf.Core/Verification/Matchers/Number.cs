﻿using Unicorn.Taf.Core.Verification.Matchers.MiscMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Entry point for number matchers.
    /// </summary>
    public static class Number
    {
        /// <summary>
        /// Matcher to check if number is even.
        /// </summary>
        /// <returns><see cref="IsEvenMatcher"/> instance</returns>
        public static IsEvenMatcher IsEven() =>
            new IsEvenMatcher();
    }
}
