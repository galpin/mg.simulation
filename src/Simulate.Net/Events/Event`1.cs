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

namespace Simulate.Events
{
    /// <summary>
    /// Represents a simulation event that executes within a simulation environment.
    /// This class is <see langword="abstract"/>.
    /// </summary>
    public abstract class Event<TSimulationEnvironment> : Event where TSimulationEnvironment : SimulationEnvironment
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="generatedOn">
        /// The simulation time at which the event was generated.
        /// </param>
        protected Event(TimeSpan generatedOn)
            : base(generatedOn)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Execute the event.
        /// </summary>
        /// <param name="environment">
        /// The simulation environment in which to execute.
        /// </param>
        /// <returns>
        /// An enumerable sequence of <see cref="Event"/>'s that are the result of this events execution.
        /// </returns>
        public virtual IEnumerable<Event> Execute(TSimulationEnvironment environment)
        {
            yield break;
        }

        #endregion
    }
}