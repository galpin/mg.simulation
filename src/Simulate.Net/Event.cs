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

using Simulate.Events;

namespace Simulate
{
    /// <summary>
    /// Represents a simulation event. This class is <see langword="abstract"/>.
    /// </summary>
    public abstract class Event
    {
        #region Public Methods

        /// <summary>
        /// Accept an <see cref="IEventVisitor"/>.
        /// </summary>
        /// <param name="visitor">
        /// The visitor to accept.
        /// </param>
        public virtual void Accept(IEventVisitor visitor)
        {
        }

        #endregion
    }
}