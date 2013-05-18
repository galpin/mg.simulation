﻿// Simulate.NET
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

namespace Simulate.Events
{
    /// <summary>
    /// Represents a visitor for <see cref="Event"/>'s.
    /// </summary>
    public interface IEventVisitor
    {
        #region Methods

        /// <summary>
        /// Visits the specified event.
        /// </summary>
        /// <param name="event">
        /// The event to visit.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="@event"/> is <see langword="null"/>.
        /// </exception>
        void Visit(TimeoutEvent @event);

        #endregion
    }
}