﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark test or suite as disabled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DisabledAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisabledAttribute"/> class with specified reason.
        /// </summary>
        /// <param name="reason">disabling reason</param>
        public DisabledAttribute(string reason)
        {
            Reason = reason;
        }

        /// <summary>
        /// Gets disabling reason.
        /// </summary>
        public string Reason { get; }
    }
}
