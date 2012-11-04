// Simulate.NET
//
// Copyright (c) Martin Galpin 2012.
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
using System.Diagnostics;
using Basics;

namespace Simulate.Collections
{
    /// <summary>
    /// Provides a priority queue implementation based on a priority heap. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// Based on the priority queue implementation in <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
    /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.
    /// 
    /// This collection is not thread-safe.
    /// </remarks>
    public sealed class PriorityQueue<T> : IEnumerable<T>, ICollection
    {
        #region Declarations

        /// <summary>
        /// The default initial capacity. This field is constant.
        /// </summary>
        private const int _defaultInitialCapacity = 11;

        /// <summary>
        /// The number of items to expand the capacity by. This field is constant.
        /// </summary>
        private const int _growFactor = 4;

        /// <summary>
        /// The type comparer used to determine priority. This field is read-only.
        /// </summary>
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// The internal priority heap. Stores items at indices 1 to N.
        /// </summary>
        private T[] _heap;

        /// <summary>
        /// The number of items in the heap.
        /// </summary>
        private int _N;

        /// <summary>
        /// The internal queue version. This is used to determine whether the queue has changed during enumeration.
        /// </summary>
        private int _version;

        /// <summary>
        /// An object that can be used to synchronize access to the <see cref="ICollection"/>. This field is read-only.
        /// </summary>
        private readonly object _syncRoot = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="PriorityQueue{T}"/>. 
        /// </summary>
        /// <remarks>
        /// This <see cref="PriorityQueue{T}"/> uses a default comparer (<see cref="Comparer{T}.Default"/>).
        /// </remarks>
        public PriorityQueue()
            : this(_defaultInitialCapacity, Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="PriorityQueue{T}"/> with the specified comparer.
        /// </summary>
        /// <param name="comparer">
        /// The comparer to use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public PriorityQueue(IComparer<T> comparer)
            : this(_defaultInitialCapacity, comparer)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="PriorityQueue{T}"/> with the specified initial capacity.
        /// </summary>
        /// <param name="initialCapacity">
        /// The initial capacity.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown if <paramref name="initialCapacity"/> is less than to equal to zero.
        /// </exception>
        /// <remarks>
        /// This <see cref="PriorityQueue{T}"/> uses a default comparer (<see cref="Comparer{T}.Default"/>).
        /// </remarks>
        public PriorityQueue(int initialCapacity)
            : this(initialCapacity, Comparer<T>.Default)
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="PriorityQueue{T}"/> that contains elements copied from the specified
        /// collection and has sufficient capacity to accomodate the number of elements copied.
        /// </summary>
        /// <param name="source">
        /// The collection whose elements are copied to the new <see cref="PriorityQueue{T}"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        public PriorityQueue(IEnumerable<T> source)
            : this(source, Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="PriorityQueue{T}"/> with the specified initial capacity and comparer.
        /// </summary>
        /// <param name="initialCapacity">
        /// The initial capacity.
        /// </param>
        /// <param name="comparer">
        /// The comparer to use.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown if <paramref name="initialCapacity"/> is less than to equal to zero.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public PriorityQueue(int initialCapacity, IComparer<T> comparer)
        {
            Guard.IsInRange(initialCapacity >= 0, () => initialCapacity);
            Guard.IsNotNull(comparer, () => comparer);

            _heap = new T[initialCapacity + 1];
            _comparer = comparer;
        }

        /// <summary>
        /// Creates a new instance of <see cref="PriorityQueue{T}"/> that contains elements copied from the specified
        /// collection, has sufficient capacity to accomodate the number of elements copied and uses the given comparer.
        /// </summary>
        /// <param name="source">
        /// The collection whose elements are copied to the new <see cref="PriorityQueue{T}"/>.
        /// </param>
        /// <param name="comparer">
        /// The comparer to use.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="source"/> or <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public PriorityQueue(IEnumerable<T> source, IComparer<T> comparer)
            : this(_defaultInitialCapacity, comparer)
        {
            Guard.IsNotNull(source, () => source);
            Guard.IsNotNull(comparer, () => comparer);

            foreach (var item in source)
            {
                Enqueue(item);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of items in the queue.
        /// </summary>
        public int Count
        {
            get { return _N; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if access to the collection is synchronized; otherwise <see langword="false"/>.
        /// This property always returns false.
        /// </returns>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return _syncRoot; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Copies the <see cref="PriorityQueue{T}"/> to an existing one-dimensional <see cref="Array"/>, starting at
        /// the specified array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements.
        /// </param>
        /// <param name="index">
        /// The zero-based starting index in <paramref name="array"/> at which copying begins.
        /// </param>
        /// <remarks>
        /// The elements are copied to the <see cref="Array"/> in the same order in which the enumerator iterates
        /// through the <see cref="PriorityQueue{T}"/>.
        /// 
        /// This method is an O(n) operation, where n is <see cref="Count"/>.
        /// </remarks>
        public void CopyTo(Array array, int index)
        {
            Guard.IsNotNull(array, () => array);
            Guard.IsInRange(index >= 0, () => index);
            Guard.IsTrue(array.Length - index >= Count, () => index);
            
            foreach (var item in this)
            {
                array.SetValue(item, index++);
            }
        }

        /// <summary>
        /// Adds an item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <remarks>
        /// If <see cref="Count"/> already equals the capacity, the capacity of the <see cref="PriorityQueue{T}"/> is
        /// increased by automatically reallocating the internal array and the existing elements are copied to the new
        /// array before the new element is added.
        /// 
        /// If <see cref="Count"/> is less then the capacity then this method is an O(log n) operation. If the internal
        /// array needs to be reallocated to accomodate the new element, this method becomes an O(n) operation.
        /// </remarks>    
        public void Enqueue(T item)
        {
            if (_N == _heap.Length - 1)
            {
                SetCapacity(_N + _growFactor);
            }
            _heap[++_N] = item;
            Swim(_N);
            _version++;
            VerifyHeap();
        }

        /// <summary>
        /// Removes and returns the highest priority item.
        /// </summary>
        /// <returns>
        /// The highest priority item.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the <see cref="PriorityQueue{T}"/> is empty.
        /// </exception>
        /// <remarks>
        /// This method is an O(log n) operation.
        /// </remarks>
        public T Dequeue()
        {
            VerifyNotEmpty();

            var item = _heap[1];
            _heap.Swap(1, _N--);
            Sink(1);
            _heap[_N + 1] = default(T);
            _version++;
            VerifyHeap();
            return item;
        }

        /// <summary>
        /// Returns the highest priority item without removing it.
        /// </summary>
        /// <returns>
        /// The highest priority item.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the <see cref="PriorityQueue{T}"/> is empty.
        /// </exception>
        public T Peek()
        {
            VerifyNotEmpty();

            return _heap[1];
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="PriorityQueue{T}"/>, if that number is
        /// less than 90 percent of current capacity.        
        /// </summary>
        /// <remarks>
        /// This method can be used to minimize a collection's memory overhead if no new elements will be added to the
        /// collection. The cost of reallocating and copying a large <see cref="PriorityQueue{T}"/> can be considerable,
        /// however, so the <see cref="TrimExcess"/> method does nothing if the list is at more than 90 percent of
        /// capacity. This avoids incurring a large reallocation cost for a relatively small gain.
        ///
        /// This method is an O(n) operation, where n is <see cref="Count"/>.
        /// </remarks>
        public void TrimExcess()
        {
            if (_N < _heap.Length - 1 * 0.9)
            {
                SetCapacity(_N);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="PriorityQueue{T}.Enumerator"/> for the <see cref="PriorityQueue{T}"/>.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private Methods

        private bool IsHeap(int k)
        {
            if (k > _N)
            {
                return true;
            }
            var left = 2 * k;
            var right = 2 * k + 1;
            if (left <= _N && IsLess(k, left))
            {
                return false;
            }
            if (right <= _N && IsLess(k, right))
            {
                return false;
            }
            return IsHeap(left) && IsHeap(right);
        }

        private bool IsLess(int i, int j)
        {
            return _comparer.Compare(_heap[i], _heap[j]) < 0;
        }

        private void SetCapacity(int capacity)
        {
            var newHeap = new T[capacity + 1];
            if (_N > 1)
            {
                Array.Copy(_heap, 0, newHeap, 0, _N + 1);
            }
            _heap = newHeap;
            _version++;
        }

        private void Swim(int k)
        {
            while (k > 1 && IsLess(k / 2, k))
            {
                _heap.Swap(k, k / 2);
                k = k / 2;
            }
        }

        private void Sink(int k)
        {
            int j;
            while ((j = 2 * k) <= _N)
            {
                if (j < _N && IsLess(j, j + 1))
                {
                    j++;
                }
                if (!IsLess(k, j))
                {
                    break;
                }
                _heap.Swap(k, j);
                k = j;
            }
        }

        private void VerifyHeap()
        {
#if DEBUG
            Debug.Assert(IsHeap(1));
#endif
        }

        private void VerifyNotEmpty()
        {
            if (_N == 0)
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        #region Enumerator

        [Serializable]
        public class Enumerator : IEnumerator<T>
        {
            #region Declarations
            
            private const int NotStarted = -1;

            private readonly PriorityQueue<T> _priorityQueue;
            private readonly int _version;

            private PriorityQueue<T> _copy;
            private int _index;
            private T _current;

            #endregion

            #region Properties

            /// <inheritdoc/>
            public T Current
            {
                get
                {
                    if (_index == NotStarted)
                    {
                        throw new InvalidOperationException();
                    }
                    return _current;
                }
            }

            /// <inheritdoc/>
            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion

            #region Constructors

            internal Enumerator(PriorityQueue<T> priorityQueue)
            {
                _priorityQueue = priorityQueue;
                _version = priorityQueue._version;
                _index = NotStarted;
                CopyFromSource();
            }

            #endregion

            #region Public Methods

            /// <inheritdoc/>
            public void Dispose()
            {
                _index = NotStarted;
                _current = default(T);
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                VerifyNotChanged();

                var canMoveNext = ++_index < _priorityQueue.Count;
                if (canMoveNext)
                {
                    _current = _copy.Dequeue();
                }
                return canMoveNext;
            }

            /// <inheritdoc/>
            public void Reset()
            {
                VerifyNotChanged();

                _index = NotStarted;
                _current = default(T);
                CopyFromSource();
            } 

            #endregion

            #region Private Methods

            private void CopyFromSource()
            {
                _copy = new PriorityQueue<T>(_priorityQueue.Count)
                {
                    _N = _priorityQueue._N
                };
                Array.Copy(_priorityQueue._heap, 1, _copy._heap, 1, _priorityQueue.Count);
            }

            private void VerifyNotChanged()
            {
                if (_priorityQueue._version != _version)
                {
                    throw new InvalidOperationException();
                }
            }

            #endregion
        }

        #endregion
    }
}