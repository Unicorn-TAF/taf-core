﻿using System;
using System.Collections.Generic;
using System.Text;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification
{
    /// <summary>
    /// Provides mechanism of assertions based on Matchers
    /// </summary>
    public static class Assert
    {
        private const string But = "But: ";
        private const string Expected = "Expected: ";
        private const string DefaultFailMessage = "Assertion failed.";

        /// <summary>
        /// Perform assertion on condition if it's true
        /// </summary>
        /// <param name="condition">condition to check if it's true</param>
        /// <param name="message">message thrown on fail</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                StringBuilder error = new StringBuilder()
                    .AppendLine(message)
                    .Append(Expected).AppendLine("true")
                    .Append(But).AppendLine("was false");

                throw new AssertionException(error.ToString());
            }
        }

        /// <summary>
        /// Perform assertion on condition if it's true
        /// </summary>
        /// <param name="condition">condition to check if it's true</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void IsTrue(bool condition) => IsTrue(condition, DefaultFailMessage);

        /// <summary>
        /// Perform assertion on condition if it's false
        /// </summary>
        /// <param name="condition">condition to check if it's false</param>
        /// <param name="message">message thrown on fail</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                StringBuilder error = new StringBuilder()
                    .AppendLine(message)
                    .Append(Expected).AppendLine("false")
                    .Append(But).AppendLine("was true");

                throw new AssertionException(error.ToString());
            }
        }

        /// <summary>
        /// Perform assertion on condition if it's false
        /// </summary>
        /// <param name="condition">condition to check if it's false</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void IsFalse(bool condition) => IsFalse(condition, DefaultFailMessage);

        /// <summary>
        /// Perform assertion on object of any type using matcher 
        /// which is not specified by type and with specified message on fail
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <param name="message">high level message thrown on fail (matcher output is added automatically)</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void That(object actual, TypeUnsafeMatcher matcher, string message)
        {
            matcher.Output
                .Append(Expected)
                .Append(matcher.CheckDescription)
                .AppendLine()
                .Append(But);

            if (!matcher.Matches(actual))
            {
                var errorText = matcher.Output.ToString();

                if (!string.IsNullOrEmpty(message))
                {
                    errorText = message + Environment.NewLine + errorText;
                }

                throw new AssertionException(errorText);
            }
        }

        /// <summary>
        /// Perform assertion on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void That(object actual, TypeUnsafeMatcher matcher) => That(actual, matcher, DefaultFailMessage);

        /// <summary>
        /// Perform assertion on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// and with specified message on fail
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="message">high level message thrown on fail (matcher output is added automatically)</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void That<T>(T actual, TypeSafeMatcher<T> matcher, string message)
        {
            matcher.Output
                .Append(Expected)
                .Append(matcher.CheckDescription)
                .AppendLine()
                .Append(But);

            if (!matcher.Matches(actual))
            {
                var errorText = matcher.Output.ToString();

                if (!string.IsNullOrEmpty(message))
                {
                    errorText = message + Environment.NewLine + errorText;
                }

                throw new AssertionException(errorText);
            }
        }

        /// <summary>
        /// Perform assertion on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void That<T>(T actual, TypeSafeMatcher<T> matcher) => That(actual, matcher, DefaultFailMessage);

        /// <summary>
        /// Perform assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// and with specified message on fail
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="message">high level message thrown on fail (matcher output is added automatically)</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void That<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher, string message)
        {
            matcher.Output
                .Append(Expected)
                .Append(matcher.CheckDescription)
                .AppendLine()
                .Append(But);

            if (!matcher.Matches(actual))
            {
                var errorText = matcher.Output.ToString();

                if (!string.IsNullOrEmpty(message))
                {
                    errorText = message + Environment.NewLine + errorText;
                }

                throw new AssertionException(errorText);
            }
        }

        /// <summary>
        /// Perform assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void That<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher) =>
            That(actual, matcher, DefaultFailMessage);

        /// <summary>
        /// Perform assertion on expected exception thrown while executing desired action.
        /// </summary>
        /// <typeparam name="T">expected exception type</typeparam>
        /// <param name="action"><see cref="Action"/> to be executed</param>
        /// <param name="message">high level message thrown on fail</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void Throws<T>(Action action, string message)
        {
            string actual;

            try
            {
                action();
                actual = "was no any exception thrown";
            }
            catch (Exception ex)
            {
                if (ex.GetType().Equals(typeof(T)))
                {
                    return;
                }
                else
                {
                    actual = "was " + ex.GetType().FullName;
                }
            }

            StringBuilder error = new StringBuilder()
                .AppendLine(message)
                .Append(Expected).AppendLine(typeof(T).FullName)
                .Append(But).AppendLine(actual);

            Fail(error.ToString());
        }

        /// <summary>
        /// Perform assertion on expected exception thrown while executing desired action.
        /// </summary>
        /// <typeparam name="T">expected exception type</typeparam>
        /// <param name="action"><see cref="Action"/> to be executed</param>
        /// <exception cref="AssertionException">is thrown when assertion was failed</exception>
        public static void Throws<T>(Action action) =>
            Throws<T>(action, DefaultFailMessage);

        /// <summary>
        /// Throws <see cref="AssertionException"/> with specified message.
        /// </summary>
        /// <param name="message">error message to display</param>
        public static void Fail(string message) =>
            throw new AssertionException(message);
    }
}
