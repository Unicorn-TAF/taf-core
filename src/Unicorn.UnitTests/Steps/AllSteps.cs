using System;
using System.Diagnostics;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Unicorn.UnitTests.Steps
{
    public class AllSteps
    {
        private readonly Lazy<AssertionSteps> assertion = new Lazy<AssertionSteps>();

        public AssertionSteps Assertion => assertion.Value;

        [Step("NumFormat {0:F1}")]
        public void StepWithFormatting(double number)
        {
            MethodBase methodBase = new StackFrame(0).GetMethod();
            TafEvents.CallOnStepStartEvent(methodBase, number);
            TafEvents.CallOnStepFinishEvent(methodBase, number);

        }
    }
}
