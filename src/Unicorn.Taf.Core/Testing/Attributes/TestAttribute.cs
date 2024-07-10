// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified methods as tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttribute"/> class without title.
        /// </summary>
        public TestAttribute() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttribute"/> class with specified title.
        /// </summary>
        /// <param name="title">test title</param>
        public TestAttribute(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Gets test title.
        /// </summary>
        public string Title { get; }
    }
}
