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
using Simulate.Events;

namespace Simulate
{
    /// <summary>
    /// Represents a simulation environment.
    /// </summary>
    public class SimulationEnvironment
    {
        #region Public Properties

        /// <summary>
        /// Gets the current simulation time.
        /// </summary>
        public TimeSpan Now { get; internal set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new <see cref="TimeoutEvent"/> that delays for a specified period of time.
        /// </summary>
        /// <param name="delay">
        /// The time for which to delay a process.
        /// </param>
        /// <returns>
        /// A new <see cref="TimeoutEvent"/> that delays for <paramref name="delay"/>.
        /// </returns>
        public TimeoutEvent Timeout(TimeSpan delay)
        {
            return new TimeoutEvent(delay);
        }

        #endregion
    }
}