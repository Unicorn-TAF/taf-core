// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified suite classes as parameterized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ParameterizedAttribute : Attribute
    {
    }
}
