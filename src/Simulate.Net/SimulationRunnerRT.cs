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
using System.Threading;

namespace Simulate
{
    /// <summary>
    /// A <see cref="ISimulationRunner"/> that executes approximately in real-time.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class SimulationRunnerRT : SimulationRunnerBase
    {
        #region Declarations

        private readonly double _factor;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="SimulationRunnerRT"/>.
        /// </summary>
        /// <param name="environment">
        /// The simulation environment.
        /// </param>
        /// <param name="factor">
        /// The factor by which to scale a simulation time step (default is <c>1.0</c>).
        /// If a simulation step is from one to ten seconds and the factor is <c>0.5</c> then the
        /// simulation step will take at least five seconds.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="environment"/> is <see langword="null"/>.
        /// </exception>
        public SimulationRunnerRT(SimulationEnvironment environment, double factor = 1.0)
            : base(environment)
        {
            _factor = factor;
        } 

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        protected override void RunCore(TimeSpan until)
        {
            while (Events.Any() && Environment.Now < until)
            {
                var next = Events.Dequeue();
                Sleep(next.At - Environment.Now);
                Step(next);
            }
        }

        #endregion

        #region Private Methods

        private void Sleep(TimeSpan timeout)
        {
            Thread.Sleep(TimeSpan.FromSeconds(timeout.TotalSeconds * _factor));
        } 

        #endregion
    }
}