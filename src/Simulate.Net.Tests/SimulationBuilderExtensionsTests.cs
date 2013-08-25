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
using System.Linq;
using Moq;
using Xunit;

namespace Simulate
{
    public class SimulationBuilderExtensionsTests
    {
        #region Public Methods

        [Fact]
        public void GivenSimulateAsync_WhenNullBuilder_ThenThrows_Test()
        {
            var builder = (SimulationBuilder<SimulationEnvironment>)null;

            Assert.Throws<ArgumentNullException>(() => builder.SimulateAsync(TimeSpan.Zero));
        }

        [Fact]
        public void GivenSimulateAsync_WhenNegativeDuration_ThenThrows_Test()
        {
            var builder = SimulationBuilder.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.SimulateAsync(TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void GivenSimulateAsync_WhenSimulationBuilderAndDuration_ThenReturnsTaskThatRunsSimulation_Test()
        {
            var duration = TimeSpan.FromSeconds(1);
            var process = new Mock<Process>();

            var result = SimulationBuilder.Create()
                .Activate(() => process.Object)
                .SimulateAsync(duration)
                .Result;

            Assert.NotNull(result);
            process.Verify(x => x.Execute(It.IsAny<SimulationEnvironment>()), Times.Once());
        }

        [Fact]
        public void GivenSimulateAsyncWithNumberOfSimulations_WhenNullBuilder_ThenThrows_Test()
        {
            var builder = (SimulationBuilder<SimulationEnvironment>)null;

            Assert.Throws<ArgumentNullException>(() => builder.SimulateAsync(TimeSpan.Zero, 10));
        }

        [Fact]
        public void GivenSimulateAsyncWithNumberOfSimulations_WhenNegativeDuration_ThenThrows_Test()
        {
            var builder = SimulationBuilder.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.SimulateAsync(TimeSpan.FromSeconds(-1), 10));
        }

        [Fact]
        public void GivenSimulateAsyncWithNumberOfSimulations_WhenNegativeNumberOfSimulations_ThenThrows_Test()
        {
            var builder = SimulationBuilder.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.SimulateAsync(TimeSpan.Zero, -1));
        }

        [Fact]
        public void GivenSimulateAsyncWithNumberOfSimulations_WhenSimulationBuilderAndDuration_ThenReturnsTasksThatRunsSimulations_Test()
        {
            var duration = TimeSpan.FromSeconds(1);
            var process = new Mock<Process>();
            var expectedSimulations = 10;

            var results = SimulationBuilder.Create()
                .Activate(() => process.Object)
                .SimulateAsync(duration, expectedSimulations)
                .Result
                .ToList();

            Assert.Equal(expectedSimulations, results.Count);
            process.Verify(x => x.Execute(It.IsAny<SimulationEnvironment>()), Times.Exactly(expectedSimulations));
        }

        #endregion
    }
}