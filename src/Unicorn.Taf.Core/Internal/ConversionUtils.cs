using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Unicorn.Taf.Core.Internal
{
    internal static class ConversionUtils
    {
        internal static T GetEnumValue<T>(string stringValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), stringValue, true);
            }
            catch
            {
                throw new ArgumentException(
                    $"{typeof(T)} is not defined. Available values are: " +
                    string.Join(",", Enum.GetValues(typeof(T)).Cast<T>()));
            }
        }
    }
}
