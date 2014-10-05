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
    /// Represents a mechanism for running a simulation.
    /// </summary>
    public interface ISimulationRunner<TSimulationEnvironment> where TSimulationEnvironment : SimulationEnvironment
    {
        #region Properties

        /// <summary>
        /// Gets an observable sequence of events executed by running the simulation.
        /// </summary>
        IObservable<Event> Events { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Activate a <see cref="Process{TSimulationEnvironment}"/> at a specified time.
        /// </summary>
        /// <param name="at">
        /// The time at which to activate <paramref name="process"/>.
        /// </param>
        /// <param name="process">
        /// The process to activate.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="at"/> is less than <see cref="TimeSpan.Zero"/>.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="process"/> is <see langword="null"/>.
        /// </exception>
        void Activate(TimeSpan at, Process<TSimulationEnvironment> process);

        /// <summary>
        /// Run the simulation.
        /// </summary>
        /// <param name="until">
        /// The time until which to run the simulation.
        /// </param>
        SimulationResult<TSimulationEnvironment> Run(TimeSpan until);

        #endregion
    }
}