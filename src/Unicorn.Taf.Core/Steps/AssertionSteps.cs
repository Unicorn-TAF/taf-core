using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Steps
{
    /// <summary>
    /// From the box implementation of steps for different kind of assertions:<para/>
    /// - typified/non-typified object checks<para/>
    /// - typified/non-typified objects collection checks<para/>
    /// - chain assertions on typified/non-typified objects<para/>
    /// - chain assertions on typified/non-typified objects collection<para/>
    /// </summary>
    public class AssertionSteps
    {
        private ChainAssert _chaninAssert = null;

        /// <summary>
        /// Step which performs assertion on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// and with specified message on fail
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="message">message thrown on fail</param>
        [Step("Assert that {0} {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AssertThat<T>(T actual, TypeSafeMatcher<T> matcher, string message) =>
            StepsUtilities.WrapStep(() => Assert.That(actual, matcher, message), actual, matcher, message);

        /// <summary>
        /// Step which performs assertion on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        [Step("Assert that {0} {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AssertThat<T>(T actual, TypeSafeMatcher<T> matcher) =>
            StepsUtilities.WrapStep(() => Assert.That(actual, matcher), actual, matcher);

        /// <summary>
        /// Step which performs assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// and with specified message on fail
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="message">message thrown on fail</param>
        public void AssertThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher, string message) =>
            ReportedCollectionAssertThat(typeof(T).Name, matcher, actual, message);

        /// <summary>
        /// Step which performs assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        public void AssertThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher) =>
            ReportedCollectionAssertThat(typeof(T).Name, matcher, actual);

        /// <summary>
        /// Perform assertion on object of any type using matcher 
        /// which is not specified by type and with specified message on fail
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <param name="message">message thrown on fail</param>
        [Step("Assert that {0} {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AssertThat(object actual, TypeUnsafeMatcher matcher, string message) =>
            StepsUtilities.WrapStep(() => Assert.That(actual, matcher, message), actual, matcher, message);

        /// <summary>
        /// Perform assertion on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        [Step("Assert that {0} {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AssertThat(object actual, TypeUnsafeMatcher matcher) =>
            StepsUtilities.WrapStep(() => Assert.That(actual, matcher), actual, matcher);

        /// <summary>
        /// Perform assertion on action to check for expected exception is thrown.
        /// </summary>
        /// <param name="action">action to perform</param>
        /// <param name="message">message thrown on fail</param>
        /// <typeparam name="T">Expected exception type</typeparam>
        [Step("{1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AssertThrows<T>(Action action, string message) =>
            StepsUtilities.WrapStep(() => Assert.Throws<T>(action, message), action, message);

        /// <summary>
        /// Perform assertion on action to check for expected exception is thrown.
        /// </summary>
        /// <param name="action">action to perform</param>
        /// <typeparam name="T">Expected exception type</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AssertThrows<T>(Action action) =>
            ReportableAssertThrows<T>(action, typeof(T));

        /// <summary>
        /// Initializes assertions chain.
        /// </summary>
        /// <returns>current assertion steps instance</returns>
        /// <param name="description">description for following assertions</param>
        [Step("{0}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public AssertionSteps StartAssertionsChain(string description) =>
            StepsUtilities.WrapStep(() =>
            {
                _chaninAssert = new ChainAssert(description);
                return this;
            }, description);

        /// <summary>
        /// Initializes assertions chain.
        /// </summary>
        /// <returns>current assertion steps instance</returns>
        public AssertionSteps StartAssertionsChain() => 
            StartAssertionsChain("Assertions chain");

        /// <summary>
        /// Step which performs soft check on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <returns>current assertion steps instance</returns>
        [Step("Verify that {0} {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public AssertionSteps VerifyThat<T>(T actual, TypeSafeMatcher<T> matcher) =>
            StepsUtilities.WrapStep(() =>
            {
                if (_chaninAssert == null)
                {
                    _chaninAssert = new ChainAssert();
                }

                _chaninAssert.That(actual, matcher);
                return this;
            }, actual, matcher);

        /// <summary>
        /// Step which performs soft check on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="message">error message displayed on fail</param>
        /// <returns>current assertion steps instance</returns>
        [Step("Verify that {0} {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public AssertionSteps VerifyThat<T>(T actual, TypeSafeMatcher<T> matcher, string message) =>
            StepsUtilities.WrapStep(() =>
            {
                if (_chaninAssert == null)
                {
                    _chaninAssert = new ChainAssert();
                }

                _chaninAssert.That(actual, matcher, message);
                return this;
            }, actual, matcher, message);

        /// <summary>
        /// Step which performs soft check on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <returns>current assertion steps instance</returns>
        [Step("Verify that {0} {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public AssertionSteps VerifyThat(object actual, TypeUnsafeMatcher matcher) =>
            StepsUtilities.WrapStep(() =>
            {
                if (_chaninAssert == null)
                {
                    _chaninAssert = new ChainAssert();
                }

                _chaninAssert.That(actual, matcher);
                return this;
            }, actual, matcher);

        /// <summary>
        /// Step which performs soft check on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <param name="message">error message displayed on fail</param>
        /// <returns>current assertion steps instance</returns>
        [Step("Verify that {0} {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public AssertionSteps VerifyThat(object actual, TypeUnsafeMatcher matcher, string message) =>
            StepsUtilities.WrapStep(() =>
            {
                if (_chaninAssert == null)
                {
                    _chaninAssert = new ChainAssert();
                }

                _chaninAssert.That(actual, matcher, message);
                return this;
            }, actual, matcher, message);

        /// <summary>
        /// Step which performs assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <returns>current assertion steps instance</returns>
        public AssertionSteps VerifyThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher)
        {
            if (_chaninAssert == null)
            {
                _chaninAssert = new ChainAssert();
            }

            ReportedCollectionVerifyThat(typeof(T).Name, matcher, actual);
            return this;
        }

        /// <summary>
        /// Step which performs assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="message">error message displayed on fail</param>
        /// <returns>current assertion steps instance</returns>
        public AssertionSteps VerifyThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher, string message)
        {
            if (_chaninAssert == null)
            {
                _chaninAssert = new ChainAssert();
            }

            ReportedCollectionVerifyThat(typeof(T).Name, matcher, actual, message);
            return this;
        }

        /// <summary>
        /// Step which performs assertion on chain of soft asserts performed after chain initialization.
        /// </summary>
        /// <exception cref="InvalidOperationException">is thrown if chain assert was not initialized</exception>
        [Step("Assert verifications chain")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AssertChain() =>
            StepsUtilities.WrapStep(() => 
            {
                if (_chaninAssert == null)
                {
                    throw new InvalidOperationException(
                        "There were no any verifications made. Please check scenario.");
                }

                try
                {
                    _chaninAssert.AssertChain();
                }
                finally
                {
                    _chaninAssert = null;
                }
            });

        [Step("Assert that collection of <{0}> {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ReportedCollectionAssertThat<T>(
            string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual, string message) =>
            StepsUtilities.WrapStep(() => Assert.That(actual, matcher, message), elementType, matcher, actual, message);

        [Step("Assert that collection of <{0}> {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ReportedCollectionAssertThat<T>(
            string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual) =>
            StepsUtilities.WrapStep(() => Assert.That(actual, matcher), elementType, matcher, actual);

        [Step("Assert that collection of <{0}> {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ReportedCollectionVerifyThat<T>(
            string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual) =>
            StepsUtilities.WrapStep(() => _chaninAssert.That(actual, matcher), elementType, matcher, actual);

        [Step("Assert that collection of <{0}> {1}")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ReportedCollectionVerifyThat<T>(
            string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual, string message) =>
            StepsUtilities.WrapStep(() => _chaninAssert.That(actual, matcher, message), elementType, matcher, actual, message);

        [Step("Assert that action throws {1} exception")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ReportableAssertThrows<T>(Action action, Type type) =>
            StepsUtilities.WrapStep(() => Assert.Throws<T>(action), action, type);
    }
}
