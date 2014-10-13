// Copyright (c) Martin Galpin 2014.
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

namespace MG.Simulation.Scenarios.Timeouts
{
    public abstract class ClockScenarioBase : ScenarioBase
    {
        private static readonly TimeSpan MaxDuration = T(20);

        protected ClockScenarioBase()
            : base(MaxDuration)
        {
        }

        protected override void AssertEnvironmentState(SimulationEnvironment environment)
        {
            Assert.Equal(MaxDuration, environment.Now);
        }

        internal sealed class ClockProcess : Process
        {
            private readonly TimeSpan _tick;

            public ClockProcess(TimeSpan tick)
            {
                _tick = tick;
            }

            public override IEnumerable<Event> Execute(SimulationEnvironment environment)
            {
                while (true)
                {
                    yield return environment.Timeout(_tick);
                }
            }
        }
    }
}