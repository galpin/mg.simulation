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

using System;
using System.Collections;
using System.Collections.Generic;
using Basics;
using Simulate.Collections;
using Simulate.Events;

namespace Simulate
{
    /// <summary>
    /// An <see langword="abstract"/> base for <see cref="ISimulationRunner"/> implementations.
    /// </summary>
    public abstract class SimulationRunnerBase : ISimulationRunner
    {
        #region Declarations

        private readonly EventVisitor _visitor;
        private readonly SimulationEnvironment _environment;
        private readonly PriorityQueue<ScheduledEventEnumerator> _events;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="SimulationRunnerBase"/>.
        /// </summary>
        /// <param name="environment">
        /// The simulation environment.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="environment"/> is <see langword="null"/>.
        /// </exception>
        protected SimulationRunnerBase(SimulationEnvironment environment)
        {
            Guard.IsNotNull(environment, "environment");

            _environment = environment;
            _visitor = new EventVisitor(this);
            _events = new PriorityQueue<ScheduledEventEnumerator>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the simulation environment.
        /// </summary>
        public SimulationEnvironment Environment
        {
            get { return _environment; }
        }

        /// <summary>
        /// Gets the queue of events.
        /// </summary>
        protected PriorityQueue<ScheduledEventEnumerator> Events
        {
            get { return _events; }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Activate(TimeSpan at, Process process)
        {
            Guard.IsInRange(at >= TimeSpan.Zero, "at");
            Guard.IsNotNull(process, "process");

            Enqueue(at, process);
        }

        /// <inheritdoc/>
        public void Run(TimeSpan until)
        {
            Guard.IsInRange(until >= TimeSpan.Zero, "until");

            RunCore(until);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// When overriden, runs the simulation.
        /// </summary>
        /// <param name="until">
        /// The time until which to run the simulation.
        /// </param>
        /// <returns>
        /// A task that runs the simulation.
        /// </returns>
        protected abstract void RunCore(TimeSpan until);

        /// <summary>
        /// Performs a simulation step.
        /// </summary>
        /// <param name="event">
        /// The event to execute during this step.
        /// </param>
        protected void Step(ScheduledEventEnumerator @event)
        {
            _environment.Now = @event.At;
            if (@event.MoveNext())
            {
                _visitor.Handle(@event);
            }
        } 

        #endregion

        #region Private Methods

        private void Enqueue(TimeSpan at, Event @event)
        {
            _events.Enqueue(new ScheduledEventEnumerator(
                at,
                @event.Execute(_environment).GetEnumerator()));
        }

        #endregion

        #region ScheduledEventEnumerator

        protected sealed class ScheduledEventEnumerator : IEnumerator<Event>, IComparable<ScheduledEventEnumerator>
        {
            private readonly IEnumerator<Event> _executeEnumerator;
            private TimeSpan _at;

            public ScheduledEventEnumerator(TimeSpan at, IEnumerator<Event> executeEnumerator)
            {
                _at = at;
                _executeEnumerator = executeEnumerator;
            }

            public Event Current
            {
                get { return _executeEnumerator.Current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public TimeSpan At
            {
                get { return _at; }
                set { _at = value; }
            }

            public void Dispose()
            {
                _executeEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                return _executeEnumerator.MoveNext();
            }

            public void Reset()
            {
                _executeEnumerator.Reset();
            }

            public int CompareTo(ScheduledEventEnumerator other)
            {
                if (At < other.At)
                {
                    return 1;
                }
                if (At > other.At)
                {
                    return -1;
                }
                return 0;
            }
        }

        #endregion

        #region EventVisitor

        private sealed class EventVisitor : IEventVisitor
        {
            private readonly SimulationRunnerBase _runner;
            private ScheduledEventEnumerator _enumerator;

            public EventVisitor(SimulationRunnerBase runner)
            {
                _runner = runner;
            }

            public void Handle(ScheduledEventEnumerator enumerator)
            {
                _enumerator = enumerator;
                _enumerator.Current.Accept(this);
                _enumerator = null;
            }

            public void Visit(TimeoutEvent @event)
            {
                _runner.Enqueue(
                    _runner.Environment.Now + @event.Delay,
                    new ResumeEvent(_enumerator));
            }
        }

        #endregion

        #region ResumeEvent

        private sealed class ResumeEvent : Event
        {
            private readonly IEnumerator<Event> _parent;

            public ResumeEvent(IEnumerator<Event> parent)
            {
                _parent = parent;
            }

            public override IEnumerable<Event> Execute(SimulationEnvironment environment)
            {
                while (_parent.MoveNext())
                {
                    yield return _parent.Current;
                }
            }
        } 

        #endregion
    }
}