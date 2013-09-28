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
using System.Collections.Generic;
using System.Linq;
using Simulate.Events;
using Simulate.Scenarios.Support;
using Simulate.Support;
using Xunit;
using Xunit.Extensions;

namespace Simulate.Scenarios
{
    public abstract class ScenarioBase : TestClass
    {
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

        #endregion

        #region Private Methods

        private void AssertScenario(
            ScenarioBase scenario,
            SimulationRunner<SimulationEnvironment> runner)
        {
            var expected = scenario.GetExpectedEvents().ToList();
            var actual = new List<Event>();
            //runner.Events.Subscribe(x => actual.Add(x));
            var result = runner.Run(TimeSpan.Zero);
            Assert.SequenceEqual(expected, actual.ToList());
            AssertEnvironmentState(result.Environment);
        }

        #endregion
    }
}