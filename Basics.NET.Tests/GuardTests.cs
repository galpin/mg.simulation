// Basics.NET
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

namespace Basics.NET.Tests
{
    public class GuardTests
    {
        #region Public Methods

        [Fact]
        public void IsNotNull_ThrowsIfParameterIsNull_Test()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(() => Guard.IsNotNull(obj, () => obj));
            Assert.Throws<ArgumentNullException>(() => Guard.IsNotNull(obj, "obj"));
        }

        [Fact]
        public void IsNotNull_DoesNotThrowIfParameterIsNotNull_Test()
        {
            var obj = new object();
            Assert.DoesNotThrow(() => Guard.IsNotNull(obj, () => obj));
            Assert.DoesNotThrow(() => Guard.IsNotNull(obj, "obj"));
        }

        [Fact]
        public void IsInRange_ThrowsIfParameterIsNotWithinRange_Test()
        {
            int param = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.IsInRange(param > 0, () => param));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.IsInRange(param > 0, "param"));
        }

        [Fact]
        public void IsIsRange_DoesNotThrowIfParameterIsWithinRange_Test()
        {
            int param = 10;
            Assert.DoesNotThrow(() => Guard.IsInRange(param > 0, () => param));
            Assert.DoesNotThrow(() => Guard.IsInRange(param > 0, "param"));
        }

        #endregion
    }
}