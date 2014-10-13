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
using MG.Common;

namespace MG.Simulation.Events
{
    /// <summary>
    /// Represents a simulation event that executes within a simulation environment.
    /// This class is <see langword="abstract"/>.
    /// </summary>
    public abstract class Event<TEvent> : Event, IEquatable<TEvent> where TEvent : Event<TEvent>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="generatedOn">
        /// The simulation time at which the event was generated.
        /// </param>
        protected Event(TimeSpan generatedOn)
            : base(generatedOn)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override bool Equals(object other)
        {
            return other == this || (other != null && GetType() == other.GetType() && Equals((TEvent)other));
        }

        /// <inheritdoc/>
        public bool Equals(TEvent other)
        {
            return other == this || (other != null && GeneratedOn == other.GeneratedOn && EqualsCore(other));
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return GetHashCodeCore(HashCodeBuilder.For<TEvent>().Add(GeneratedOn));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return String.Format("Event<{0}>()", typeof(TEvent));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current object to equal to <paramref name="other"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        protected abstract bool EqualsCore(TEvent other);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected abstract int GetHashCodeCore(HashCodeBuilder builder);

        #endregion
    }
}