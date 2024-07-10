﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    /// <summary>
    /// Matcher to negotiate action of another matcher
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class TypeSafeNotMatcher<T> : TypeSafeMatcher<T>
    {
        private readonly TypeSafeMatcher<T> _matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSafeNotMatcher{T}"/> class for specified matcher.
        /// </summary>
        /// <param name="matcher">instance of matcher with specified check</param>
        public TypeSafeNotMatcher(TypeSafeMatcher<T> matcher)
        {
            matcher.Reverse = true;
            _matcher = matcher;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Not {_matcher.CheckDescription}";

        /// <summary>
        /// Negates main matcher check.
        /// </summary>
        /// <param name="actual">object under check</param>
        /// <returns>true - if main matching was failed; otherwise - false</returns>
        public override bool Matches(T actual)
        {
            if (_matcher.Matches(actual))
            {
                Output.Append(_matcher.Output);
                return false;
            }

            return true;
        }
    }
}
