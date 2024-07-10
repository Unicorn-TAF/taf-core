// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Base matcher for objects without type parameterization.
    /// </summary>
    public abstract class TypeUnsafeMatcher : BaseMatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeUnsafeMatcher"/> class.
        /// </summary>
        protected TypeUnsafeMatcher() : base()
        {
        }

        /// <summary>
        /// Checks if target object matches condition corresponding to specific matcher implementation.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public abstract bool Matches(object actual);
    }
}
