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

using System;
using Moq;
using Xunit;

namespace MG.Simulation
{
    public class SimulationRunnerTests
    {
        #region Public Methods

        [Fact]
        public void GivenCtor_WhenNullSimulationEnvironment_ThenThrows_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new SimulationRunner<SimulationEnvironment>(null));
        }

        [Fact]
        public void GivenCtor_WhenSimulationEnvironment_Then()
        {
            var expected = new SimulationEnvironment();

            var result = new SimulationRunner<SimulationEnvironment>(expected);

            Assert.Equal(expected, result.Environment);
        }

        [Fact]
        public void GivenActivate_WhenAtIsNegative_ThenThrows_Test()
        {
            var result = new SimulationRunner<SimulationEnvironment>(new SimulationEnvironment());
            var at = TimeSpan.FromSeconds(-1);
            var process = new Mock<Process>().Object;

            Assert.Throws<ArgumentOutOfRangeException>(() => result.Activate(at, process));
        }

        [Fact]
        public void GivenActivate_WhenProcessIsNull_ThenThrows_Test()
        {
            var result = new SimulationRunner<SimulationEnvironment>(new SimulationEnvironment());

            Assert.Throws<ArgumentNullException>(() => result.Activate(TimeSpan.Zero, null));
        }

        [Fact]
        public void GivenRun_WhenUntilIsNegative_ThenThrows_Test()
        {
            var result = new SimulationRunner<SimulationEnvironment>(new SimulationEnvironment());
            var until = TimeSpan.FromSeconds(-1);

            Assert.Throws<ArgumentOutOfRangeException>(() => result.Run(until));
        }

        // See Scenarios.

        #endregion
    }
}