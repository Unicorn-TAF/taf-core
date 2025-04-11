using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark test or suite as disabled and optionally to specify disable runtime condition.
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
        /// Initializes a new instance of the <see cref="DisabledAttribute"/> class with specified reason 
        /// and path to disable condition property. The property should be in the same class and be public static.
        /// Is property value is true, then target suite or test will be disabled, otherwise it will be executed.
        /// </summary>
        /// <param name="reason">disabling reason</param>
        /// <param name="conditionProperty">property name containing disable condition</param>
        public DisabledAttribute(string reason, string conditionProperty)
        {
            Reason = reason;
            ConditionProperty = conditionProperty;
        }

        /// <summary>
        /// Gets disabling reason.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Property name (in current class) containing disable condition, should be public static. 
        /// If it's true, then target suite or test will not be executed, otherwise it will be run.
        /// </summary>
        public string ConditionProperty { get; }
    }
}
