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

using System;
using Xunit;

namespace MG.Simulation.Collections
{
    public class ArrayExtensionsTests
    {
        #region Public Methods

        [Fact]
        public void Swap_ThrowsIfArrayIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => ((object[]) null).Swap(0, 1));
        }

        [Fact]
        public void Swap_Test()
        {
            var array = new[] {0, 1, 2, 3};

            array.Swap(1, 2);

            Assert.Equal(new[] {0, 2, 1, 3}, array);
        }

        #endregion
    }
}