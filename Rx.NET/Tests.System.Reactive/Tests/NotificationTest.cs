// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class NotificationTest : ReactiveTest
    {
        #region ToObservable

        [TestMethod]
        public void ToObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Notification.CreateOnNext<int>(1).ToObservable(null));
        }

        [TestMethod]
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

        [TestMethod]
        public void ToObservable_Return()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Notification.CreateOnNext<int>(42).ToObservable(scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<int>(201, 42),
                OnCompleted<int>(201)
            );
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void Notifications_Equality()
        {
            var n = Notification.CreateOnNext(42);
            var e = Notification.CreateOnError<int>(new Exception());
            var c = Notification.CreateOnCompleted<int>();

            var n1 = n; var n2 = n;
            var e1 = e; var e2 = e;
            var c1 = c; var c2 = c;

            Assert.IsTrue(n1 == n2);
            Assert.IsTrue(e1 == e2);
            Assert.IsTrue(c1 == c2);

            Assert.IsTrue(n1.Equals(n2));
            Assert.IsTrue(e1.Equals(e2));
            Assert.IsTrue(c1.Equals(c2));
        }

        [TestMethod]
        public void OnNext_CtorAndProps()
        {
            var n = Notification.CreateOnNext<int>(42);
            Assert.AreEqual(NotificationKind.OnNext, n.Kind);
            Assert.IsTrue(n.HasValue);
            Assert.AreEqual(42, n.Value);
            Assert.IsNull(n.Exception);
        }

        [TestMethod]
        public void OnNext_Equality()
        {
            var n1 = Notification.CreateOnNext<int>(42);
            var n2 = Notification.CreateOnNext<int>(42);
            var n3 = Notification.CreateOnNext<int>(24);
            var n4 = Notification.CreateOnCompleted<int>();

            Assert.IsTrue(n1.Equals(n1));
            Assert.IsTrue(n1.Equals(n2));
            Assert.IsTrue(n2.Equals(n1));

            Assert.IsFalse(n1.Equals(null));
            Assert.IsFalse(n1.Equals(""));

            Assert.IsFalse(n1.Equals(n3));
            Assert.IsFalse(n3.Equals(n1));
            Assert.IsFalse(n1.Equals(n4));
            Assert.IsFalse(n4.Equals(n1));

            Assert.IsTrue(n1 == n2);
            Assert.IsTrue(n2 == n1);
            Assert.IsFalse(n1 == null);
            Assert.IsFalse(null == n1);
            Assert.IsTrue(!(n1 != n2));
            Assert.IsTrue(!(n2 != n1));
            Assert.IsFalse(!(n1 != null));
            Assert.IsFalse(!(null != n1));
        }

        [TestMethod]
        public void OnNext_GetHashCode()
        {
            var n1 = Notification.CreateOnNext<int>(42);
            var n2 = Notification.CreateOnNext<int>(42);

            Assert.AreNotEqual(0, n1.GetHashCode());
            Assert.AreEqual(n1.GetHashCode(), n2.GetHashCode());
        }

        [TestMethod]
        public void OnNext_ToString()
        {
            var n1 = Notification.CreateOnNext<int>(42);
            Assert.IsTrue(n1.ToString().Contains("OnNext"));
            Assert.IsTrue(n1.ToString().Contains(42.ToString()));
        }

        [TestMethod]
        public void OnNext_AcceptObserver()
        {
            var con = new CheckOnNextObserver();
            var n1 = Notification.CreateOnNext<int>(42);
            n1.Accept(con);

            Assert.AreEqual(42, con.Value);
        }

        class CheckOnNextObserver : IObserver<int>
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

        [TestMethod]
        public void OnNext_AcceptObserverWithResult()
        {
            var n1 = Notification.CreateOnNext<int>(42);
            var res = n1.Accept(new AcceptObserver(x => "OK", _ => { Assert.Fail(); return null; }, () => { Assert.Fail(); return null; }));

            Assert.AreEqual("OK", res);
        }

        [TestMethod]
        public void OnNext_AcceptObserverWithResult_Null()
        {
            var n1 = Notification.CreateOnNext<int>(42);
            ReactiveAssert.Throws<ArgumentNullException>(() => n1.Accept(default(IObserver<int, string>)));
        }

        [TestMethod]
        public void OnNext_AcceptAction()
        {
            var obs = false;

            var n1 = Notification.CreateOnNext<int>(42);
            n1.Accept(x => { obs = true; }, _ => { Assert.Fail(); }, () => { Assert.Fail(); });

            Assert.IsTrue(obs);
        }

        [TestMethod]
        public void OnNext_Accept_ArgumentChecking()
        {
            var n = Notification.CreateOnNext<int>(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default(IObserver<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default(Action<int>), ex => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, default(Action<Exception>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, ex => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(default(Func<int, string>), ex => "", () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(x => "", default(Func<Exception, string>), () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(x => "", ex => "", default(Func<string>)));
        }

        [TestMethod]
        public void OnNext_AcceptActionWithResult()
        {
            var n1 = Notification.CreateOnNext<int>(42);
            var res = n1.Accept(x => "OK", _ => { Assert.Fail(); return null; }, () => { Assert.Fail(); return null; });

            Assert.AreEqual("OK", res);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void OnError_CtorNull()
        {
            Notification.CreateOnError<int>(null);
        }

        [TestMethod]
        public void OnError_Accept_ArgumentChecking()
        {
            var n = Notification.CreateOnError<int>(new Exception());

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default(IObserver<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default(Action<int>), ex => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, default(Action<Exception>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, ex => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(default(Func<int, string>), ex => "", () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(x => "", default(Func<Exception, string>), () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(x => "", ex => "", default(Func<string>)));
        }

        [TestMethod]
        public void OnError_CtorAndProps()
        {
            var e = new Exception();
            var n = Notification.CreateOnError<int>(e);
            Assert.AreEqual(NotificationKind.OnError, n.Kind);
            Assert.IsFalse(n.HasValue);
            Assert.AreSame(e, n.Exception);

            try
            {
                var x = n.Value;
                Assert.Fail();
            }
            catch (Exception _e)
            {
                Assert.AreSame(e, _e);
            }
        }

        [TestMethod]
        public void OnError_Equality()
        {
            var ex1 = new Exception();
            var ex2 = new Exception();

            var n1 = Notification.CreateOnError<int>(ex1);
            var n2 = Notification.CreateOnError<int>(ex1);
            var n3 = Notification.CreateOnError<int>(ex2);
            var n4 = Notification.CreateOnCompleted<int>();

            Assert.IsTrue(n1.Equals(n1));
            Assert.IsTrue(n1.Equals(n2));
            Assert.IsTrue(n2.Equals(n1));

            Assert.IsFalse(n1.Equals(null));
            Assert.IsFalse(n1.Equals(""));

            Assert.IsFalse(n1.Equals(n3));
            Assert.IsFalse(n3.Equals(n1));
            Assert.IsFalse(n1.Equals(n4));
            Assert.IsFalse(n4.Equals(n1));

            Assert.IsTrue(n1 == n2);
            Assert.IsTrue(n2 == n1);
            Assert.IsFalse(n1 == null);
            Assert.IsFalse(null == n1);
            Assert.IsTrue(!(n1 != n2));
            Assert.IsTrue(!(n2 != n1));
            Assert.IsFalse(!(n1 != null));
            Assert.IsFalse(!(null != n1));
        }

        [TestMethod]
        public void OnError_GetHashCode()
        {
            var ex = new Exception();

            var n1 = Notification.CreateOnError<int>(ex);
            var n2 = Notification.CreateOnError<int>(ex);

            Assert.AreNotEqual(0, n1.GetHashCode());
            Assert.AreEqual(n1.GetHashCode(), n2.GetHashCode());
        }

        [TestMethod]
        public void OnError_ToString()
        {
            var ex = new Exception();

            var n1 = Notification.CreateOnError<int>(ex);
            Assert.IsTrue(n1.ToString().Contains("OnError"));
            Assert.IsTrue(n1.ToString().Contains(ex.GetType().Name)); // CHECK, no message?
        }

        [TestMethod]
        public void OnError_AcceptObserver()
        {
            var ex = new Exception();

            var obs = new CheckOnErrorObserver();

            var n1 = Notification.CreateOnError<int>(ex);
            n1.Accept(obs);

            Assert.AreSame(ex, obs.Error);
        }

        class CheckOnErrorObserver : IObserver<int>
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

        [TestMethod]
        public void OnError_AcceptObserverWithResult()
        {
            var ex = new Exception();

            var n1 = Notification.CreateOnError<int>(ex);
            var res = n1.Accept(new AcceptObserver(x => { Assert.Fail(); return null; }, _ => { Assert.AreSame(ex, _); return "OK"; }, () => { Assert.Fail(); return null; }));

            Assert.AreEqual("OK", res);
        }

        [TestMethod]
        public void OnError_AcceptObserverWithResult_Null()
        {
            var n1 = Notification.CreateOnError<int>(new Exception());
            ReactiveAssert.Throws<ArgumentNullException>(() => n1.Accept(default(IObserver<int, string>)));
        }

        [TestMethod]
        public void OnError_AcceptAction()
        {
            var ex = new Exception();

            var obs = false;

            var n1 = Notification.CreateOnError<int>(ex);
            n1.Accept(x => { Assert.Fail(); }, _ => { obs = true; }, () => { Assert.Fail(); });

            Assert.IsTrue(obs);
        }

        [TestMethod]
        public void OnError_AcceptActionWithResult()
        {
            var ex = new Exception();

            var n1 = Notification.CreateOnError<int>(ex);
            var res = n1.Accept(x => { Assert.Fail(); return null; }, x => "OK", () => { Assert.Fail(); return null; });

            Assert.AreEqual("OK", res);
        }

        [TestMethod]
        public void OnCompleted_CtorAndProps()
        {
            var n = Notification.CreateOnCompleted<int>();
            Assert.AreEqual(NotificationKind.OnCompleted, n.Kind);
            Assert.IsFalse(n.HasValue);
            Assert.IsNull(n.Exception);

            var ok = false;
            try
            {
                var x = n.Value;
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                ok = true;
            }

            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void OnCompleted_Accept_ArgumentChecking()
        {
            var n = Notification.CreateOnCompleted<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default(IObserver<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(default(Action<int>), ex => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, default(Action<Exception>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept(x => { }, ex => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(default(Func<int, string>), ex => "", () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(x => "", default(Func<Exception, string>), () => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => n.Accept<string>(x => "", ex => "", default(Func<string>)));
        }

        [TestMethod]
        public void OnCompleted_Equality()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            var n2 = Notification.CreateOnCompleted<int>();
            var n3 = Notification.CreateOnNext<int>(2);

            Assert.IsTrue(n1.Equals(n1));
            Assert.IsTrue(n1.Equals(n2));
            Assert.IsTrue(n2.Equals(n1));

            Assert.IsFalse(n1.Equals(null));
            Assert.IsFalse(n1.Equals(""));

            Assert.IsFalse(n1.Equals(n3));
            Assert.IsFalse(n3.Equals(n1));

            Assert.IsTrue(n1 == n2);
            Assert.IsTrue(n2 == n1);
            Assert.IsFalse(n1 == null);
            Assert.IsFalse(null == n1);
            Assert.IsTrue(!(n1 != n2));
            Assert.IsTrue(!(n2 != n1));
            Assert.IsFalse(!(n1 != null));
            Assert.IsFalse(!(null != n1));
        }

        [TestMethod]
        public void OnCompleted_GetHashCode()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            var n2 = Notification.CreateOnCompleted<int>();

            Assert.AreNotEqual(0, n1.GetHashCode());
            Assert.AreEqual(n1.GetHashCode(), n2.GetHashCode());
        }

        [TestMethod]
        public void OnCompleted_ToString()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            Assert.IsTrue(n1.ToString().Contains("OnCompleted"));
        }

        [TestMethod]
        public void OnCompleted_AcceptObserver()
        {
            var obs = new CheckOnCompletedObserver();

            var n1 = Notification.CreateOnCompleted<int>();
            n1.Accept(obs);

            Assert.IsTrue(obs.Completed);
        }

        class CheckOnCompletedObserver : IObserver<int>
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

        [TestMethod]
        public void OnCompleted_AcceptObserverWithResult()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            var res = n1.Accept(new AcceptObserver(x => { Assert.Fail(); return null; }, _ => { Assert.Fail(); return null; }, () => "OK"));

            Assert.AreEqual("OK", res);
        }

        [TestMethod]
        public void OnCompleted_AcceptObserverWithResult_Null()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            ReactiveAssert.Throws<ArgumentNullException>(() => n1.Accept(default(IObserver<int, string>)));
        }

        [TestMethod]
        public void OnCompleted_AcceptAction()
        {
            var obs = false;

            var n1 = Notification.CreateOnCompleted<int>();
            n1.Accept(x => { Assert.Fail(); }, _ => { Assert.Fail(); }, () => { obs = true; });

            Assert.IsTrue(obs);
        }

        [TestMethod]
        public void OnCompleted_AcceptActionWithResult()
        {
            var n1 = Notification.CreateOnCompleted<int>();
            var res = n1.Accept(x => { Assert.Fail(); return null; }, _ => { Assert.Fail(); return null; }, () => "OK");

            Assert.AreEqual("OK", res);
        }

        class AcceptObserver : IObserver<int, string>
        {
            private Func<int, string> _onNext;
            private Func<Exception, string> _onError;
            private Func<string> _onCompleted;

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
