﻿// Copyright (c) Martin Galpin 2013.
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
using Moq;
using Xunit;

namespace MG.Simulation.Events
{
    public class TimeoutEventTests
    {
        #region Public Methods

        [Fact]
        public void GivenCtor_ThenCorrectlyInitialisesMembers_Test()
        {
            var expectedGeneratedOn = TimeSpan.FromSeconds(1);
            var expectedDelay = TimeSpan.FromSeconds(2);

            var actual = new TimeoutEvent(expectedGeneratedOn, expectedDelay);

            Assert.Equal(expectedGeneratedOn, actual.GeneratedOn);
            Assert.Equal(expectedDelay, actual.Delay);
        }

        [Fact]
        public void GivenCtor_WhenDelayIsNegative_ThenThrows_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TimeoutEvent(TimeSpan.Zero, TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void GivenAccept_WhenVisitor_ThenCallsVisitOnVisitor_Test()
        {
            var visitor = new Mock<IEventVisitor>();
            var @event = new TimeoutEvent(TimeSpan.Zero, TimeSpan.Zero);

            @event.Accept(visitor.Object);

            visitor.Verify(x => x.Visit(@event), Times.Once);
        }

        [Fact]
        public void GivenAccept_WhenVisitorIsNull_ThenThrows_Test()
        {
            var @event = new TimeoutEvent(TimeSpan.Zero, TimeSpan.Zero);

            Assert.Throws<ArgumentNullException>(() => @event.Accept(null));
        }

        #endregion
    }
}