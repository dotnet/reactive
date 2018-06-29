// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class AutoConnectTest : ReactiveTest
    {
        [Fact]
        public void AutoConnect_Basic()
        {
            var called = 0;

            var source = Observable.Defer(() =>
            {
                called++;
                return Observable.Range(1, 5);
            })
            .Replay()
            .AutoConnect();

            Assert.Equal(0, called);

            var list = source.ToList().First();

            Assert.Equal(1, called);
            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5 }, list);

            list = source.ToList().First();

            Assert.Equal(1, called);
            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5 }, list);
        }

        [Fact]
        public void AutoConnect_Immediately()
        {
            var called = 0;

            var source = Observable.Defer(() =>
            {
                called++;
                return Observable.Range(1, 5);
            })
            .Replay()
            .AutoConnect(0);

            Assert.Equal(1, called);

            var list = source.ToList().First();

            Assert.Equal(1, called);
            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5 }, list);

            list = source.ToList().First();

            Assert.Equal(1, called);
            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5 }, list);
        }

        [Fact]
        public void AutoConnect_TwoConsumers()
        {
            var called = 0;

            var source = Observable.Defer(() =>
            {
                called++;
                return Observable.Range(1, 5);
            })
            .Replay()
            .AutoConnect(2);

            Assert.Equal(0, called);

            var list0 = new List<int>();

            source.Subscribe(v => list0.Add(v));

            Assert.Equal(0, called);
            Assert.Equal(0, list0.Count);

            var list = source.ToList().First();

            Assert.Equal(1, called);
            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5 }, list);

            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5 }, list0);

            list = source.ToList().First();

            Assert.Equal(1, called);
            Assert.Equal(new List<int>() { 1, 2, 3, 4, 5 }, list);
        }

        [Fact]
        public void AutoConnect_Dispose()
        {
            var subject = new Subject<int>();

            var disposable = new IDisposable[1];

            var source = subject
            .Replay()
            .AutoConnect(1, d => disposable[0] = d);

            Assert.Null(disposable[0]);

            var list = new List<int>();

            source.Subscribe(v => list.Add(v));

            Assert.NotNull(disposable[0]);

            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);

            disposable[0].Dispose();

            subject.OnNext(4);
            subject.OnNext(5);

            Assert.Equal(new List<int>() { 1, 2, 3 }, list);

        }
    }
}
