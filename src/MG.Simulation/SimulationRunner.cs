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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Basics;
using Simulate.Collections;
using Simulate.Events;

namespace Simulate
{
    /// <summary>
    /// A default <see cref="ISimulationRunner{TSimulationEnvironment}"/> implementation. This class cannot be inherited.
    /// </summary>
    public sealed class SimulationRunner<TSimulationEnvironment> : ISimulationRunner<TSimulationEnvironment>
        where TSimulationEnvironment : SimulationEnvironment
    {
        #region Declarations

        private readonly EventVisitor _visitor;
        private readonly TSimulationEnvironment _environment;
        private readonly PriorityQueue<ScheduledEventEnumerator> _eventQueue;
        private readonly Subject<Event> _events = new Subject<Event>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="SimulationRunner{TSimulationEnvironment}"/>.
        /// </summary>
        /// <param name="environment">
        /// The simulation environment.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="environment"/> is <see langword="null"/>.
        /// </exception>
        public SimulationRunner(TSimulationEnvironment environment)
        {
            Guard.IsNotNull(environment, "environment");

            _environment = environment;
            _visitor = new EventVisitor(this);
            _eventQueue = new PriorityQueue<ScheduledEventEnumerator>();
        }

        #endregion

        #region Public Properties

        /// <inheritdoc/>
        public IObservable<Event> Events
        {
            get { return _events; }
        }

        /// <summary>
        /// Gets the simulation environment.
        /// </summary>
        public TSimulationEnvironment Environment
        {
            get { return _environment; }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Activate(TimeSpan at, Process<TSimulationEnvironment> process)
        {
            Guard.IsInRange(at >= TimeSpan.Zero, "at");
            Guard.IsNotNull(process, "process");

            Enqueue(new ScheduledEventEnumerator(
                at,
                process.Execute(_environment).GetEnumerator()));
        }

        /// <inheritdoc/>
        public SimulationResult<TSimulationEnvironment> Run(TimeSpan until)
        {
            Guard.IsInRange(until >= TimeSpan.Zero, "until");

            while (_eventQueue.Any() && _environment.Now < until)
            {
                Step(until);
            }
            _events.OnCompleted();
            return new SimulationResult<TSimulationEnvironment>(_environment);
        }

        #endregion

        #region Private Methods

        private void Step(TimeSpan until)
        {
            var eventEnumerator = _eventQueue.Dequeue();
            if (eventEnumerator.At > until)
            {
                _environment.Now = until;
                return;
            }
            _environment.Now = eventEnumerator.At;
            if (eventEnumerator.MoveNext())
            {
                _events.OnNext(eventEnumerator.Current);
                _visitor.Handle(eventEnumerator);
            }
        }

        private void Enqueue(ScheduledEventEnumerator enumerator)
        {
            _eventQueue.Enqueue(enumerator);
        }

        #endregion

        #region ScheduledEventEnumerator

        private sealed class ScheduledEventEnumerator : IEnumerator<Event>, IComparable<ScheduledEventEnumerator>
        {
            private readonly IEnumerator<Event> _executeEnumerator;

            public ScheduledEventEnumerator(TimeSpan at, IEnumerator<Event> executeEnumerator)
            {
                At = at;
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

            public TimeSpan At { get; set; }

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
                return At.CompareTo(other.At);
            }
        }

        #endregion

        #region EventVisitor

        private sealed class EventVisitor : IEventVisitor
        {
            private readonly SimulationRunner<TSimulationEnvironment> _runner;
            private ScheduledEventEnumerator _enumerator;

            public EventVisitor(SimulationRunner<TSimulationEnvironment> runner)
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
                _enumerator.At = _runner.Environment.Now + @event.Delay;
                _runner.Enqueue(_enumerator);
            }
        }

        #endregion
    }
}