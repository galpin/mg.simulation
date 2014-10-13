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
using MG.Simulation.Scenarios.Support;
using MG.Simulation.Support;
using Xunit;
using Xunit.Extensions;

namespace MG.Simulation.Scenarios
{
    public abstract class ScenarioBase : TestClass
    {
        #region Declarations

        private readonly TimeSpan _maxDuration;

        private static readonly TimeSpan MaxDuration = TimeSpan.FromHours(1);

        #endregion

        #region Constructors

        protected ScenarioBase(TimeSpan? maxDuration = null)
        {
            _maxDuration = maxDuration ?? MaxDuration;
        }

        #endregion

        #region Public Methods

        [Fact]
        public void AssertScenarioExpectations_Test()
        {
            var runner = new SimulationRunner<SimulationEnvironment>(new SimulationEnvironment());
            foreach (var process in GetProcesses())
            {
                runner.Activate(process.At, process.Process);
            }
            AssertScenario(this, runner);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// When overriden, returns a sequence of processes to activate.
        /// </summary>
        /// <returns>
        /// An enumerable sequence of <see cref="ActivateProcess"/> instances for each process to activate.
        /// </returns>
        protected abstract IEnumerable<ActivateProcess> GetProcesses();

        /// <summary>
        /// When overriden, returns the expected events.
        /// </summary>
        /// <returns>
        /// An enumerable sequence of <see cref="Event"/> instances expected from the simulation.
        /// </returns>
        protected abstract IEnumerable<Event> GetExpectedEvents();

        /// <summary>
        /// When overriden, verifies that state conditions for the simulation environment are met.
        /// </summary>
        /// <param name="environment">
        /// The simulation environment at the end of the simulation.
        /// </param>
        protected abstract void AssertEnvironmentState(SimulationEnvironment environment);

        /// <summary>
        /// Creates a <see cref="ActivateProcess"/>.
        /// </summary>
        /// <param name="process">
        /// The process.
        /// </param>
        /// <param name="at">
        /// The optional activation time.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ActivateProcess"/>.
        /// </returns>
        protected ActivateProcess A(Process<SimulationEnvironment> process, TimeSpan? at = null)
        {
            return new ActivateProcess(at ?? TimeSpan.Zero, process);
        }

        protected static TimeSpan T(double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        #endregion

        #region Private Methods

        private void AssertScenario(ScenarioBase scenario, ISimulationRunner<SimulationEnvironment> runner)
        {
            var expected = scenario.GetExpectedEvents().ToList();
            var actual = new List<Event>();
            runner.Events.Subscribe(actual.Add);
            var result = runner.Run(_maxDuration);
            Assert.SequenceEqual(expected, actual.ToList());
            AssertEnvironmentState(result.Environment);
        }

        #endregion
    }
}