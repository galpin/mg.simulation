// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using Xunit;

namespace Simulate
{
    public class SimulationBuilderTests
    {
        #region Public Methods

        [Fact]
        public void GivenCreate_WhenNullFactory_ThenThrows_Test()
        {
            Assert.Throws<ArgumentNullException>(() => SimulationBuilder.Create((Func<SimulationEnvironment>)null));
        }

        [Fact]
        public void GivenCreate_WhenFactoryFunc_ReturnsSimulationBuilderForSimulationEnvironment_Test()
        {
            var builder = SimulationBuilder.Create(() => new SimulationEnvironment());

            Assert.IsType<SimulationBuilder<SimulationEnvironment>>(builder);
        }

        #endregion
    }
}