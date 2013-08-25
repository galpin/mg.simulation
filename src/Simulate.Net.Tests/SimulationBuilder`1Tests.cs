// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simulate
{
    public class SimulationBuilder_1Tests
    {
        #region Public Methods

        [Fact]
        public void WhenCreate_GivenNullFactory_ThenThrows_Test()
        {
            Assert.Throws<ArgumentNullException>(() => SimulationBuilder<SimulationEnvironment>.Create(null));
        }

        [Fact]
        public void WhenCreate_GivenFactoryFunc_ReturnsSimulationBuilderForSimulationEnvironment_Test()
        {
            var builder = SimulationBuilder<SimulationEnvironment>.Create(() => new SimulationEnvironment());

            Assert.IsType<SimulationBuilder<SimulationEnvironment>>(builder);
        }

        [Fact]
        public void WhenActivate_GivenNullFactory_ThenThrows_Test()
        {
            var builder = CreateBuilder();

            Assert.Throws<ArgumentNullException>(() => builder.Activate(null));
        }

        [Fact]
        public void WhenActivate_GivenNegativeActivationTime_ThenThrows_Test()
        {
            var builder = CreateBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                builder.Activate(() => new StatisticProcess(), TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void WhenActivate_GivenProcessAndNoActivationTime_ActivatesProcessImmediately_Test()
        {
            var process = new StatisticProcess();

            CreateBuilder()
                .Activate(() => process)
                .Simulate(TimeSpan.FromSeconds(5));

            Assert.Equal(TimeSpan.Zero, process.ActivationTime);
        }

        [Fact]
        public void WhenActivate_GivenProcessAndActivationTime_ActivatesProcessAtActivationTime_Test()
        {
            var process = new StatisticProcess();
            var expectedActivationTime = TimeSpan.FromSeconds(1);

            CreateBuilder()
                .Activate(() => process, expectedActivationTime)
                .Simulate(TimeSpan.FromSeconds(5));

            Assert.Equal(expectedActivationTime, process.ActivationTime);
        }

        [Fact]
        public void WhenSimulate_GivenNegativeSimulationDuration_Throws_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateBuilder().Simulate(TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void WhenSimulate_GivenSimulationDuration_ReturnsSimulationUntilDuration_Test()
        {
            var expectedDuration = TimeSpan.FromSeconds(5);
            var delay = expectedDuration + TimeSpan.FromSeconds(1);

            var result = CreateBuilder()
                .Activate(() => new DelayProcess(delay))
                .Simulate(expectedDuration);

            Assert.Equal(expectedDuration, result.Environment.Now);
        }

        [Fact]
        public void WhenSimulate_GivenSimulationDurationAndNumberOfSimulations_ReturnsNumberOfSimulationsRunUntilDuration_Test()
        {
            var expectedSimulations = 10;
            var expectedDuration = TimeSpan.FromSeconds(5);
            var delay = expectedDuration + TimeSpan.FromSeconds(1);

            var results = CreateBuilder()
                .Activate(() => new DelayProcess(delay))
                .Simulate(expectedDuration, expectedSimulations)
                .ToList();

            Assert.Equal(expectedSimulations, results.Count);
            foreach (var result in results)
            {
                Assert.Equal(expectedDuration, result.Environment.Now);
            }
        }

        #endregion

        #region Private Methods

        private static SimulationBuilder<SimulationEnvironment> CreateBuilder()
        {
            return SimulationBuilder<SimulationEnvironment>.Create(() => new SimulationEnvironment());
        }

        #endregion

        #region ProcessStub

        private sealed class StatisticProcess : Process
        {
            public TimeSpan ActivationTime;

            public override IEnumerable<Event> Execute(SimulationEnvironment environment)
            {
                ActivationTime = environment.Now;
                yield break;
            }
        }

        #endregion

        #region DelayProcess

        private sealed class DelayProcess : Process
        {
            private readonly TimeSpan _delay;

            public DelayProcess(TimeSpan delay)
            {
                _delay = delay;
            }

            public override IEnumerable<Event> Execute(SimulationEnvironment environment)
            {
                yield return environment.Timeout(_delay);
            }
        }

        #endregion
    }
}