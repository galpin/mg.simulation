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

using MG.Common;

namespace Simulate
{
    /// <summary>
    /// Provides the results of a simulation. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="TSimulationEnvironment">
    /// The simulation environment.
    /// </typeparam>
    public sealed class SimulationResult<TSimulationEnvironment>
        where TSimulationEnvironment : SimulationEnvironment
    {
        #region Declarations

        private readonly TSimulationEnvironment _environment;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="SimulationResult{TSimulationEnvironment}"/>.
        /// </summary>
        /// <param name="environment">
        /// The simulation environment.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="environment"/> is <see langword="null"/>.
        /// </exception>
        public SimulationResult(TSimulationEnvironment environment)
        {
            Guard.IsNotNull(environment, "environment");

            _environment = environment;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the simulation environment.
        /// </summary>
        public TSimulationEnvironment Environment
        {
            get { return _environment; }
        }

        #endregion
    }
}