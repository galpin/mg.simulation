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
using System.Linq;
using MG.Common;

namespace MG.Simulation.Events
{
    public sealed class CompositeEvent : Event
    {
        private readonly IReadOnlyList<Event> _innerEvents; 

        public CompositeEvent(TimeSpan generatedOn, IEnumerable<Event> innerEvents)
            : base(generatedOn)
        {
            _innerEvents = innerEvents.ToList();
        }

        public CompositeEvent(TimeSpan generatedOn, params Event[] innerEvents)
            : this(generatedOn, innerEvents.AsEnumerable())
        {
        }

        public IReadOnlyList<Event> InnerEvents
        {
            get { return _innerEvents; }
        }

        /// <inheritdoc/>
        public override sealed void Accept(IEventVisitor visitor)
        {
            Guard.IsNotNull(visitor, "visitor");

            visitor.Visit(this);
        } 
    }
}