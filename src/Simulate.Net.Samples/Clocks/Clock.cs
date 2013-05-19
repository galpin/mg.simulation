// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using System.Collections.Generic;
using Simulate.Events;

namespace Simulate.Samples.Clocks
{
    public class Clock : Process<CustomSimulationEnvironment>
    {
        #region Declarations

        public override IEnumerable<Event> Execute(CustomSimulationEnvironment environment)
        {
            while (true)
            {
                Console.WriteLine(environment.UtcNow());
                yield return new TimeoutEvent(TimeSpan.FromSeconds(1));
            }
        } 

        #endregion
    }
}