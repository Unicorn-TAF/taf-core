﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;
using Unicorn.Taf.Core.Verification.Matchers.MiscMatchers;

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

        /// <summary>
        /// Matcher to check if number is positive.
        /// </summary>
        /// <returns><see cref="IsPositiveMatcher"/> instance</returns>
        [Obsolete("Please use IsGreaterThanMatcher instead")]
        public static IsPositiveMatcher IsPositive() =>
            new IsPositiveMatcher();
    }
}
