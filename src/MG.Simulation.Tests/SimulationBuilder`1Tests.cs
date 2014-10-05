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
using System.Collections.Generic;
using System.Linq;
using MG.Simulation.Events;
using Xunit;

namespace MG.Simulation
{
    public class SimulationBuilder_1Tests
    {
        #region Public Methods

        [Fact]
        public void GivenCreate_WhenNullFactory_ThenThrows_Test()
        {
            Assert.Throws<ArgumentNullException>(() => SimulationBuilder<SimulationEnvironment>.Create(null));
        }

        [Fact]
        public void GivenCreate_WhenFactoryFunc_ReturnsSimulationBuilderForSimulationEnvironment_Test()
        {
            var builder = SimulationBuilder<SimulationEnvironment>.Create(() => new SimulationEnvironment());

            Assert.IsType<SimulationBuilder<SimulationEnvironment>>(builder);
        }

        [Fact]
        public void GivenActivate_WhenNullFactory_ThenThrows_Test()
        {
            var builder = CreateBuilder();

            Assert.Throws<ArgumentNullException>(() => builder.Activate(null));
        }

        [Fact]
        public void GivenActivate_WhenNegativeActivationTime_ThenThrows_Test()
        {
            var builder = CreateBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                builder.Activate(() => new StatisticProcess(), TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void GivenActivate_WhenProcessAndNoActivationTime_ActivatesProcessImmediately_Test()
        {
            var process = new StatisticProcess();

            CreateBuilder()
                .Activate(() => process)
                .Simulate(TimeSpan.FromSeconds(5));

            Assert.Equal(TimeSpan.Zero, process.ActivationTime);
        }

        [Fact]
        public void GivenActivate_WhenProcessAndActivationTime_ActivatesProcessAtActivationTime_Test()
        {
            var process = new StatisticProcess();
            var expectedActivationTime = TimeSpan.FromSeconds(1);

            CreateBuilder()
                .Activate(() => process, expectedActivationTime)
                .Simulate(TimeSpan.FromSeconds(5));

            Assert.Equal(expectedActivationTime, process.ActivationTime);
        }

        [Fact]
        public void GivenActivateRange_WhenProcessesAndActivationTime_ActivatesProcessesAtActivationTime_Test()
        {
            var process1 = new StatisticProcess();
            var process2 = new StatisticProcess();
            var expectedActivationTime = TimeSpan.FromSeconds(1);
            var factories = new Func<Process<SimulationEnvironment>>[]
            {
                () => process1,
                () => process2
            };

            CreateBuilder()
                .ActivateRange(factories, expectedActivationTime)
                .Simulate(TimeSpan.FromSeconds(5));

            Assert.Equal(expectedActivationTime, process1.ActivationTime);
            Assert.Equal(expectedActivationTime, process2.ActivationTime);
        }

        [Fact]
        public void GivenSimulate_WhenNegativeSimulationDuration_Throws_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateBuilder().Simulate(TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void GivenSimulate_WhenSimulationDuration_ReturnsSimulationUntilDuration_Test()
        {
            var expectedDuration = TimeSpan.FromSeconds(5);
            var delay = expectedDuration + TimeSpan.FromSeconds(1);

            var result = CreateBuilder()
                .Activate(() => new DelayProcess(delay))
                .Simulate(expectedDuration);

            Assert.Equal(expectedDuration, result.Environment.Now);
        }

        [Fact]
        public void GivenSimulate_WhenSimulationDurationAndNumberOfSimulations_ReturnsNumberOfSimulationsRunUntilDuration_Test()
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

        [Fact]
        public void GivenSubscribe_WithSingleObserverFactory_ForwardsSimulationEventsToObserver_Test()
        {
            var expectedDuration = TimeSpan.FromSeconds(5);
            var actual = new List<Event>();

            CreateBuilder()
                .Activate(() => new ClockProcess())
                .Subscribe(events => events.Subscribe(actual.Add))
                .Simulate(expectedDuration);

            Assert.Equal(6, actual.Count);
            foreach (var @event in actual)
            {
                Assert.IsType<TimeoutEvent>(@event);
            }
        }

        [Fact]
        public void GivenSubscribe_WhenNullObserverFactory_Throws_Test()
        {
            Assert.Throws<ArgumentNullException>(() => CreateBuilder().Subscribe(null));
        }

        [Fact]
        public void GivenSubscribe_WithSingleObserverFactory_InvokesOnCompleteWhenSimulationFinishes_Test()
        {
            var expectedDuration = TimeSpan.FromSeconds(5);
            var isComplete = false;

            CreateBuilder()
                .Activate(() => new ClockProcess())
                .Subscribe(events => events.Subscribe(_ => { }, () => isComplete = true))
                .Simulate(expectedDuration);

            Assert.True(isComplete);
        }

        [Fact]
        public void GivenSubscribe_WithMultipleObserverFactories_ForwardsSimulationEventsToAllObservers_Test()
        {
            var expectedDuration = TimeSpan.FromSeconds(5);
            var actual1 = new List<Event>();
            var actual2 = new List<Event>();

            CreateBuilder()
                .Activate(() => new ClockProcess())
                .Subscribe(events => events.Subscribe(actual1.Add))
                .Subscribe(events => events.Subscribe(actual2.Add))
                .Simulate(expectedDuration);

            Assert.Equal(actual1.Count, actual2.Count);
            //Assert.SequenceEqual(actual1, actual2);
        }

        #endregion

        #region Private Methods

        private static SimulationBuilder<SimulationEnvironment> CreateBuilder()
        {
            return SimulationBuilder<SimulationEnvironment>.Create(() => new SimulationEnvironment());
        }

        #endregion

        #region StatisticProcess

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

        #region ClockProcess

        private sealed class ClockProcess : Process
        {
            public override IEnumerable<Event> Execute(SimulationEnvironment environment)
            {
                while (true)
                {
                    yield return environment.Timeout(TimeSpan.FromSeconds(1));
                }
            }
        }

        #endregion
    }
}