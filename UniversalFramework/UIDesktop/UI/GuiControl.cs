﻿using System;
using System.Diagnostics;
using System.Windows.Automation;
using Unicorn.UICore.UI;
using Unicorn.UICore.UIProperties;
using Unicorn.UIDesktop.Driver;

namespace Unicorn.UIDesktop.UI
{
    public abstract class GuiControl : GuiSearchContext, IControl
    {
        public virtual string ClassName { get { return null; } }
        public abstract ControlType Type { get; }

        public virtual AutomationElement Instance
        {
            get { return SearchContext; }
        }


        public GuiControl() { }

        public GuiControl(AutomationElement instance)
            : base()
        {
            SearchContext = instance;
        }


        public string GetAttribute(string attribute)
        {
            throw new NotImplementedException();
        }

        public bool Enabled
        {
            get
            {
                return (bool)Instance.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty);
            }
        }

        public UIPoint Location
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Text
        {
            get
            {
                var name = (string)Instance.GetCurrentPropertyValue(AutomationElement.NameProperty);
                return !string.IsNullOrEmpty(name) ? name : (string)Instance.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty);
            }
        }

        public UISize Size
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Visible
        {
            get
            {
                bool isVisible;
                try
                {
                    isVisible = !Instance.Current.IsOffscreen;
                }
                catch (ElementNotAvailableException)
                {
                    isVisible = false;
                }
                return isVisible;
            }
        }

        public void CheckAttributeContains(string attribute, string expectedValue)
        {
            throw new NotImplementedException();
        }

        public void CheckAttributeDoeNotContain(string attribute, string expectedValue)
        {
            throw new NotImplementedException();
        }

        public void CheckAttributeEquals(string attribute, string expectedValue)
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            object pattern = null;

            if (Instance.TryGetCurrentPattern(InvokePattern.Pattern, out pattern))
                ((InvokePattern)pattern).Invoke();
            else
                ((TogglePattern)Instance.GetCurrentPattern(TogglePattern.Pattern)).Toggle();
        }

        public void WaitForAttributeValue(string attribute, string value, bool contains = true)
        {
            throw new NotImplementedException();
        }

        public void WaitForEnabled()
        {
            int timeout = 10000;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while ((!Enabled || !Visible) && timer.ElapsedMilliseconds < timeout) ;
            if (timer.ElapsedMilliseconds >= timeout)
                throw new ElementInvalidStateException(string.Format("Control '{0}' is in illegal state. Visible: {1}, Enabled: {2}", Text, Visible, Enabled));
        }

        protected T GetPattern<T>() where T : BasePattern
        {
            var pattern = (AutomationPattern)typeof(T).GetField("Pattern").GetValue(null);
            object patternObject;
            Instance.TryGetCurrentPattern(pattern, out patternObject);
            return (T)patternObject;
        }
    }
}
