// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to assign an ID to test method. Test ID is some unique value which could be used 
    /// to identify some perticular test and to tie it with a test case in test management system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestCaseIdAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseIdAttribute"/> class with specified test case ID.
        /// </summary>
        /// <param name="id">test case ID</param>
        public TestCaseIdAttribute(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets test case ID.
        /// </summary>
        public string Id { get; }
    }
}
