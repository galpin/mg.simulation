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
using System.Threading.Tasks;
using MG.Common;

namespace MG.Simulation
{
    /// <summary>
    /// Provides extension methods for <see cref="SimulationBuilder"/> instances.
    /// This class is <see langword="static"/>.
    /// </summary>
    public static class SimulationBuilderExtensions
    {
        #region Public Methods

        /// <summary>
        /// Returns a task that runs a number of simulations until the specified time.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="SimulationBuilder{TSimulationEnvironment}"/>.
        /// </param>
        /// <param name="until">
        /// The time until which to run each simulation.
        /// </param>
        /// <param name="simulations">
        /// The number of simulations to run.
        /// </param>
        /// <returns>
        /// A task that runs a number of simulations until the specified time and returns a enumerable sequence
        /// of simulation results.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="until"/> is less than <see cref="TimeSpan.Zero"/> or 
        /// <paramref name="simulations"/> is less than zero.
        /// </exception>
        public static Task<IEnumerable<SimulationResult<TSimulationEnvironment>>> SimulateAsync<TSimulationEnvironment>(
            this SimulationBuilder<TSimulationEnvironment> builder,
            TimeSpan until,
            int simulations)
            where TSimulationEnvironment : SimulationEnvironment
        {
            Guard.IsNotNull(builder, "builder");
            Guard.IsInRange(until >= TimeSpan.Zero, "at");
            Guard.IsInRange(simulations >= 0, "simulations");

            return Task.Run(() => builder.Simulate(until, simulations));
        }

        /// <summary>
        /// Returns a task that runs a simulation until the specified time.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="SimulationBuilder{TSimulationEnvironment}"/>.
        /// </param>
        /// <param name="until">
        /// The time until which to run the simulation.
        /// </param>
        /// <returns>
        /// A task that runs the simulation and returns the simulation result.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="until"/> is less than <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public static Task<SimulationResult<TSimulationEnvironment>> SimulateAsync<TSimulationEnvironment>(
            this SimulationBuilder<TSimulationEnvironment> builder,
            TimeSpan until)
            where TSimulationEnvironment : SimulationEnvironment
        {
            Guard.IsNotNull(builder, "builder");
            Guard.IsInRange(until >= TimeSpan.Zero, "at");

            return Task.Run(() => builder.Simulate(until));
        }

        #endregion
    }
}