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
using System.Threading.Tasks;
using Basics;

namespace Simulate
{
    /// <summary>
    /// A mechanism for programmatically building simulations. This class cannot be inherited.
    /// </summary>
    public sealed class SimulationBuilder
    {
        #region Declarations
 
        private readonly List<ProcessFactory> _processFactories;
        private readonly Func<SimulationEnvironment> _environmentFactory;
        private readonly Func<SimulationEnvironment, ISimulationRunner> _simulationRunnerFactory;

        #endregion

        #region Constructors

        private SimulationBuilder(Func<SimulationEnvironment> environmentFactory)
        {
            _environmentFactory = environmentFactory;
            _processFactories = new List<ProcessFactory>();
            _simulationRunnerFactory = environment => new SimulationRunner(environment);
        }

        #endregion

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
        public static SimulationBuilder Create(Func<SimulationEnvironment> factory)
        {
            Guard.IsNotNull(factory, "environmentFactory");

            return new SimulationBuilder(factory);
        }

        /// <summary>
        /// Activates a simulated process.
        /// </summary>
        /// <param name="factory">
        /// A factory function that creates the <see cref="Process"/> to activate.
        /// </param>
        /// <param name="at">
        /// The optional time at which to activate the process. If not specified, the process will be
        /// activated immediately.
        /// </param>
        /// <returns>
        /// This <see cref="SimulationBuilder"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="factory"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="at"/> is less than <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public SimulationBuilder Activate(Func<Process> factory, TimeSpan? at = null)
        {
            Guard.IsNotNull(factory, "factory");
            Guard.IsInRange(at == null || at >= TimeSpan.Zero, "at");

            _processFactories.Add(new ProcessFactory {
                Factory = factory,
                ActiveAt = at ?? TimeSpan.Zero
            });
            return this;
        }

        /// <summary>
        /// Builds the simulation.
        /// </summary>
        /// <returns>
        /// A <see cref="ISimulationRunner"/> that is ready to run the simulation.
        /// </returns>
        public ISimulationRunner Build()
        {
            var runner = _simulationRunnerFactory(_environmentFactory());
            foreach (var factory in _processFactories)
            {
                runner.Activate(factory.ActiveAt, factory.Factory());
            }
            return runner;
        }

        /// <summary>
        /// Runs until the specified time.
        /// </summary>
        /// <param name="until">
        /// The time until which to run the simulation.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="until"/> is less than <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public void Simulate(TimeSpan until)
        {
            Guard.IsInRange(until >= TimeSpan.Zero, "at");

            Build().Run(until);
        }

        /// <summary>
        /// Runs a number of simulations until the specified time.
        /// </summary>
        /// <param name="until">
        /// The time until which to run each simulation.
        /// </param>
        /// <param name="simulations">
        /// The number of simulations to run.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="until"/> is less than <see cref="TimeSpan.Zero"/> or 
        /// <paramref name="simulations"/> is less than zero.
        /// </exception>
        public void Simulate(TimeSpan until, int simulations)
        {
            Guard.IsInRange(until >= TimeSpan.Zero, "at");
            Guard.IsInRange(simulations >= 0, "simulations");

            Parallel.For(0, simulations, _ => Build().Run(until));
        }

        #endregion

        #region ProcessFactory

        private sealed class ProcessFactory
        {
            public TimeSpan ActiveAt { get; set; }
            public Func<Process> Factory { get; set; } 
        }

        #endregion
    }
}