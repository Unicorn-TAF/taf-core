﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System.Collections.Generic;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Base matcher for type specific collection matchers implementations.
    /// </summary>
    /// <typeparam name="T">type of object under assertion</typeparam>
    public abstract class TypeSafeCollectionMatcher<T> : BaseMatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSafeCollectionMatcher{T}"/> class.
        /// </summary>
        protected TypeSafeCollectionMatcher() : base()
        {
        }

        /// <summary>
        /// Checks if target objects collection matches condition corresponding to specific matcher implementations.
        /// </summary>
        /// <param name="actual">objects collection under assertion</param>
        /// <returns>true - if objects collection matches specific condition; otherwise - false</returns>
        public abstract bool Matches(IEnumerable<T> actual);

        /// <summary>
        /// Gets truncated to 200 chars version of collection ToString
        /// </summary>
        /// <param name="collection">collection instance</param>
        /// <param name="trimLength">max number of symbols for output</param>
        /// <returns>collection description string</returns>
        protected string DescribeCollection(IEnumerable<T> collection, int trimLength)
        {
            string itemsList = string.Join(", ", collection);

            if (itemsList.Length > trimLength)
            {
                itemsList = itemsList.Substring(0, trimLength) + " etc . . .";
            }

            return itemsList;
        }
    }
}
