using System;

namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    /// <summary>
    /// Matcher to check if object is of expected type. 
    /// </summary>
    public class OfTypeMatcher : TypeUnsafeMatcher
    {
        private readonly Type _expectedType;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfTypeMatcher"/> class with expected type.
        /// </summary>
        public OfTypeMatcher(Type expectedType) : base()
        {
            _expectedType = expectedType;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"is of {_expectedType.FullName} type";

        /// <summary>
        /// Checks if object is of expected type.
        /// </summary>
        /// <param name="actual">object under check</param>
        /// <returns>true - if object is of expected type; otherwise - false</returns>
        public override bool Matches(object actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }
            
            if (actual.GetType() != _expectedType)
            {
                DescribeMismatch(actual.GetType().FullName);
                return false;
            }

            return true;
        }
    }
}
