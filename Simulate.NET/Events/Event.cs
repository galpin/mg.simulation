// Copyright (c) Sahara Force India Formula One Team 2013.

using System;
using Basics;

namespace Simulate.Events
{
    /// <summary>
    /// Represents a simulation event. This class is <see langword="abstract"/>.
    /// </summary>
    public abstract class Event
    {
        #region Declarations

        private readonly DateTime _generatedOn;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="Event"/>.
        /// </summary>
        /// <param name="generatedOn">
        /// The date and time that the event was generated, specified in universal time.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="generatedOn"/> is not <see cref="DateTimeKind.Utc"/>.
        /// </exception>
        protected Event(DateTime generatedOn)
        {
            Guard.IsUtc(generatedOn, "generatedOn");

            _generatedOn = generatedOn;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the date and time that the event was generated, specified in universal time.
        /// </summary>
        public DateTime GeneratedOn
        {
            get { return _generatedOn; }
        }

        #endregion
    }
}