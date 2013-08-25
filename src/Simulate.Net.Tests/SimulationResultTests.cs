// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using Xunit;

namespace Simulate
{
    public class SimulationResultTests
    {
        #region Public Methods

        [Fact]
        public void GivenCtor_WhenNullSimulationEnvironment_ThenThrows_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new SimulationResult<SimulationEnvironment>(null));
        }

        [Fact]
        public void GivenCtor_WhenSimulationEnvironment_Then()
        {
            var expected = new SimulationEnvironment();

            var result = new SimulationResult<SimulationEnvironment>(expected);

            Assert.Equal(expected, result.Environment);
        }

        #endregion
    }
}