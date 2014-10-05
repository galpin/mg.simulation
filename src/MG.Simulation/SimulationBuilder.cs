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

namespace Simulate
{
    /// <summary>
    /// A mechanism for programmatically building simulations. This class cannot be inherited.
    /// </summary>
    public class SimulationBuilder
    {
        #region Public Methods

        /// <summary>
        /// Creates a new <see cref="SimulationBuilder"/>.
        /// </summary>
        /// <param name="factory">
        /// A factory function that creates a new <see cref="SimulationEnvironment"/>.
        /// </param>
        /// <returns>
        /// A <see cref="SimulationBuilder"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="factory"/> is <see langword="null"/>.
        /// </exception>
        public static SimulationBuilder<TSimulationEnvironment> Create<TSimulationEnvironment>(
            Func<TSimulationEnvironment> factory)
            where TSimulationEnvironment : SimulationEnvironment
        {
            return SimulationBuilder<TSimulationEnvironment>.Create(factory);
        }

        /// <summary>
        /// Creates a new <see cref="SimulationBuilder"/> for a <see cref="SimulationEnvironment"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="SimulationBuilder{SimulationEnvironment}"/>.
        /// </returns>
        public static SimulationBuilder<SimulationEnvironment> Create()
        {
            return SimulationBuilder<SimulationEnvironment>.Create(() => new SimulationEnvironment());
        }

        #endregion
    }
}