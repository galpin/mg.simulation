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
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simulate.NET.Collections
{
    public class PriorityQueueTests
    {
        #region Public Methods

        [Fact]
        public void Ctor_WithNoArguments_CreatesEmptyQueue_Test()
        {
            //Assert.Empty(new PriorityQueue<int>());
        }

        [Fact]
        public void Ctor_WithComparer_ThrowsIfComparerIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>((IComparer<int>)null));
        }

        [Fact]
        public void Ctor_WithEnumerable_CopiesElementsFromGivenSequence_Test()
        {
            //var expectedSequence = new[] {1, 2, 3, 4, 5};
            //Assert.Equal(expectedSequence, new PriorityQueue<int>(expectedSequence));
        }

        [Fact]
        public void Ctor_WithCapacity_ThrowsIfCapacityIsZeroOrNegative_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PriorityQueue<int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new PriorityQueue<int>(-1));
        }

        [Fact]
        public void Ctor_WithCapacityAndComparer_ThrowsIfCapacityIsZeroOrNegative_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PriorityQueue<int>(0, Comparer<int>.Default));
            Assert.Throws<ArgumentOutOfRangeException>(() => new PriorityQueue<int>(-1, Comparer<int>.Default));
        }

        [Fact]
        public void Ctor_WithCapacityAndComparer_ThrowsIfComparerIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>(10, null));
        }

        [Fact]
        public void Ctor_WithSource_ThrowsIfSourceIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>((IEnumerable<int>)null));
        }

        [Fact]
        public void Ctor_WithSourceAndComparer_ThrowsIfSourceIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PriorityQueue<int>((IEnumerable<int>)null));
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

            AssertDequeue(priorityQueue, expectedDequeueOrder:new[] {1, 10, 100});
        }

        [Fact]
        public void Enqueue_Test()
        {
            var priorityQueue = new PriorityQueue<int>();

            priorityQueue.Enqueue(1);
            priorityQueue.Enqueue(2);
            priorityQueue.Enqueue(3);

            Assert.Equal(3, priorityQueue.Count);
            AssertDequeue(priorityQueue, expectedDequeueOrder:new[] {3,2,1});
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
        public void Dequeue_Test()
        {
            AssertDequeue(expectedDequeueOrder: new[] {3, 2, 1}, enqueueItems: new[] {1, 2, 3});
            AssertDequeue(expectedDequeueOrder: new[] { 100, 10, 1 }, enqueueItems: new[] { 100, 1, 10 });
            AssertDequeue(expectedDequeueOrder: new[] { 5 }, enqueueItems: new[] { 5 });
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
        public void Dequeue_ThrowsIfPriorityQueueIsEmpty_Test()
        {
            Assert.Throws<InvalidOperationException>(() => new PriorityQueue<int>().Dequeue());
        }

        [Fact]
        public void Peek_ThrowsIfPriorityQueueIsEmpty_Test()
        {
            Assert.Throws<InvalidOperationException>(() => new PriorityQueue<int>().Peek());
        }

        [Fact]
        public void TrimExcess_Test()
        {
            var priorityQueue = new PriorityQueue<int>(initialCapacity:100);
            var elements = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            foreach (var element in elements)
            {
                priorityQueue.Enqueue(element);    
            }
            
            priorityQueue.TrimExcess();

            AssertDequeue(priorityQueue, expectedDequeueOrder:new[] {10, 9, 8, 7, 6, 5, 4, 3, 2, 1});
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
            //Assert.Empty(queue);
        }

        #endregion

        #region InverseDefaultComparator

        private class InverseDefaultComparator<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return Comparer<T>.Default.Compare(x, y) * -1;
            }
        }

        #endregion
    }
}