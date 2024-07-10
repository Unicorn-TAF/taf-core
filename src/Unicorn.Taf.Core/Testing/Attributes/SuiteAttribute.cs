// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified classes as test suites.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SuiteAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteAttribute"/> class with specified name.
        /// </summary>
        /// <param name="name">test suite name</param>
        public SuiteAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets test suite name.
        /// </summary>
        public string Name { get; }
    }
}
