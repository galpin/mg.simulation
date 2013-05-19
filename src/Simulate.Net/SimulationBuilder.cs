using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basics;

namespace Simulate
{
    public class SimulationBuilder
    {
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
        public static SimulationBuilder<TEnvironment> Create<TEnvironment>(Func<TEnvironment> factory) where TEnvironment : SimulationEnvironment
        {
            Guard.IsNotNull(factory, "environmentFactory");

            return new SimulationBuilder<TEnvironment>(factory);
        }

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
        public static SimulationBuilder<SimulationEnvironment> Create()
        {
            return new SimulationBuilder<SimulationEnvironment>(() => new SimulationEnvironment());
        }
    }
}
