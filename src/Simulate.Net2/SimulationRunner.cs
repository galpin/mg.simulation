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
using System.Linq;

namespace Simulate
{
    /// <summary>
    /// A default <see cref="ISimulationRunner"/> implementation. This class cannot be inherited.
    /// </summary>
    public sealed class SimulationRunner : SimulationRunnerBase
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="SimulationRunner"/>.
        /// </summary>
        /// <param name="environment">
        /// The simulation environment.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="environment"/> is <see langword="null"/>.
        /// </exception>
        public SimulationRunner(SimulationEnvironment environment)
            : base(environment)
        {
        }

        #endregion

        #region Constructors

        /// <inheritdoc/>
        protected override void RunCore(TimeSpan until)
        {
            while (Events.Any() && Environment.Now < until)
            {
                Step(Events.Dequeue());
            }
        } 

        #endregion
    }
}