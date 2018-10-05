// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class NotificationTest : ReactiveTest
    {
        #region ToObservable

        [Fact]
        public void ToObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Notification.CreateOnNext(1).ToObservable(null));
        }

        [Fact]
        public void ToObservable_Empty()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Notification.CreateOnCompleted<int>().ToObservable(scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
        }

        [Fact]
        public void ToObservable_Return()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Notification.CreateOnNext(42).ToObservable(scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnCompleted<int>(201)
            );
        }

        [Fact]
        public void ToObservable_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Notification.CreateOnError<int>(ex).ToObservable(scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void ToObservable_CurrentThread()
        {
            var evt = new ManualResetEvent(false);

            Notification.CreateOnCompleted<int>().ToObservable().Subscribe(_ => { }, () =>
            {
                evt.Set();
            });

            evt.WaitOne();
        }

        #endregion

        [Fact]
        public void Notifications_Equality()
        {
            var n = Notification.CreateOnNext(42);
            var e = Notification.CreateOnError<int>(new Exception());
            var c = Notification.CreateOnCompleted<int>();

            var n1 = n; var n2 = n;
            var e1 = e; var e2 = e;
            var c1 = c; var c2 = c;

            Assert.True(n1 == n2);
            Assert.True(e1 == e2);
            Assert.True(c1 == c2);

            Assert.True(n1.Equals(n2));
            Assert.True(e1.Equals(e2));
            Assert.True(c1.Equals(c2));
        }

        [Fact]
        public void OnNext_CtorAndProps()
        {
            var n = Notification.CreateOnNext(42);
            Assert.Equal(NotificationKind.OnNext, n.Kind);
            Assert.True(n.HasValue);
            Assert.Equal(42, n.Value);
            Assert.Null(n.Exception);
        }

        [Fact]
        public void OnNext_Equality()
        {
            var n1 = Notification.CreateOnNext(42);
            var n2 = Notification.CreateOnNext(42);
            var n3 = Notification.CreateOnNext(24);
            var n4 = Notification.CreateOnCompleted<int>();

            Assert.True(n1.Equals(n1));
            Assert.True(n1.Equals(n2));
            Assert.True(n2.Equals(n1));

            Assert.False(n1.Equals(null));
            Assert.False(n1.Equals(""));

            Assert.False(n1.Equals(n3));
            Assert.False(n3.Equals(n1));
            Assert.False(n1.Equals(n4));
            Assert.False(n4.Equals(n1));

            Assert.True(n1 == n2);
            Assert.True(n2 == n1);
            Assert.False(n1 == null);
            Assert.False(null == n1);
            Assert.True(!(n1 != n2));
            Assert.True(!(n2 != n1));
            Assert.False(!(n1 != null));
            Assert.False(!(null != n1));
        }

        [Fact]
        public void OnNext_GetHashCode()
        {
            var n1 = Notification.CreateOnNext(42);
            var n2 = Notification.CreateOnNext(42);

            Assert.NotEqual(0, n1.GetHashCode());
            Assert.Equal(n1.GetHashCode(), n2.GetHashCode());
        }

        [Fact]
        public void OnNext_ToString()
        {
            var n1 = Notification.CreateOnNext(42);
            Assert.True(n1.ToString().Contains("OnNext"));
            Assert.True(n1.ToString().Contains(42.ToString()));
        }

        [Fact]
        public void OnNext_AcceptObserver()
        {
            var con = new CheckOnNextObserver();
            var n1 = Notification.CreateOnNext(42);
            n1.Accept(con);

            Assert.Equal(42, con.Value);
        }

        private class CheckOnNextObserver : IObserver<int>
        {
            public void OnNext(int value)
            {
                Value = value;
            }

            public int Value { get; private set; }

            public void OnError(Exception exception)
            {
                throw new NotImplementedException();
            }

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void OnNext_AcceptObserverWithResult()
        {
            var n1 = Notification.CreateOnNext(42);
            var res = n1.Accept(new AcceptObserver(x => "OK", _ => { Assert.True(false); return null; }, () => { Assert.True(false); return null; }));

            Assert.Equal("OK", res);
        }

        [Fact]
        public void OnNext_AcceptObserverWithResult_Null()
        {
            var n1 = Notification.CreateOnNext(42);
            ReactiveAssert.Throws<ArgumentNullException>(() => n1.Accept(default(IObserver<int, string>)));
        }

        [Fact]
        public void OnNext_AcceptAction()
        {
            var obs = false;

            var n1 = Notification.CreateOnNext(42);
            n1.Accept(x => { obs = true; }, _ => { Assert.True(false); }, () => { Assert.True(false); });

            Assert.True(obs);
        }

        [Fact]
        public void OnNext_Accept_ArgumentChecking()
        {
            var n = Notification.CreateOnNext(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default, ex => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, ex => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default, ex => "", () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => "", default, () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => "", ex => "", default));
        }

        [Fact]
        public void OnNext_AcceptActionWithResult()
        {
            var n1 = Notification.CreateOnNext(42);
            var res = n1.Accept(x => "OK", _ => { Assert.True(false); return null; }, () => { Assert.True(false); return null; });

            Assert.Equal("OK", res);
        }

        [Fact]
        public void OnError_CtorNull()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Notification.CreateOnError<int>(null));
        }

        [Fact]
        public void OnError_Accept_ArgumentChecking()
        {
            var n = Notification.CreateOnError<int>(new Exception());

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default, ex => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, ex => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default, ex => "", () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => "", default, () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => "", ex => "", default));
        }

        [Fact]
        public void OnError_CtorAndProps()
        {
            var e = new Exception();
            var n = Notification.CreateOnError<int>(e);
            Assert.Equal(NotificationKind.OnError, n.Kind);
            Assert.False(n.HasValue);
            Assert.Same(e, n.Exception);

            try
            {
                var x = n.Value;
                Assert.True(false);
            }
            catch (Exception _e)
            {
                Assert.Same(e, _e);
            }
        }

        [Fact]
        public void OnError_Equality()
        {
            var ex1 = new Exception();
            var ex2 = new Exception();

            var n1 = Notification.CreateOnError<int>(ex1);
            var n2 = Notification.CreateOnError<int>(ex1);
            var n3 = Notification.CreateOnError<int>(ex2);
            var n4 = Notification.CreateOnCompleted<int>();

            Assert.True(n1.Equals(n1));
            Assert.True(n1.Equals(n2));
            Assert.True(n2.Equals(n1));

            Assert.False(n1.Equals(null));
            Assert.False(n1.Equals(""));

            Assert.False(n1.Equals(n3));
            Assert.False(n3.Equals(n1));
            Assert.False(n1.Equals(n4));
            Assert.False(n4.Equals(n1));

            Assert.True(n1 == n2);
            Assert.True(n2 == n1);
            Assert.False(n1 == null);
            Assert.False(null == n1);
            Assert.True(!(n1 != n2));
            Assert.True(!(n2 != n1));
            Assert.False(!(n1 != null));
            Assert.False(!(null != n1));
        }

        [Fact]
        public void OnError_GetHashCode()
        {
            var ex = new Exception();

            var n1 = Notification.CreateOnError<int>(ex);
            var n2 = Notification.CreateOnError<int>(ex);

            Assert.NotEqual(0, n1.GetHashCode());
            Assert.Equal(n1.GetHashCode(), n2.GetHashCode());
        }

        [Fact]
        public void OnError_ToString()
        {
            var ex = new Exception();

            var n1 = Notification.CreateOnError<int>(ex);
            Assert.True(n1.ToString().Contains("OnError"));
            Assert.True(n1.ToString().Contains(ex.GetType().Name)); // CHECK, no message?
        }

        [Fact]
        public void OnError_AcceptObserver()
        {
            var ex = new Exception();

            var obs = new CheckOnErrorObserver();

            var n1 = Notification.CreateOnError<int>(ex);
            n1.Accept(obs);

            Assert.Same(ex, obs.Error);
        }

        private class CheckOnErrorObserver : IObserver<int>
        {
            public void OnNext(int value)
            {
                throw new NotImplementedException();
            }

            public Exception Error { get; private set; }

            public void OnError(Exception exception)
            {
                Error = exception;
            }

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void OnError_AcceptObserverWithResult()
        {
            var ex = new Exception();

            var n1 = Notification.CreateOnError<int>(ex);
            var res = n1.Accept(new AcceptObserver(x => { Assert.True(false); return null; }, _ => { Assert.Same(ex, _); return "OK"; }, () => { Assert.True(false); return null; }));

            Assert.Equal("OK", res);
        }

        [Fact]
        public void OnError_AcceptObserverWithResult_Null()
        {
            var n1 = Notification.CreateOnError<int>(new Exception());
            ReactiveAssert.Throws<ArgumentNullException>(() => n1.Accept(default(IObserver<int, string>)));
        }

        [Fact]
        public void OnError_AcceptAction()
        {
            var ex = new Exception();

            var obs = false;

            var n1 = Notification.CreateOnError<int>(ex);
            n1.Accept(x => { Assert.True(false); }, _ => { obs = true; }, () => { Assert.True(false); });

            Assert.True(obs);
        }

        [Fact]
        public void OnError_AcceptActionWithResult()
        {
            var ex = new Exception();

            var n1 = Notification.CreateOnError<int>(ex);
            var res = n1.Accept(x => { Assert.True(false); return null; }, x => "OK", () => { Assert.True(false); return null; });

            Assert.Equal("OK", res);
        }

        [Fact]
        public void OnCompleted_CtorAndProps()
        {
            var n = Notification.CreateOnCompleted<int>();
            Assert.Equal(NotificationKind.OnCompleted, n.Kind);
            Assert.False(n.HasValue);
            Assert.Null(n.Exception);

            var ok = false;
            try
            {
                var x = n.Value;
                Assert.True(false);
            }
            catch (InvalidOperationException)
            {
                ok = true;
            }

            Assert.True(ok);
        }

        [Fact]
        public void OnCompleted_Accept_ArgumentChecking()
        {
            var n = Notification.CreateOnCompleted<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default, ex => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, ex => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default, ex => "", () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => "", default, () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => "", ex => "", default));
        }

        [Fact]
        public void OnCompleted_Equality()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            var n2 = Notification.CreateOnCompleted<int>();
            var n3 = Notification.CreateOnNext(2);

            Assert.True(n1.Equals(n1));
            Assert.True(n1.Equals(n2));
            Assert.True(n2.Equals(n1));

            Assert.False(n1.Equals(null));
            Assert.False(n1.Equals(""));

            Assert.False(n1.Equals(n3));
            Assert.False(n3.Equals(n1));

            Assert.True(n1 == n2);
            Assert.True(n2 == n1);
            Assert.False(n1 == null);
            Assert.False(null == n1);
            Assert.True(!(n1 != n2));
            Assert.True(!(n2 != n1));
            Assert.False(!(n1 != null));
            Assert.False(!(null != n1));
        }

        [Fact]
        public void OnCompleted_GetHashCode()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            var n2 = Notification.CreateOnCompleted<int>();

            Assert.NotEqual(0, n1.GetHashCode());
            Assert.Equal(n1.GetHashCode(), n2.GetHashCode());
        }

        [Fact]
        public void OnCompleted_ToString()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            Assert.True(n1.ToString().Contains("OnCompleted"));
        }

        [Fact]
        public void OnCompleted_AcceptObserver()
        {
            var obs = new CheckOnCompletedObserver();

            var n1 = Notification.CreateOnCompleted<int>();
            n1.Accept(obs);

            Assert.True(obs.Completed);
        }

        private class CheckOnCompletedObserver : IObserver<int>
        {
            public void OnNext(int value)
            {
                throw new NotImplementedException();
            }

            public bool Completed { get; private set; }

            public void OnError(Exception exception)
            {
                throw new NotImplementedException();
            }

            public void OnCompleted()
            {
                Completed = true;
            }
        }

        [Fact]
        public void OnCompleted_AcceptObserverWithResult()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            var res = n1.Accept(new AcceptObserver(x => { Assert.True(false); return null; }, _ => { Assert.True(false); return null; }, () => "OK"));

            Assert.Equal("OK", res);
        }

        [Fact]
        public void OnCompleted_AcceptObserverWithResult_Null()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            ReactiveAssert.Throws<ArgumentNullException>(() => n1.Accept(default(IObserver<int, string>)));
        }

        [Fact]
        public void OnCompleted_AcceptAction()
        {
            var obs = false;

            var n1 = Notification.CreateOnCompleted<int>();
            n1.Accept(x => { Assert.True(false); }, _ => { Assert.True(false); }, () => { obs = true; });

            Assert.True(obs);
        }

        [Fact]
        public void OnCompleted_AcceptActionWithResult()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            var res = n1.Accept(x => { Assert.True(false); return null; }, _ => { Assert.True(false); return null; }, () => "OK");

            Assert.Equal("OK", res);
        }

        private class AcceptObserver : IObserver<int, string>
        {
            private readonly Func<int, string> _onNext;
            private readonly Func<Exception, string> _onError;
            private readonly Func<string> _onCompleted;

            public AcceptObserver(Func<int, string> onNext, Func<Exception, string> onError, Func<string> onCompleted)
            {
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            public string OnNext(int value)
            {
                return _onNext(value);
            }

            public string OnError(Exception exception)
            {
                return _onError(exception);
            }

            public string OnCompleted()
            {
                return _onCompleted();
            }
        }
    }
}
