// MG.Simulation
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
using System.Linq;
using Xunit;

namespace MG.Simulation.Collections
{
    public class PriorityQueueTests
    {
        #region Public Methods

        [Fact]
        public void CopyTo_Test()
        {
            var priorityQueue = new PriorityQueue<int>(Enumerable.Range(0, 100));
            var destinationArray = new int[priorityQueue.Count];

            priorityQueue.CopyTo(destinationArray, 0);

            Assert.Equal(priorityQueue, destinationArray);
        }

        [Fact]
        public void CopyTo_CanStartCopyingAtSpecificIndex_Test()
        {
            var priorityQueue = new PriorityQueue<int>(Enumerable.Range(0, 100));
            var destinationArray = new int[priorityQueue.Count + 1];

            priorityQueue.CopyTo(destinationArray, 1);

            Assert.Equal(priorityQueue, destinationArray.Skip(1));
        }

        [Fact]
        public void CopyTo_ThrowsIfArrayIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>().CopyTo(null, 0));
        }

        [Fact]
        public void CopyTo_ThrowsIfIndexIsLessThanZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PriorityQueue<int>().CopyTo(new object[0], -1));
        }

        [Fact]
        public void
            CopyTo_ThrowsIfTheNumberOfElementsInTheQueueIsGreaterThanSpaceAvailableFromArrayIndexToTheEndOfArray_Test()
        {
            Assert.Throws<ArgumentException>(() => new PriorityQueue<int>(new[] {1, 2, 3}).CopyTo(new int[3], 1));
        }

        [Fact]
        public void Ctor_WithNoArguments_CreatesEmptyQueue_Test()
        {
            Assert.Empty(new PriorityQueue<int>());
        }

        [Fact]
        public void Ctor_WithComparer_ThrowsIfComparerIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>((IComparer<int>) null));
        }

        [Fact]
        public void Ctor_WithEnumerable_CopiesElementsFromGivenSequence_Test()
        {
            var expectedSequence = new[] {5, 4, 3, 2, 1};
            Assert.Equal(expectedSequence, new PriorityQueue<int>(expectedSequence));
        }

        [Fact]
        public void Ctor_WithCapacity_ThrowsIfCapacityIsLessThanZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PriorityQueue<int>(-1));
            Assert.DoesNotThrow(() => new PriorityQueue<int>(0));
        }

        [Fact]
        public void Ctor_WithCapacityAndComparer_ThrowsIfCapacityIsLessThanZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PriorityQueue<int>(-1, Comparer<int>.Default));
            Assert.DoesNotThrow(() => new PriorityQueue<int>(0, Comparer<int>.Default));
        }

        [Fact]
        public void Ctor_WithCapacityAndComparer_ThrowsIfComparerIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>(10, null));
        }

        [Fact]
        public void Ctor_WithSource_ThrowsIfSourceIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>((IEnumerable<int>) null));
        }

        [Fact]
        public void Ctor_WithSourceAndComparer_ThrowsIfSourceIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>((IEnumerable<int>) null));
        }

        [Fact]
        public void Ctor_WithSourceAndComparer_ThrowsIfComparerIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>(Enumerable.Empty<int>(), null));
        }

        [Fact]
        public void Ctor_WithCustomComparator_Test()
        {
            var priorityQueue = new PriorityQueue<int>(
                new[] {10, 1, 100},
                new InverseDefaultComparator<int>());

            AssertDequeue(priorityQueue, expectedDequeueOrder: new[] {1, 10, 100});
        }

        [Fact]
        public void Count_Test()
        {
            Assert.Equal(0, new PriorityQueue<int>().Count);
            Assert.Equal(5, new PriorityQueue<int>(new[] {1, 2, 3, 4, 5}).Count);
        }

        [Fact]
        public void Dequeue_Test()
        {
            AssertDequeue(expectedDequeueOrder: new[] {3, 2, 1}, enqueueItems: new[] {1, 2, 3});
            AssertDequeue(expectedDequeueOrder: new[] {100, 10, 1}, enqueueItems: new[] {100, 1, 10});
            AssertDequeue(expectedDequeueOrder: new[] {5}, enqueueItems: new[] {5});
        }

        [Fact]
        public void Dequeue_ThrowsIfPriorityQueueIsEmpty_Test()
        {
            Assert.Throws<InvalidOperationException>(() => new PriorityQueue<int>().Dequeue());
        }

        [Fact]
        public void Enqueue_Test()
        {
            var priorityQueue = new PriorityQueue<int>();

            priorityQueue.Enqueue(1);
            priorityQueue.Enqueue(2);
            priorityQueue.Enqueue(3);

            Assert.Equal(3, priorityQueue.Count);
            AssertDequeue(priorityQueue, expectedDequeueOrder: new[] {3, 2, 1});
        }

        [Fact]
        public void Enqueue_CanEnqueueItemsPastInitialCapacity_Test()
        {
            var initialCapacity = 10;
            var priorityQueue = new PriorityQueue<int>(initialCapacity);

            for (int i = 0; i < initialCapacity * 2; i++)
            {
                priorityQueue.Enqueue(i);
            }
        }

        [Fact]
        public void IsSynchronized_Test()
        {
            Assert.False(((ICollection) new PriorityQueue<int>()).IsSynchronized);
        }

        [Fact]
        public void Peek_Test()
        {
            var priorityQueue = new PriorityQueue<string>(new[] {"aaa", "bbb", "ccc"});

            Assert.Equal("ccc", priorityQueue.Peek());
            priorityQueue.Dequeue();
            Assert.Equal("bbb", priorityQueue.Peek());
            priorityQueue.Dequeue();
            Assert.Equal("aaa", priorityQueue.Peek());
        }

        [Fact]
        public void Peek_ThrowsIfPriorityQueueIsEmpty_Test()
        {
            Assert.Throws<InvalidOperationException>(() => new PriorityQueue<int>().Peek());
        }

        [Fact]
        public void SyncRoot_Test()
        {
            ICollection priorityQueue = new PriorityQueue<int>();
            Assert.Same(priorityQueue.SyncRoot, priorityQueue.SyncRoot);
        }

        [Fact]
        public void TrimExcess_Test()
        {
            var priorityQueue = new PriorityQueue<int>(initialCapacity: 100);
            var elements = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            foreach (var element in elements)
            {
                priorityQueue.Enqueue(element);
            }

            priorityQueue.TrimExcess();

            AssertDequeue(priorityQueue, expectedDequeueOrder: new[] {10, 9, 8, 7, 6, 5, 4, 3, 2, 1});
        }

        [Fact]
        public void Dequeue_WhenItemsAreOfTheSamePriorityThenItemsAreDequeudedInOrderOfInsertion_Test()
        {
            var item1 = new Item(1, "1");
            var item2 = new Item(2, "2");
            var item2a = new Item(2, "2a");
            var item2b = new Item(2, "2b");
            var item2c = new Item(2, "2c");
            var item3 = new Item(3, "3");
            var item4 = new Item(4, "4");
            var priorityQueue = new PriorityQueue<Item>();

            priorityQueue.Enqueue(item1);
            priorityQueue.Enqueue(item2);
            priorityQueue.Enqueue(item2a);
            priorityQueue.Enqueue(item2b);
            priorityQueue.Enqueue(item2c);
            priorityQueue.Enqueue(item3);
            priorityQueue.Enqueue(item4);

            Assert.Same(item1, priorityQueue.Dequeue());
            Assert.Same(item2, priorityQueue.Dequeue());
            Assert.Same(item2a, priorityQueue.Dequeue());
            Assert.Same(item2b, priorityQueue.Dequeue());
            Assert.Same(item2c, priorityQueue.Dequeue());
            Assert.Same(item3, priorityQueue.Dequeue());
            Assert.Same(item4, priorityQueue.Dequeue());
        }

        #endregion

        #region Private Methods

        private void AssertDequeue(IEnumerable<int> expectedDequeueOrder, IEnumerable<int> enqueueItems)
        {
            AssertDequeue(new PriorityQueue<int>(enqueueItems), expectedDequeueOrder);
        }

        private void AssertDequeue<T>(PriorityQueue<T> queue, IEnumerable<T> expectedDequeueOrder)
        {
            foreach (var expectedValue in expectedDequeueOrder)
            {
                Assert.Equal(expectedValue, queue.Dequeue());
            }
            Assert.Empty(queue);
        }

        #endregion

        #region InverseDefaultComparator

        private class InverseDefaultComparator<T> : IComparer<T>
        {
            #region IComparer<T> Members

            public int Compare(T x, T y)
            {
                return Comparer<T>.Default.Compare(x, y) * -1;
            }

            #endregion
        }

        #endregion

        #region Item

        [DebuggerDisplay("{_tag}")]
        private class Item : IComparable<Item>
        {
            private readonly int _priority;
            private readonly string _tag;

            public Item(int priority, string tag)
            {
                _priority = priority;
                _tag = tag;
            }

            public int CompareTo(Item other)
            {
                return other._priority.CompareTo(_priority);
            }
        }

        #endregion
    }

    public class PriorityQueueEnumeratorTests
    {
        #region Public Methods

        [Fact]
        public void Current_ThrowsIfRequestedBeforeMoveNext_Test()
        {
            Assert.Throws<InvalidOperationException>(() => new PriorityQueue<int>().GetEnumerator().Current);
        }

        [Fact]
        public void Current_DoesNotMovePositionOfEnumerator_Test()
        {
            var priorityQueue = new PriorityQueue<string>(new[] {"a", "b"});
            using (var enumerator = priorityQueue.GetEnumerator())
            {
                enumerator.MoveNext();
                Assert.Same(enumerator.Current, enumerator.Current);
            }
        }

        [Fact]
        public void GetEnumerator_Test()
        {
            var inputSequence = new[] {1, 2, 3, 4, 5};
            var priorityQueue = new PriorityQueue<int>(inputSequence);
            using (var enumerator = priorityQueue.GetEnumerator())
            {
                var index = inputSequence.Length;
                while (enumerator.MoveNext())
                {
                    Assert.Equal(inputSequence[--index], enumerator.Current);
                }
                Assert.False(enumerator.MoveNext());
            }
        }


        [Fact]
        public void MoveNext_ThrowsIfCollectionIsModified_Test()
        {
            var priorityQueue = new PriorityQueue<int>(new[] {1, 2});
            using (var enumerator = priorityQueue.GetEnumerator())
            {
                priorityQueue.Enqueue(3);
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }

        [Fact]
        public void Reset_Test()
        {
            var priorityQueue = new PriorityQueue<int>(new[] {1, 2});
            using (var enumerator = priorityQueue.GetEnumerator())
            {
                Action enumerate = () =>
                    {
                        enumerator.MoveNext();
                        Assert.Equal(2, enumerator.Current);
                        enumerator.MoveNext();
                        Assert.Equal(1, enumerator.Current);
                        Assert.False(enumerator.MoveNext());
                    };
                enumerate();
                enumerator.Reset();
                enumerate();
            }
        }

        [Fact]
        public void Reset_ThrowsIfCollectionIsModified_Test()
        {
            var priorityQueue = new PriorityQueue<int>(new[] {1, 2});
            using (var enumerator = priorityQueue.GetEnumerator())
            {
                priorityQueue.Enqueue(3);
                Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
            }
        }

        #endregion
    }
}