// Simulate.NET
//
// Copyright (c) Martin Galpin 2013.
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

namespace Simulate.Events
{
    public class EventTests
    {
        #region Public Methods

        [Fact]
        public void GivenCtor_ThenCorrectlyInitialisesMembers_Test()
        {
            var expectedGeneratedOn = TimeSpan.FromSeconds(1);

            var actual = new StubEvent(expectedGeneratedOn);

            Assert.Equal(expectedGeneratedOn, actual.GeneratedOn);
        }

        [Fact]
        public void ImplementsEqualityContract_Test()
        {
        }

        #endregion

        #region StubEvent

        private class StubEvent : Event<StubEvent>
        {
           public StubEvent(TimeSpan generatedOn)
               : base(generatedOn)
           {
           }

            protected override bool EqualsCore(StubEvent other)
            {
                return true;
            }
        }

        #endregion
    }
}