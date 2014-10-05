// Copyright (c) Sahara Force India Formula One Team 2013.

using System;

namespace MG.Simulation.Samples.Clocks
{
    public class Simulation
    {
        #region Public Methods

        public static void Run()
        {
            SimulationBuilder.Create()
                .Activate(() => new Clock())
                .Simulate(TimeSpan.FromSeconds(30));
        }

        #endregion
    }
}