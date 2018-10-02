﻿using System;

namespace Unicorn.UI.Core.PageObject
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class NameAttribute : Attribute
    {
        public NameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name
        {
            get;
            
            protected set;
        }
    }
}
