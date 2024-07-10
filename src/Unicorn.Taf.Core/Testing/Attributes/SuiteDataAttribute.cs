// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified methods as suite data providers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SuiteDataAttribute : Attribute
    {
    }
}
