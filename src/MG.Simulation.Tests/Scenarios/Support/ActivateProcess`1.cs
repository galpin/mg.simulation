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

namespace Simulate.Scenarios.Support
{
    public class ActivateProcess<TSimulationEnvironment> where TSimulationEnvironment : SimulationEnvironment
    {
        private readonly TimeSpan _at;
        private readonly Process<TSimulationEnvironment> _process;

        public ActivateProcess(TimeSpan at, Process<TSimulationEnvironment> process)
        {
            _at = at;
            _process = process;
        }

        public TimeSpan At
        {
            get { return _at; }
        }

        public Process<TSimulationEnvironment> Process
        {
            get { return _process; }
        }
    }
}