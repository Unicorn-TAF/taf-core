﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to make test parameterized by data sets.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestDataAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataAttribute"/> class with specified test data provider method.
        /// </summary>
        /// <param name="method">test data provider method name</param>
        public TestDataAttribute(string method)
        {
            Method = method;
        }

        /// <summary>
        /// Gets test data provider method name.
        /// </summary>
        public string Method { get; }
    }
}
