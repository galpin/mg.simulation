// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using Xunit;

namespace Simulate.Events
{
    public class EventTests
    {
        #region Public Methods

        [Fact]
        public void Ctor_CorrectlyInitialisesMembers_Test()
        {
            var expectedGeneratedOn = DateTime.UtcNow;

            var actual = new StubEvent(expectedGeneratedOn);

            Assert.Equal(expectedGeneratedOn, actual.GeneratedOn);
        }

        #endregion

        #region StubEvent

        private sealed class StubEvent : Event
        {
            public StubEvent(DateTime generatedOn) : base(generatedOn)
            {
            }
        }

        #endregion
    }
}