// MG.Simulation
//
// Copyright (c) Martin Galpin 2012.
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

using MG.Common;

namespace MG.Simulation.Collections
{
    /// <summary>
    /// Provides extension methods for arrays. This class is <see langword="static"/>.
    /// </summary>
    internal static class ArrayExtensions
    {
        #region Public Methods
        
        /// <summary>
        /// Swaps two elements of an array.
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="x">The first index to swap.</param>
        /// <param name="y">The second index to swap.</param>
        public static void Swap<T>(this T[] array, int x, int y)
        {
            Guard.IsNotNull(array, "array");

            var temp = array[y];
            array[y] = array[x];
            array[x] = temp;
        }

        #endregion
    }
}