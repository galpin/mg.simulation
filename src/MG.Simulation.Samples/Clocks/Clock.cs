// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using System.Collections.Generic;
using MG.Simulation.Events;

namespace MG.Simulation.Samples.Clocks
{
    public class Clock : Process
    {
        #region Declarations

        public override IEnumerable<Event> Execute(SimulationEnvironment environment)
        {
            while (true)
            {
                Console.WriteLine(environment.Now);
                yield return environment.Timeout(TimeSpan.FromSeconds(1));
            }
        } 

        #endregion
    }
}