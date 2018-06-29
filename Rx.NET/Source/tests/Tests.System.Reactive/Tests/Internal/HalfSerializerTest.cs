// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;
using Xunit;

namespace ReactiveTests.Tests
{
    public class HalfSerializerTest
    {
        private int wip;
        private Exception error;
        private Consumer consumer = new Consumer();

        [Fact]
        public void HalfSerializer_OnNext()
        {
            HalfSerializer.ForwardOnNext(consumer, 1, ref wip, ref error);

            Assert.Equal(0, wip);
            Assert.Null(error);

            Assert.Equal(1, consumer.items.Count);
            Assert.Equal(1, consumer.items[0]);
            Assert.Equal(0, consumer.done);
            Assert.Null(consumer.exc);
        }

        [Fact]
        public void HalfSerializer_OnError()
        {
            var ex = new InvalidOperationException();

            HalfSerializer.ForwardOnError(consumer, ex, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            HalfSerializer.ForwardOnNext(consumer, 2, ref wip, ref error);

            Assert.Equal(0, consumer.items.Count);
            Assert.Equal(0, consumer.done);
            Assert.Equal(ex, consumer.exc);
        }

        [Fact]
        public void HalfSerializer_OnError_Ignore_Further_Events()
        {
            var ex = new InvalidOperationException();

            HalfSerializer.ForwardOnError(consumer, ex, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            HalfSerializer.ForwardOnNext(consumer, 2, ref wip, ref error);
            var ex2 = new NotSupportedException();
            HalfSerializer.ForwardOnError(consumer, ex2, ref wip, ref error);
            HalfSerializer.ForwardOnCompleted(consumer, ref wip, ref error);

            Assert.Equal(0, consumer.items.Count);
            Assert.Equal(0, consumer.done);
            Assert.Equal(ex, consumer.exc);
        }

        [Fact]
        public void HalfSerializer_OnCompleted()
        {
            HalfSerializer.ForwardOnCompleted(consumer, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            HalfSerializer.ForwardOnNext(consumer, 2, ref wip, ref error);

            Assert.Equal(0, consumer.items.Count);
            Assert.Equal(1, consumer.done);
            Assert.Null(consumer.exc);
        }

        [Fact]
        public void HalfSerializer_OnCompleted_Ignore_Further_Events()
        {
            HalfSerializer.ForwardOnCompleted(consumer, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            HalfSerializer.ForwardOnNext(consumer, 2, ref wip, ref error);
            var ex2 = new NotSupportedException();
            HalfSerializer.ForwardOnError(consumer, ex2, ref wip, ref error);
            HalfSerializer.ForwardOnCompleted(consumer, ref wip, ref error);

            Assert.Equal(0, consumer.items.Count);
            Assert.Equal(1, consumer.done);
            Assert.Null(consumer.exc);
        }

        // Practically simulates concurrent invocation of the HalfSerializer methods
        [Fact]
        public void HalfSerializer_OnNext_Reentrant_Error()
        {
            var c = new ReentrantConsumer(this, true);

            HalfSerializer.ForwardOnNext(c, 1, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            Assert.Equal(1, consumer.items.Count);
            Assert.Equal(1, consumer.items[0]);
            Assert.Equal(0, consumer.done);
            Assert.Equal(c.x, consumer.exc);
        }

        // Practically simulates concurrent invocation of the HalfSerializer methods
        [Fact]
        public void HalfSerializer_OnNext_Reentrant_OnCompleted()
        {
            var c = new ReentrantConsumer(this, false);

            HalfSerializer.ForwardOnNext(c, 1, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            Assert.Equal(1, consumer.items.Count);
            Assert.Equal(1, consumer.items[0]);
            Assert.Equal(1, consumer.done);
            Assert.Null(consumer.exc);
        }

        private sealed class Consumer : ISink<int>
        {
            internal List<int> items = new List<int>();

            internal int done;
            internal Exception exc;

            public void ForwardOnCompleted()
            {
                done++;
            }

            public void ForwardOnError(Exception error)
            {
                exc = error;
            }

            public void ForwardOnNext(int value)
            {
                items.Add(value);
            }
        }

        private sealed class ReentrantConsumer : ISink<int>
        {
            private readonly HalfSerializerTest parent;
            private readonly bool errorReenter;

            internal readonly Exception x = new IndexOutOfRangeException();

            public ReentrantConsumer(HalfSerializerTest parent, bool errorReenter)
            {
                this.parent = parent;
                this.errorReenter = errorReenter;
            }

            public void ForwardOnCompleted()
            {
                parent.consumer.ForwardOnCompleted();
            }

            public void ForwardOnError(Exception error)
            {
                parent.consumer.ForwardOnError(error);
            }

            public void ForwardOnNext(int value)
            {
                parent.consumer.ForwardOnNext(value);
                if (errorReenter)
                {
                    HalfSerializer.ForwardOnError(this, x, ref parent.wip, ref parent.error);
                }
                else
                {
                    HalfSerializer.ForwardOnCompleted(this, ref parent.wip, ref parent.error);
                }
            }
        }
    }
}
