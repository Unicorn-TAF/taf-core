using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unicorn.Taf.Core.Utility
{
    /// <summary>
    /// Objects comparer with recursive approach to check equality of objects considering all their public fields and 
    /// properties values. It's possible to specify certain fields or properties to exclude them from comparison.
    /// </summary>
    public class DeepObjectsComparer
    {
        private string[] ignorePaths;
        private string bullet;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeepObjectsComparer"/> class.
        /// </summary>
        public DeepObjectsComparer()
        {
            ignorePaths = new string[0];
            bullet = string.Empty;
        }

        /// <summary>
        /// Adds paths to fields/properties to ignore during comparison. 
        /// Field or property which path ends with one of ignore paths will be skipped during comparison.
        /// Example: SomeObject.SomeField.SomeInnerField
        /// </summary>
        /// <param name="ignorePaths"></param>
        /// <returns></returns>
        public DeepObjectsComparer IgnorePaths(params string[] ignorePaths)
        {
            this.ignorePaths = ignorePaths;
            return this;
        }

        /// <summary>
        /// Adds custom bullet when appending items in diff output.
        /// </summary>
        /// <param name="bullet">bullet value</param>
        /// <returns></returns>
        public DeepObjectsComparer UseItemsBulletsInOutput(string bullet)
        {
            this.bullet = bullet + " ";
            return this;
        }

        /// <summary>
        /// Compare two objects with recursive approach to check their equality considering all public fields 
        /// and properties values (primitives, objects, IEnumerable fields).
        /// It there are any ignore paths specified, then fields or properties which paths ends 
        /// with one of ignore paths will be skipped during comparison.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>list of differences if any, otherwise - empty list</returns>
        public List<string> CompareObjects(object actual, object expected) =>
            CompareObjects(actual, expected, "");

        private List<string> CompareObjects(object actual, object expected, string path)
        {
            var differences = new List<string>();

            // Handle null cases
            if (actual == null && expected != null)
            {
                differences.Add(GetDiff(path, "null", "not null"));
                return differences;
            }
            else if (expected == null && actual != null)
            {
                differences.Add(GetDiff(path, "not null", "null"));
                return differences;
            }
            else if (actual == null && expected == null)
            {
                return differences; // Both null, no differences
            }

            // Verify types match
            if (!actual.GetType().Equals(expected.GetType()))
            {
                differences.Add(GetDiff(path, "has type: " + actual.GetType(), "has type: " + expected.GetType()));
                return differences;
            }

            Type type = actual.GetType();
            string currentPath = string.IsNullOrEmpty(path) ? type.Name : path;

            // Compare properties
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string newPath = $"{currentPath}.{property.Name}";
                CompareProperties(property, actual, expected, newPath, differences);
            }

            // Compare fields
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                string newPath = $"{currentPath}.{field.Name}";
                CompareFields(field, actual, expected, newPath, differences);
            }

            return differences;
        }

        private void CompareProperties(PropertyInfo property, object actual, object expected, string newPath, List<string> differences)
        {
            // if current path ends with any entry from fields to ignore, skip this field/property
            if (ignorePaths.Any(f => newPath.EndsWith(f)))
            {
                return;
            }

            // Get values
            object value1 = property.GetValue(actual);
            object value2 = property.GetValue(expected);

            Type propertType = property.PropertyType;

            // Handle primitive types and strings
            if (propertType.IsPrimitive || propertType == typeof(string) || propertType.IsValueType)
            {
                ComparePrimitives(value1, value2, newPath, differences);
            }
            // Handle collections
            else if (typeof(IEnumerable).IsAssignableFrom(propertType))
            {
                CompareCollections((IEnumerable)value1, (IEnumerable)value2, newPath, differences);
            }
            // Handle complex objects recursively
            else
            {
                var nestedDiffs = CompareObjects(value1, value2, newPath);
                differences.AddRange(nestedDiffs);
            }
        }

        private void CompareFields(FieldInfo field, object actual, object expected, string newPath, List<string> differences)
        {
            // if current path ends with any entry from fields to ignore, skip this field/property
            if (ignorePaths.Any(f => newPath.EndsWith(f)))
            {
                return;
            }

            // Get values
            object value1 = field.GetValue(actual);
            object value2 = field.GetValue(expected);

            Type fieldType = field.FieldType;

            // Handle primitive types and strings
            if (fieldType.IsPrimitive || fieldType == typeof(string) || fieldType.IsValueType)
            {
                ComparePrimitives(value1, value2, newPath, differences);
            }
            // Handle collections
            else if (typeof(IEnumerable).IsAssignableFrom(fieldType))
            {
                CompareCollections((IEnumerable)value1, (IEnumerable)value2, newPath, differences);
            }
            // Handle complex objects recursively
            else
            {
                var nestedDiffs = CompareObjects(value1, value2, newPath);
                differences.AddRange(nestedDiffs);
            }
        }

        private void ComparePrimitives(object value1, object value2, string newPath, List<string> differences)
        {
            if (value1 != null)
            {
                if (!value1.Equals(value2))
                {
                    differences.Add(GetDiff(newPath, value1, value2));
                }
            }
            else if (value2 != null)
            {
                differences.Add(GetDiff(newPath, "null", value2));
            }
        }

        private void CompareCollections(IEnumerable enumerable1, IEnumerable enumerable2, string newPath, List<string> differences)
        {
            var en1 = enumerable1.GetEnumerator();
            var en2 = enumerable2.GetEnumerator();

            int i = 0;
            while (en1.MoveNext())
            {
                if (!en2.MoveNext())
                {
                    differences.Add(GetDiff(newPath, "collection size differs"));
                    break;
                }

                string diff = CompareObjects(en1.Current, en2.Current, $"{newPath}[{i}]").FirstOrDefault();

                if (diff != null)
                {
                    differences.Add(diff);
                }

                i++;
            }

            // in case if enumerable2 has more items, but reached end of enumerable1
            if (en2.MoveNext())
            {
                differences.Add(GetDiff(newPath, "collection size differs"));
            }
        }

        private string GetDiff(string path, object actual, object expected) =>
            bullet + path +
            Environment.NewLine + "    Expected >> " + expected +
            Environment.NewLine + "      Actual >> " + actual;

        private string GetDiff(string path, string message) =>
            bullet + path + Environment.NewLine + "             >> " + message;
    }
}
