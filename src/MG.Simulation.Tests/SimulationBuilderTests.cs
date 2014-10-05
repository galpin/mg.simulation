// Simulate.NET
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

using System;
using Xunit;

namespace Simulate
{
    public class SimulationBuilderTests
    {
        #region Public Methods

        [Fact]
        public void GivenCreate_WhenNullFactory_ThenThrows_Test()
        {
            Assert.Throws<ArgumentNullException>(() => SimulationBuilder.Create((Func<SimulationEnvironment>)null));
        }

        [Fact]
        public void GivenCreate_WhenFactoryFunc_ReturnsSimulationBuilderForSimulationEnvironment_Test()
        {
            var builder = SimulationBuilder.Create(() => new SimulationEnvironment());

            Assert.IsType<SimulationBuilder<SimulationEnvironment>>(builder);
        }

        #endregion
    }
}