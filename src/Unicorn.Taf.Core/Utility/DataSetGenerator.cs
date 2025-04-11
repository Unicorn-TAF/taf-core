using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Utility
{
    /// <summary>
    /// Syntactically convenient utility to generate test data sets lists based on some common cases.
    /// </summary>
    public static class DataSetGenerator
    {
        /// <summary>
        /// Generates list of <see cref="DataSet"/> based on single parameter values list. 
        /// DataSet name will be equal to its value.
        /// </summary>
        /// <typeparam name="T">items type (any)</typeparam>
        /// <param name="items">list of items for data sets generation</param>
        /// <returns>list of <see cref="DataSet"/></returns>
        public static List<DataSet> FromItems<T>(params T[] items) =>
            items.Select(di => new DataSet(di.ToString(), di)).ToList();

        /// <summary>
        /// Generates list of <see cref="DataSet"/> based on all possible combinations of items between two enumerables.
        /// DataSet name is generated as: [item1 x item2]
        /// </summary>
        /// <typeparam name="T1">items type (any)</typeparam>
        /// <typeparam name="T2">items type (any)</typeparam>
        /// <param name="items">items to combine</param>
        /// <param name="itemsToCombine">another items to combine</param>
        /// <returns>list of <see cref="DataSet"/></returns>
        public static List<DataSet> CombinationOf<T1, T2>(IEnumerable<T1> items, IEnumerable<T2> itemsToCombine)
        {
            List<DataSet> testData = new List<DataSet>();

            foreach (T1 item in items)
            {
                foreach (T2 itemToCombine in itemsToCombine)
                {
                    testData.Add(new DataSet($"[{item} x {itemToCombine}]", item, itemToCombine));
                }
            }

            return testData;
        }

        /// <summary>
        /// Generates list of <see cref="DataSet"/> based on all possible combinations of items between two arrays.
        /// DataSet name is generated as: [item1 x item2]
        /// </summary>
        /// <typeparam name="T1">items type (any)</typeparam>
        /// <typeparam name="T2">items type (any)</typeparam>
        /// <param name="items">items to combine</param>
        /// <param name="itemToCombine">another items to combine</param>
        /// <returns>list of <see cref="DataSet"/></returns>
        public static List<DataSet> CombinationOf<T1, T2>(T1[] items, T2[] itemToCombine) =>
            CombinationOf(items.ToList(), itemToCombine.ToList());

        /// <summary>
        /// Generates list of <see cref="DataSet"/> based on all possible combinations of items between three enumerables.
        /// DataSet name is generated as: [item1 x item2 x item3]
        /// </summary>
        /// <typeparam name="T1">items type (any)</typeparam>
        /// <typeparam name="T2">items type (any)</typeparam>
        /// <typeparam name="T3">items type (any)</typeparam>
        /// <param name="items">items to combine</param>
        /// <param name="itemsToCombine">another items to combine</param>
        /// <param name="anotherItemsToCombine">another items to combine</param>
        /// <returns>list of <see cref="DataSet"/></returns>
        public static List<DataSet> CombinationOf<T1, T2, T3>(
            IEnumerable<T1> items, IEnumerable<T2> itemsToCombine, IEnumerable<T3> anotherItemsToCombine)
        {
            List<DataSet> testData = new List<DataSet>();

            foreach (T1 item in items)
            {
                foreach (T2 itemToCombine in itemsToCombine)
                {
                    foreach (T3 anotherItemToCombine in anotherItemsToCombine)
                    {
                        testData.Add(new DataSet($"[{item} x {itemToCombine} x {anotherItemToCombine}]",
                            item, itemToCombine, anotherItemToCombine));
                    }
                }
            }

            return testData;
        }

        /// <summary>
        /// Generates list of <see cref="DataSet"/> based on all possible combinations of items between three arrays.
        /// DataSet name is generated as: [item1 x item2 x item3]
        /// </summary>
        /// <typeparam name="T1">items type (any)</typeparam>
        /// <typeparam name="T2">items type (any)</typeparam>
        /// <typeparam name="T3">items type (any)</typeparam>
        /// <param name="items">items to combine</param>
        /// <param name="itemsToCombine">another items to combine</param>
        /// <param name="anotherItemsToCombine">another items to combine</param>
        /// <returns>list of <see cref="DataSet"/></returns>
        public static List<DataSet> CombinationOf<T1, T2, T3>(T1[] items, T2[] itemsToCombine, T3[] anotherItemsToCombine) =>
            CombinationOf(items.ToList(), itemsToCombine.ToList(), anotherItemsToCombine.ToList());
    }
}
