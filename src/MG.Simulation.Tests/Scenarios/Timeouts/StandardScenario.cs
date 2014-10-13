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

using System.Collections.Generic;
using MG.Simulation.Events;
using MG.Simulation.Scenarios.Support;

namespace MG.Simulation.Scenarios.Timeouts
{
    public class StandardScenario : ClockScenarioBase
    {
        protected override IEnumerable<ActivateProcess> GetProcesses()
        {
            yield return A(new ClockProcess(T(1)));
        }

        protected override IEnumerable<Event> GetExpectedEvents()
        {
            yield return new TimeoutEvent(T(0), T(1));
            yield return new TimeoutEvent(T(1), T(1));
            yield return new TimeoutEvent(T(2), T(1));
            yield return new TimeoutEvent(T(3), T(1));
            yield return new TimeoutEvent(T(4), T(1));
            yield return new TimeoutEvent(T(5), T(1));
            yield return new TimeoutEvent(T(6), T(1));
            yield return new TimeoutEvent(T(7), T(1));
            yield return new TimeoutEvent(T(8), T(1));
            yield return new TimeoutEvent(T(9), T(1));
            yield return new TimeoutEvent(T(10), T(1));
            yield return new TimeoutEvent(T(11), T(1));
            yield return new TimeoutEvent(T(12), T(1));
            yield return new TimeoutEvent(T(13), T(1));
            yield return new TimeoutEvent(T(14), T(1));
            yield return new TimeoutEvent(T(15), T(1));
            yield return new TimeoutEvent(T(16), T(1));
            yield return new TimeoutEvent(T(17), T(1));
            yield return new TimeoutEvent(T(18), T(1));
            yield return new TimeoutEvent(T(19), T(1));
            yield return new TimeoutEvent(T(20), T(1));
        }
    }
}