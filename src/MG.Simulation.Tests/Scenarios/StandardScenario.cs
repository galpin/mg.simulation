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
using MG.Simulation.Events;
using MG.Simulation.Scenarios.Support;

namespace MG.Simulation.Scenarios
{
    public class StandardScenario : ScenarioBase
    {
        #region Protected Methods

        /// <inheritdoc/>
        protected override IEnumerable<ActivateProcess> GetProcesses()
        {
            yield return A(new StubProcess());
        }

        /// <inheritdoc/>
        protected override IEnumerable<Event> GetExpectedEvents()
        {
            yield return new TimeoutEvent(TimeSpan.Zero, TimeSpan.FromSeconds(1));
            yield return new TimeoutEvent(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
            yield return new TimeoutEvent(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3));
        }

        /// <inheritdoc/>
        protected override void AssertEnvironmentState(SimulationEnvironment environment)
        {
            Assert.Equal(TimeSpan.FromSeconds(3), environment.Now);
        }

        #endregion

        #region StubProcess

        private class StubProcess : Process
        {
            public override IEnumerable<Event> Execute(SimulationEnvironment environment)
            {
                yield return environment.Timeout(TimeSpan.FromSeconds(1));
                yield return environment.Timeout(TimeSpan.FromSeconds(2));
                yield return environment.Timeout(TimeSpan.FromSeconds(3));
            }
        }

        #endregion
    }
}