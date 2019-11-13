﻿using System;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject.By
{
    /// <summary>
    /// Provides with ability to specify search condition by 'Id' for UI control PageObject
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ByIdAttribute : FindAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByNameAttribute"/> class with specified locator.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        public ByIdAttribute(string locator) : base(Using.Id, locator)
        {
        }
    }
}