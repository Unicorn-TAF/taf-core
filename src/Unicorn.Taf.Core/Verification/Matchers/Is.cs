﻿using System;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;
using Unicorn.Taf.Core.Verification.Matchers.CoreMatchers;
using Unicorn.Taf.Core.Verification.Matchers.MiscMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Entry point for core matchers.
    /// </summary>
    public static class Is
    {
        /// <summary>
        /// Matcher to check if actual object is equal to expected one.
        /// </summary>
        /// <typeparam name="T">check items type</typeparam>
        /// <param name="objectToCompare">expected item to check equality</param>
        /// <returns><see cref="EqualToMatcher{T}"/> instance</returns>
        public static EqualToMatcher<T> EqualTo<T>(T objectToCompare) =>
            new EqualToMatcher<T>(objectToCompare);

        /// <summary>
        /// Matcher to check if actual object is deeply equal to expected one (recursively checks all public fields and properties).
        /// </summary>
        /// <typeparam name="T">check items type</typeparam>
        /// <param name="objectToCompare">expected item to check equality</param>
        /// <returns><see cref="EqualToMatcher{T}"/> instance</returns>
        public static DeepEqualToMatcher<T> DeeplyEqualTo<T>(T objectToCompare) =>
            new DeepEqualToMatcher<T>(objectToCompare);

        /// <summary>
        /// Matcher to check if object is null.
        /// </summary>
        /// <returns><see cref="NullMatcher"/> instance</returns>
        public static NullMatcher Null() =>
            new NullMatcher();

        /// <summary>
        /// Matcher to check if actual object is of expected type.
        /// </summary>
        /// <param name="expectedType">expected object type</param>
        /// <returns><see cref="OfTypeMatcher"/> instance</returns>
        public static OfTypeMatcher OfType(Type expectedType) =>
            new OfTypeMatcher(expectedType);

        /// <summary>
        /// Matcher to check if <see cref="IComparable"/> is greater than other <see cref="IComparable"/>.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <returns><see cref="IsGreaterThanMatcher"/> matcher instance</returns>
        public static IsGreaterThanMatcher IsGreaterThan(IComparable compareTo) =>
            new IsGreaterThanMatcher(compareTo);

        /// <summary>
        /// Matcher to check if <see cref="IComparable"/> is greater than or equal to other <see cref="IComparable"/>.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <returns><see cref="IsGreaterThanOrEqualToMatcher"/> matcher instance</returns>
        public static IsGreaterThanOrEqualToMatcher IsGreaterThanOrEqualTo(IComparable compareTo) =>
            new IsGreaterThanOrEqualToMatcher(compareTo);

        /// <summary>
        /// Matcher to check if <see cref="IComparable"/> is less than other <see cref="IComparable"/>.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <returns><see cref="IsLessThanMatcher"/> matcher instance</returns>
        public static IsLessThanMatcher IsLessThan(IComparable compareTo) =>
            new IsLessThanMatcher(compareTo);

        /// <summary>
        /// Matcher to check if <see cref="IComparable"/> is less than or equal to other <see cref="IComparable"/>.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <returns><see cref="IsLessThanOrEqualToMatcher"/> matcher instance</returns>
        public static IsLessThanOrEqualToMatcher IsLessThanOrEqualTo(IComparable compareTo) =>
            new IsLessThanOrEqualToMatcher(compareTo);

        /// <summary>
        /// Matcher to check if <see cref="double"/> is close enough to other <see cref="double"/> with epsilon.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <param name="epsilon">epsilon to consider</param>
        /// <returns><see cref="DoubleIsCloseToMatcher"/> matcher instance</returns>
        public static DoubleIsCloseToMatcher CloseTo(double compareTo, double epsilon) =>
            new DoubleIsCloseToMatcher(compareTo, epsilon);

        /// <summary>
        /// Matcher to check if <see cref="TimeSpan"/> is close enough to other <see cref="TimeSpan"/> with epsilon.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <param name="epsilon">epsilon to consider</param>
        /// <returns><see cref="TimeSpanIsCloseToMatcher"/> matcher instance</returns>
        public static TimeSpanIsCloseToMatcher CloseTo(TimeSpan compareTo, TimeSpan epsilon) =>
            new TimeSpanIsCloseToMatcher(compareTo, epsilon);

        /// <summary>
        /// Matcher to check if <see cref="DateTime"/> is close enough to other <see cref="DateTime"/> with epsilon.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        /// <param name="epsilon">epsilon to consider</param>
        /// <returns><see cref="DateTimeIsCloseToMatcher"/> matcher instance</returns>
        public static DateTimeIsCloseToMatcher CloseTo(DateTime compareTo, TimeSpan epsilon) =>
            new DateTimeIsCloseToMatcher(compareTo, epsilon);

        /// <summary>
        /// Matcher to negotiate action of another matcher.
        /// </summary>
        /// <param name="matcher">instance of matcher with specified check</param>
        /// <returns><see cref="NotMatcher"/> instance</returns>
        public static NotMatcher Not(TypeUnsafeMatcher matcher) =>
            new NotMatcher(matcher);

        /// <summary>
        /// Matcher to negotiate action of another matcher.
        /// </summary>
        /// <typeparam name="T">check items type</typeparam>
        /// <param name="matcher">instance of matcher with specified check</param>
        /// <returns><see cref="TypeSafeNotMatcher{T}"/> instance</returns>
        public static TypeSafeNotMatcher<T> Not<T>(TypeSafeMatcher<T> matcher) =>
            new TypeSafeNotMatcher<T>(matcher);

        /// <summary>
        /// Matcher to negotiate action of specified collection matcher.
        /// </summary>
        /// <typeparam name="T">check items type</typeparam>
        /// <param name="matcher">instance of collection matcher with specified check</param>
        /// <returns><see cref="TypeSafeCollectionNotMatcher{T}"/> instance</returns>
        public static TypeSafeCollectionNotMatcher<T> Not<T>(TypeSafeCollectionMatcher<T> matcher) =>
            new TypeSafeCollectionNotMatcher<T>(matcher);
    }
}
