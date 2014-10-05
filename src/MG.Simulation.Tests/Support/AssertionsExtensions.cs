// MG.Simulation
//
// Copyright (c) Martin Galpin 2013.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library. If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit.Extensions;
using Xunit.Sdk;

namespace Simulate.Support
{
    /// <summary>
    /// Provides extension methods for <see cref="Assertions"/> instances. This class is <see langword="static"/>.
    /// </summary>
    internal static class AssertionsExtensions
    {
        #region Public Methods

        /// <summary>
        /// Asserts that the given sequences are equal, using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">
        /// The sequence element type.
        /// </typeparam>
        /// <param name="assert">
        /// The assertions.
        /// </param>
        /// <param name="expected">
        /// The expected sequence.
        /// </param>
        /// <param name="actual">
        /// The actual sequence.
        /// </param>
        /// <exception cref="Xunit.Sdk.EqualException">
        /// Thrown when the given sequences are not equal.
        /// </exception>
        [DebuggerStepThrough]
        public static void SequenceEqual<T>(this Assertions assert, IEnumerable<T> expected, IEnumerable<T> actual)
        {
            SequenceEqual(assert, expected, actual, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Asserts that the given sequences are equal, using a specified equality.
        /// </summary>
        /// <typeparam name="T">
        /// The sequence element type.
        /// </typeparam>
        /// <param name="assert">
        /// The assertions.
        /// </param>
        /// <param name="expected">
        /// The expected sequence.
        /// </param>
        /// <param name="actual">
        /// The actual sequence.
        /// </param>
        /// <param name="comparer">
        /// The equality comparer.
        /// </param>
        /// <exception cref="Xunit.Sdk.EqualException">
        /// Thrown when the given sequences are not equal.
        /// </exception>
        [DebuggerStepThrough]
        public static void SequenceEqual<T>(
            this Assertions assert,
            IEnumerable<T> expected,
            IEnumerable<T> actual,
            IEqualityComparer<T> comparer)
        {
            if (expected == null)
            {
                if (actual != null)
                {
                    throw new EqualException(null, actual);
                }
            }
            else if (actual == null)
            {
                throw new EqualException(expected, null);
            }
            else if (!expected.SequenceEqual(actual, comparer))
            {
                throw new EqualException(expected, actual);
            }
        }

        #endregion
    }
}