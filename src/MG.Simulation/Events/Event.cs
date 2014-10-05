// MG.Simulation
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

namespace Simulate.Events
{
    /// <summary>
    /// Represents a simulation event. This class is <see langword="abstract"/>.
    /// </summary>
    public abstract class Event
    {
        #region Declarations

        private readonly TimeSpan _generatedOn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="generatedOn">
        /// The simulation time at which the event was generated.
        /// </param>
        protected Event(TimeSpan generatedOn)
        {
            _generatedOn = generatedOn;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the simulation time at which the event was generated.
        /// </summary>
        public TimeSpan GeneratedOn
        {
            get { return _generatedOn; }
        }

        #endregion

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