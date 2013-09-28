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
using Basics;

namespace Simulate.Events
{
    /// <summary>
    /// An event that delays a process for a specified period of time.
    /// </summary>
    public class TimeoutEvent : Event
    {
        #region Declarations

        private readonly TimeSpan _delay;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="TimeoutEvent"/>.
        /// </summary>
        /// <param name="generatedOn">
        /// The simulation time at which the event was generated.
        /// </param>
        /// <param name="delay">
        /// The time for which to delay.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="delay"/> is less than <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public TimeoutEvent(TimeSpan generatedOn, TimeSpan delay)
            : base(generatedOn)
        {
            Guard.IsInRange(delay >= TimeSpan.Zero, "delay");

            _delay = delay;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the time for which to delay.
        /// </summary>
        public TimeSpan Delay
        {
            get { return _delay; }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override sealed void Accept(IEventVisitor visitor)
        {
            Guard.IsNotNull(visitor, "visitor");

            visitor.Visit(this);
        } 

        #endregion
    }
}