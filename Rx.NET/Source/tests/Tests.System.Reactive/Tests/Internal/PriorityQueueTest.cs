// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reactive;
using Xunit;

namespace ReactiveTests.Tests
{
    public class PriorityQueueTest
    {
        [Fact]
        public void Enqueue_dequeue()
        {
            var q = new PriorityQueue<int>();

            for (var i = 0; i < 16; i++)
            {
                Assert.Equal(0, q.Count);

                q.Enqueue(i);

                Assert.Equal(1, q.Count);
                Assert.Equal(i, q.Peek());
                Assert.Equal(1, q.Count);
                Assert.Equal(i, q.Dequeue());
                Assert.Equal(0, q.Count);
            }
        }

        [Fact]
        public void Enqueue_all_dequeue_all()
        {
            var q = new PriorityQueue<int>();

            for (var i = 0; i < 33; i++)
            {
                q.Enqueue(i);
                Assert.Equal(i + 1, q.Count);
            }

            Assert.Equal(33, q.Count);

            for (var i = 0; i < 33; i++)
            {
                Assert.Equal(33 - i, q.Count);
                Assert.Equal(i, q.Peek());
                Assert.Equal(i, q.Dequeue());
            }

            Assert.Equal(0, q.Count);
        }

        [Fact]
        public void Reverse_Enqueue_all_dequeue_all()
        {
            var q = new PriorityQueue<int>();

            for (var i = 32; i >= 0; i--)
            {
                q.Enqueue(i);
                Assert.Equal(33 - i, q.Count);
            }

            Assert.Equal(33, q.Count);

            for (var i = 0; i < 33; i++)
            {
                Assert.Equal(33 - i, q.Count);
                Assert.Equal(i, q.Peek());
                Assert.Equal(i, q.Dequeue());
            }

            Assert.Equal(0, q.Count);
        }

        [Fact]
        public void Remove_from_middle()
        {
            var q = new PriorityQueue<int>();

            for (var i = 0; i < 33; i++)
            {
                q.Enqueue(i);
            }

            q.Remove(16);

            for (var i = 0; i < 16; i++)
            {
                Assert.Equal(i, q.Dequeue());
            }

            for (var i = 16; i < 32; i++)
            {
                Assert.Equal(i + 1, q.Dequeue());
            }
        }

        [Fact]
        public void Repro_329()
        {
            var queue = new PriorityQueue<int>();

            queue.Enqueue(2);
            queue.Enqueue(1);
            queue.Enqueue(5);
            queue.Enqueue(2);

            Assert.Equal(1, queue.Dequeue());
            Assert.Equal(2, queue.Dequeue());
            Assert.Equal(2, queue.Dequeue());
            Assert.Equal(5, queue.Dequeue());
        }
    }
}
