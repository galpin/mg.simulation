// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using Moq;
using Xunit;

namespace Simulate.Events
{
    public class EventTests
    {
        #region Public Methods

        [Fact]
        public void GivenCtor_WhenDelay_ThenCorrectlyInitialisesMembers_Test()
        {
            var expectedDelay = TimeSpan.Zero;

            var actual = new TimeoutEvent(expectedDelay);

            Assert.Equal(expectedDelay, actual.Delay);
        }

        [Fact]
        public void GivenCtor_WhenDelayIsNegative_ThenThrows_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TimeoutEvent(TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void GivenAccept_WhenVisitor_ThenCallsVisitOnVisitor_Test()
        {
            var visitor = new Mock<IEventVisitor>();
            var @event = new TimeoutEvent(TimeSpan.Zero);

            @event.Accept(visitor.Object);

            visitor.Verify(x => x.Visit(@event), Times.Once);
        }

        [Fact]
        public void GivenAccept_WhenVisitorIsNull_ThenThrows_Test()
        {
            var @event = new TimeoutEvent(TimeSpan.Zero);

            Assert.Throws<ArgumentNullException>(() => @event.Accept(null));
        }

        #endregion
    }
}