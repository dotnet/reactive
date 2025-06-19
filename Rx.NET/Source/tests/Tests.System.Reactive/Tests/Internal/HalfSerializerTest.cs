﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class HalfSerializerTest
    {
#pragma warning disable IDE0044 // Spurious suggestion to add readonly. Not appropriate because this is passed by ref
        private int _wip;
#pragma warning restore IDE0044
        private Exception _error;
        private readonly Consumer _consumer = new();

        [TestMethod]
        public void HalfSerializer_OnNext()
        {
            HalfSerializer.ForwardOnNext(_consumer, 1, ref _wip, ref _error);

            Assert.Equal(0, _wip);
            Assert.Null(_error);

            Assert.Equal(1, _consumer.Items.Count);
            Assert.Equal(1, _consumer.Items[0]);
            Assert.Equal(0, _consumer.Done);
            Assert.Null(_consumer.Exc);
        }

        [TestMethod]
        public void HalfSerializer_OnError()
        {
            var ex = new InvalidOperationException();

            HalfSerializer.ForwardOnError(_consumer, ex, ref _wip, ref _error);

            Assert.Equal(1, _wip);
            Assert.Equal(_error, ExceptionHelper.Terminated);

            HalfSerializer.ForwardOnNext(_consumer, 2, ref _wip, ref _error);

            Assert.Equal(0, _consumer.Items.Count);
            Assert.Equal(0, _consumer.Done);
            Assert.Equal(ex, _consumer.Exc);
        }

        [TestMethod]
        public void HalfSerializer_OnError_Ignore_Further_Events()
        {
            var ex = new InvalidOperationException();

            HalfSerializer.ForwardOnError(_consumer, ex, ref _wip, ref _error);

            Assert.Equal(1, _wip);
            Assert.Equal(_error, ExceptionHelper.Terminated);

            HalfSerializer.ForwardOnNext(_consumer, 2, ref _wip, ref _error);
            var ex2 = new NotSupportedException();
            HalfSerializer.ForwardOnError(_consumer, ex2, ref _wip, ref _error);
            HalfSerializer.ForwardOnCompleted(_consumer, ref _wip, ref _error);

            Assert.Equal(0, _consumer.Items.Count);
            Assert.Equal(0, _consumer.Done);
            Assert.Equal(ex, _consumer.Exc);
        }

        [TestMethod]
        public void HalfSerializer_OnCompleted()
        {
            HalfSerializer.ForwardOnCompleted(_consumer, ref _wip, ref _error);

            Assert.Equal(1, _wip);
            Assert.Equal(_error, ExceptionHelper.Terminated);

            HalfSerializer.ForwardOnNext(_consumer, 2, ref _wip, ref _error);

            Assert.Equal(0, _consumer.Items.Count);
            Assert.Equal(1, _consumer.Done);
            Assert.Null(_consumer.Exc);
        }

        [TestMethod]
        public void HalfSerializer_OnCompleted_Ignore_Further_Events()
        {
            HalfSerializer.ForwardOnCompleted(_consumer, ref _wip, ref _error);

            Assert.Equal(1, _wip);
            Assert.Equal(_error, ExceptionHelper.Terminated);

            HalfSerializer.ForwardOnNext(_consumer, 2, ref _wip, ref _error);
            var ex2 = new NotSupportedException();
            HalfSerializer.ForwardOnError(_consumer, ex2, ref _wip, ref _error);
            HalfSerializer.ForwardOnCompleted(_consumer, ref _wip, ref _error);

            Assert.Equal(0, _consumer.Items.Count);
            Assert.Equal(1, _consumer.Done);
            Assert.Null(_consumer.Exc);
        }

        // Practically simulates concurrent invocation of the HalfSerializer methods
        [TestMethod]
        public void HalfSerializer_OnNext_Reentrant_Error()
        {
            var c = new ReentrantConsumer(this, true);

            HalfSerializer.ForwardOnNext(c, 1, ref _wip, ref _error);

            Assert.Equal(1, _wip);
            Assert.Equal(_error, ExceptionHelper.Terminated);

            Assert.Equal(1, _consumer.Items.Count);
            Assert.Equal(1, _consumer.Items[0]);
            Assert.Equal(0, _consumer.Done);
            Assert.Equal(c.X, _consumer.Exc);
        }

        // Practically simulates concurrent invocation of the HalfSerializer methods
        [TestMethod]
        public void HalfSerializer_OnNext_Reentrant_OnCompleted()
        {
            var c = new ReentrantConsumer(this, false);

            HalfSerializer.ForwardOnNext(c, 1, ref _wip, ref _error);

            Assert.Equal(1, _wip);
            Assert.Equal(_error, ExceptionHelper.Terminated);

            Assert.Equal(1, _consumer.Items.Count);
            Assert.Equal(1, _consumer.Items[0]);
            Assert.Equal(1, _consumer.Done);
            Assert.Null(_consumer.Exc);
        }

        private sealed class Consumer : ISink<int>
        {
            internal List<int> Items = [];

            internal int Done;
            internal Exception Exc;

            public void ForwardOnCompleted()
            {
                Done++;
            }

            public void ForwardOnError(Exception error)
            {
                Exc = error;
            }

            public void ForwardOnNext(int value)
            {
                Items.Add(value);
            }
        }

        private sealed class ReentrantConsumer : ISink<int>
        {
            private readonly HalfSerializerTest _parent;
            private readonly bool _errorReenter;

            internal readonly Exception X = new IndexOutOfRangeException();

            public ReentrantConsumer(HalfSerializerTest parent, bool errorReenter)
            {
                _parent = parent;
                _errorReenter = errorReenter;
            }

            public void ForwardOnCompleted()
            {
                _parent._consumer.ForwardOnCompleted();
            }

            public void ForwardOnError(Exception error)
            {
                _parent._consumer.ForwardOnError(error);
            }

            public void ForwardOnNext(int value)
            {
                _parent._consumer.ForwardOnNext(value);
                if (_errorReenter)
                {
                    HalfSerializer.ForwardOnError(this, X, ref _parent._wip, ref _parent._error);
                }
                else
                {
                    HalfSerializer.ForwardOnCompleted(this, ref _parent._wip, ref _parent._error);
                }
            }
        }
    }
}
