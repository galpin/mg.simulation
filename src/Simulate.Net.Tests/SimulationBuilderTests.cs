// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using Xunit;

namespace Simulate
{
    public class SimulationBuilderTests
    {
        #region Public Methods

        [Fact]
        public void WhenCreate_GivenNullFactory_ThenThrows_Test()
        {
            Assert.Throws<ArgumentNullException>(() => SimulationBuilder.Create((Func<SimulationEnvironment>)null));
        }

        [Fact]
        public void WhenCreate_GivenFactoryFunc_ReturnsSimulationBuilderForSimulationEnvironment_Test()
        {
            var builder = SimulationBuilder.Create(() => new SimulationEnvironment());

            Assert.IsType<SimulationBuilder<SimulationEnvironment>>(builder);
        }

        #endregion
    }
}