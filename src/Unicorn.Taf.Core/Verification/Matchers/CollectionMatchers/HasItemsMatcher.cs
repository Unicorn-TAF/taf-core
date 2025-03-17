using System;
using System.Collections.Generic;
using Unicorn.Taf.Core.Utility;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection has specified items array. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class HasItemsMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly IEnumerable<T> _expectedObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasItemsMatcher{T}"/> class with specified expected objects.
        /// </summary>
        /// <param name="expectedObjects">expected objects</param>
        public HasItemsMatcher(IEnumerable<T> expectedObjects)
        {
            _expectedObjects = expectedObjects;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription =>
            "has items: " + DescribeCollection(_expectedObjects, 200);

        /// <summary>
        /// Checks if collection contains specified items.
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if collection contains specific items; otherwise - false</returns>
        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            CollectionsComparer<T> comparer = new CollectionsComparer<T>()
                .TrimOutputTo(1000)
                .UseItemsBulletsInOutput(">");

            bool result;

            if (Reverse)
            {
                result = !comparer.NotContains(actual, _expectedObjects);
                DescribeMismatch(Environment.NewLine + comparer.Output);
            }
            else
            {
                result = comparer.Contains(actual, _expectedObjects);
                DescribeMismatch(Environment.NewLine + comparer.Output);
            }

            return result;
        }
    }
}
