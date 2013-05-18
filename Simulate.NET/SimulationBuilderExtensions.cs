// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using System.Threading.Tasks;
using Basics;

namespace Simulate
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
        /// The <see cref="SimulationBuilder"/>.
        /// </param>
        /// <param name="until">
        /// The time until which to run each simulation.
        /// </param>
        /// <param name="simulations">
        /// The number of simulations to run.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="until"/> is less than <see cref="TimeSpan.Zero"/> or 
        /// <paramref name="simulations"/> is less than zero.
        /// </exception>
        public static Task SimulateAsync(this SimulationBuilder builder, TimeSpan until, int simulations)
        {
            Guard.IsInRange(until >= TimeSpan.Zero, "at");
            Guard.IsInRange(simulations >= 0, "simulations");

            return Task.Run(() => builder.Simulate(until, simulations));
        }

        /// <summary>
        /// Returns a task that runs until the specified time.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="SimulationBuilder"/>.
        /// </param>
        /// <param name="until">
        /// The time until which to run the simulation.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="builder"/> is <see langword="null"/>.
        /// </exception>
        /// <returns>
        /// A task that runs the simulation.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="until"/> is less than <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public static Task SimulateAsync(this SimulationBuilder builder, TimeSpan until)
        {
            Guard.IsInRange(until >= TimeSpan.Zero, "at");

            return Task.Run(() => builder.Simulate(until));
        }

        #endregion
    }
}