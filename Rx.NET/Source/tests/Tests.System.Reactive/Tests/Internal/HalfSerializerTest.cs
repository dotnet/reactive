// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using Xunit;
using System;
using System.Collections.Generic;

namespace ReactiveTests.Tests
{
    
    public class HalfSerializerTest
    {
        int wip;

        Exception error;

        Consumer consumer = new Consumer();

        [Fact]
        public void HalfSerializer_OnNext()
        {
            HalfSerializer.OnNext(consumer, 1, ref wip, ref error);

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

            HalfSerializer.OnError(consumer, ex, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            HalfSerializer.OnNext(consumer, 2, ref wip, ref error);

            Assert.Equal(0, consumer.items.Count);
            Assert.Equal(0, consumer.done);
            Assert.Equal(ex, consumer.exc);
        }

        [Fact]
        public void HalfSerializer_OnError_Ignore_Further_Events()
        {
            var ex = new InvalidOperationException();

            HalfSerializer.OnError(consumer, ex, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            HalfSerializer.OnNext(consumer, 2, ref wip, ref error);
            var ex2 = new NotSupportedException();
            HalfSerializer.OnError(consumer, ex2, ref wip, ref error);
            HalfSerializer.OnCompleted(consumer, ref wip, ref error);

            Assert.Equal(0, consumer.items.Count);
            Assert.Equal(0, consumer.done);
            Assert.Equal(ex, consumer.exc);
        }

        [Fact]
        public void HalfSerializer_OnCompleted()
        {
            HalfSerializer.OnCompleted(consumer, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            HalfSerializer.OnNext(consumer, 2, ref wip, ref error);

            Assert.Equal(0, consumer.items.Count);
            Assert.Equal(1, consumer.done);
            Assert.Null(consumer.exc);
        }

        [Fact]
        public void HalfSerializer_OnCompleted_Ignore_Further_Events()
        {
            HalfSerializer.OnCompleted(consumer, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            HalfSerializer.OnNext(consumer, 2, ref wip, ref error);
            var ex2 = new NotSupportedException();
            HalfSerializer.OnError(consumer, ex2, ref wip, ref error);
            HalfSerializer.OnCompleted(consumer, ref wip, ref error);

            Assert.Equal(0, consumer.items.Count);
            Assert.Equal(1, consumer.done);
            Assert.Null(consumer.exc);
        }

        // Practically simulates concurrent invocation of the HalfSerializer methods
        [Fact]
        public void HalfSerializer_OnNext_Reentrant_Error()
        {
            var c = new ReentrantConsumer(this, true);

            HalfSerializer.OnNext(c, 1, ref wip, ref error);

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

            HalfSerializer.OnNext(c, 1, ref wip, ref error);

            Assert.Equal(1, wip);
            Assert.Equal(error, ExceptionHelper.Terminated);

            Assert.Equal(1, consumer.items.Count);
            Assert.Equal(1, consumer.items[0]);
            Assert.Equal(1, consumer.done);
            Assert.Null(consumer.exc);
        }

        sealed class Consumer : IObserver<int>
        {
            internal List<int> items = new List<int>();

            internal int done;
            internal Exception exc;

            public void OnCompleted()
            {
                done++;
            }

            public void OnError(Exception error)
            {
                exc = error;
            }

            public void OnNext(int value)
            {
                items.Add(value);
            }
        }

        sealed class ReentrantConsumer : IObserver<int>
        {
            readonly HalfSerializerTest parent;

            readonly bool errorReenter;

            internal readonly Exception x = new IndexOutOfRangeException();

            public ReentrantConsumer(HalfSerializerTest parent, bool errorReenter)
            {
                this.parent = parent;
                this.errorReenter = errorReenter;
            }

            public void OnCompleted()
            {
                parent.consumer.OnCompleted();
            }

            public void OnError(Exception error)
            {
                parent.consumer.OnError(error);
            }

            public void OnNext(int value)
            {
                parent.consumer.OnNext(value);
                if (errorReenter)
                {
                    HalfSerializer.OnError(this, x, ref parent.wip, ref parent.error);
                } else
                {
                    HalfSerializer.OnCompleted(this, ref parent.wip, ref parent.error);
                }
            }
        }
    }
}
