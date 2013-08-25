// Copyright (c) Sahara Force India Formula One Team 2013.

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
        public void WhenSimulateAsync_GivenNullBuilder_ThenThrows_Test()
        {
            var builder = (SimulationBuilder<SimulationEnvironment>)null;

            Assert.Throws<ArgumentNullException>(() => builder.SimulateAsync(TimeSpan.Zero));
        }

        [Fact]
        public void WhenSimulateAsync_GivenNegativeDuration_ThenThrows_Test()
        {
            var builder = SimulationBuilder.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.SimulateAsync(TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void WhenSimulateAsync_GivenSimulationBuilderAndDuration_ThenReturnsTaskThatRunsSimulation_Test()
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
        public void WhenSimulateAsyncWithNumberOfSimulations_GivenNullBuilder_ThenThrows_Test()
        {
            var builder = (SimulationBuilder<SimulationEnvironment>)null;

            Assert.Throws<ArgumentNullException>(() => builder.SimulateAsync(TimeSpan.Zero, 10));
        }

        [Fact]
        public void WhenSimulateAsyncWithNumberOfSimulations_GivenNegativeDuration_ThenThrows_Test()
        {
            var builder = SimulationBuilder.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.SimulateAsync(TimeSpan.FromSeconds(-1), 10));
        }

        [Fact]
        public void WhenSimulateAsyncWithNumberOfSimulations_GivenNegativeNumberOfSimulations_ThenThrows_Test()
        {
            var builder = SimulationBuilder.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.SimulateAsync(TimeSpan.Zero, -1));
        }

        [Fact]
        public void WhenSimulateAsyncWithNumberOfSimulations_GivenSimulationBuilderAndDuration_ThenReturnsTasksThatRunsSimulations_Test()
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