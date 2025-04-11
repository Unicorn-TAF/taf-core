using System;
using System.Collections.Generic;
using Unicorn.Taf.Core.Utility;

namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    /// <summary>
    /// Matcher to check if object is deeply equal to expected one (recursively checks all public fields and properties).
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class DeepEqualToMatcher<T> : TypeSafeMatcher<T>
    {
        private readonly T _objectToCompare;
        private string[] pathsToIgnore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeepEqualToMatcher{T}"/> class for specified expected object.
        /// </summary>
        /// <param name="objectToCompare">expected object</param>
        public DeepEqualToMatcher(T objectToCompare)
        {
            _objectToCompare = objectToCompare;
        }

        /// <summary>
        /// Adds paths to fields/properties to ignore during comparison. 
        /// Field or property which path ends with one of ignore paths will be skipped during comparison.
        /// Example: SomeObject.SomeField.SomeInnerField
        /// </summary>
        /// <param name="pathsToIgnore">list of paths to fields/properties to be ignored</param>
        /// <returns></returns>
        public DeepEqualToMatcher<T> IgnoringPaths(params string[] pathsToIgnore)
        {
            this.pathsToIgnore = pathsToIgnore;
            return this;
        }
            
        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"is deeply equal to '{_objectToCompare}'";

        /// <summary>
        /// Checks if object is deeply equal to expected one (recursively checks all public fields and properties).
        /// </summary>
        /// <param name="actual">object under check</param>
        /// <returns>true - if actual object is deeply equal to expected one; otherwise - false</returns>
        public override bool Matches(T actual)
        {
            DeepObjectsComparer comparer = new DeepObjectsComparer()
                .UseItemsBulletsInOutput(">");

            if (pathsToIgnore != null)
            {
                comparer.IgnorePaths(pathsToIgnore);
            }

            List<string> diff = comparer.CompareObjects(actual, _objectToCompare);

            if (diff.Count == 0)
            {
                DescribeMismatch("objects are equal");
                return true;
            }
            else
            {
                DescribeMismatch(string.Join(Environment.NewLine, diff));
                return false;
            }
        }
    }
}
