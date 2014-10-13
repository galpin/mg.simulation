// Copyright (c) Martin Galpin 2014.
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
using System.Collections.Generic;
using Moq;
using Xunit;

namespace MG.Simulation.Events
{
    public class CompositeEventTests
    {
        [Fact]
        public void Ctor_ThrowsIfEventsIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeEvent(TimeSpan.Zero, (IEnumerable<Event>) null));
        }

        [Fact]
        public void Ctor_CorrectlyInitializesMembers_Test()
        {
            var expectedGeneratedOn = TimeSpan.FromTicks(1);
            var expectedInnerEvents = new[]
            {
                new TimeoutEvent(TimeSpan.Zero, TimeSpan.Zero),
                new TimeoutEvent(TimeSpan.Zero, TimeSpan.Zero)
            };

            var composite = new CompositeEvent(expectedGeneratedOn, expectedInnerEvents);

            Assert.Equal(expectedGeneratedOn, composite.GeneratedOn);
            Assert.Equal(expectedInnerEvents.Length, composite.InnerEvents.Count);
            Assert.Same(expectedInnerEvents[0], composite.InnerEvents[0]);
            Assert.Same(expectedInnerEvents[1], composite.InnerEvents[1]);
        }

        [Fact]
        public void Accept_InvokesVisitOnVisitor_Test()
        {
            var visitor = new Mock<IEventVisitor>();
            var @event = new TimeoutEvent(TimeSpan.Zero, TimeSpan.Zero);

            @event.Accept(visitor.Object);

            visitor.Verify(x => x.Visit(@event), Times.Once);
        }

        [Fact]
        public void Accept_ThrowsIfVisitorIsNull_Test()
        {
            var @event = new TimeoutEvent(TimeSpan.Zero, TimeSpan.Zero);

            Assert.Throws<ArgumentNullException>(() => @event.Accept(null));
        }
        
        // TODO Equlity Contract.
    }
}