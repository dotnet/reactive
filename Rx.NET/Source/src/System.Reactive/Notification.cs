﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reactive.Concurrency;

// Notification<T> defines Equals, ==, and !=, but not GetHashCode.
// This seems like an oversight, but changing this would technically be a breaking change,
// so we suprress the warnings.
#pragma warning disable CS0659
#pragma warning disable CS0661

namespace System.Reactive
{
    /// <summary>
    /// Indicates the type of a notification.
    /// </summary>
    public enum NotificationKind
    {
        /// <summary>
        /// Represents an OnNext notification.
        /// </summary>
        OnNext,

        /// <summary>
        /// Represents an OnError notification.
        /// </summary>
        OnError,

        /// <summary>
        /// Represents an OnCompleted notification.
        /// </summary>
        OnCompleted
    }

    /// <summary>
    /// Represents a notification to an observer.
    /// </summary>
    /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
    [Serializable]
    public abstract class Notification<T> : IEquatable<Notification<T>>
    {
        /// <summary>
        /// Default constructor used by derived types.
        /// </summary>
        protected internal Notification()
        {
        }

        /// <summary>
        /// Returns the value of an OnNext notification or throws an exception.
        /// </summary>
        public abstract T Value { get; }

        /// <summary>
        /// Returns a value that indicates whether the notification has a value.
        /// </summary>
        public abstract bool HasValue { get; }

        /// <summary>
        /// Returns the exception of an OnError notification or returns <c>null</c>.
        /// </summary>
        public abstract Exception? Exception { get; }

        /// <summary>
        /// Gets the kind of notification that is represented.
        /// </summary>
        public abstract NotificationKind Kind { get; }

        /// <summary>
        /// Represents an OnNext notification to an observer.
        /// </summary>
        [DebuggerDisplay("OnNext({Value})")]
        [Serializable]
        internal sealed class OnNextNotification : Notification<T>
        {
            /// <summary>
            /// Constructs a notification of a new value.
            /// </summary>
            public OnNextNotification(T value)
            {
                Value = value;
            }

            /// <summary>
            /// Returns the value of an OnNext notification.
            /// </summary>
            public override T Value { get; }

            /// <summary>
            /// Returns <c>null</c>.
            /// </summary>
            public override Exception? Exception => null;

            /// <summary>
            /// Returns <c>true</c>.
            /// </summary>
            public override bool HasValue => true;

            /// <summary>
            /// Returns <see cref="NotificationKind.OnNext"/>.
            /// </summary>
            public override NotificationKind Kind => NotificationKind.OnNext;

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            public override int GetHashCode() => Value?.GetHashCode() ?? 0;

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            public override bool Equals(Notification<T>? other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (other is null)
                {
                    return false;
                }

                if (other.Kind != NotificationKind.OnNext)
                {
                    return false;
                }

                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            /// <summary>
            /// Returns a string representation of this instance.
            /// </summary>
            public override string ToString() => string.Format(CultureInfo.CurrentCulture, "OnNext({0})", Value);

            /// <summary>
            /// Invokes the observer's method corresponding to the notification.
            /// </summary>
            /// <param name="observer">Observer to invoke the notification on.</param>
            public override void Accept(IObserver<T> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                observer.OnNext(Value);
            }

            /// <summary>
            /// Invokes the observer's method corresponding to the notification and returns the produced result.
            /// </summary>
            /// <param name="observer">Observer to invoke the notification on.</param>
            /// <returns>Result produced by the observation.</returns>
            public override TResult Accept<TResult>(IObserver<T, TResult> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                return observer.OnNext(Value);
            }

            /// <summary>
            /// Invokes the delegate corresponding to the notification.
            /// </summary>
            /// <param name="onNext">Delegate to invoke for an OnNext notification.</param>
            /// <param name="onError">Delegate to invoke for an OnError notification.</param>
            /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
            public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                if (onNext == null)
                {
                    throw new ArgumentNullException(nameof(onNext));
                }

                if (onError == null)
                {
                    throw new ArgumentNullException(nameof(onError));
                }

                if (onCompleted == null)
                {
                    throw new ArgumentNullException(nameof(onCompleted));
                }

                onNext(Value);
            }

            /// <summary>
            /// Invokes the delegate corresponding to the notification and returns the produced result.
            /// </summary>
            /// <param name="onNext">Delegate to invoke for an OnNext notification.</param>
            /// <param name="onError">Delegate to invoke for an OnError notification.</param>
            /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
            /// <returns>Result produced by the observation.</returns>
            public override TResult Accept<TResult>(Func<T, TResult> onNext, Func<Exception, TResult> onError, Func<TResult> onCompleted)
            {
                if (onNext == null)
                {
                    throw new ArgumentNullException(nameof(onNext));
                }

                if (onError == null)
                {
                    throw new ArgumentNullException(nameof(onError));
                }

                if (onCompleted == null)
                {
                    throw new ArgumentNullException(nameof(onCompleted));
                }

                return onNext(Value);
            }
        }

        /// <summary>
        /// Represents an OnError notification to an observer.
        /// </summary>
        [DebuggerDisplay("OnError({Exception})")]
        [Serializable]
        internal sealed class OnErrorNotification : Notification<T>
        {
            /// <summary>
            /// Constructs a notification of an exception.
            /// </summary>
            public OnErrorNotification(Exception exception)
            {
                Exception = exception;
            }

            /// <summary>
            /// Throws the exception.
            /// </summary>
            public override T Value { get { Exception.Throw(); return default!; } }

            /// <summary>
            /// Returns the exception.
            /// </summary>
            public override Exception Exception { get; }

            /// <summary>
            /// Returns <c>false</c>.
            /// </summary>
            public override bool HasValue => false;

            /// <summary>
            /// Returns <see cref="NotificationKind.OnError"/>.
            /// </summary>
            public override NotificationKind Kind => NotificationKind.OnError;

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            public override int GetHashCode() => Exception.GetHashCode();

            /// <summary>
            /// Indicates whether this instance and other are equal.
            /// </summary>
            public override bool Equals(Notification<T>? other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (other is null)
                {
                    return false;
                }

                if (other.Kind != NotificationKind.OnError)
                {
                    return false;
                }

                return Equals(Exception, other.Exception);
            }

            /// <summary>
            /// Returns a string representation of this instance.
            /// </summary>
            public override string ToString() => string.Format(CultureInfo.CurrentCulture, "OnError({0})", Exception.GetType().FullName);

            /// <summary>
            /// Invokes the observer's method corresponding to the notification.
            /// </summary>
            /// <param name="observer">Observer to invoke the notification on.</param>
            public override void Accept(IObserver<T> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                observer.OnError(Exception);
            }

            /// <summary>
            /// Invokes the observer's method corresponding to the notification and returns the produced result.
            /// </summary>
            /// <param name="observer">Observer to invoke the notification on.</param>
            /// <returns>Result produced by the observation.</returns>
            public override TResult Accept<TResult>(IObserver<T, TResult> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                return observer.OnError(Exception);
            }

            /// <summary>
            /// Invokes the delegate corresponding to the notification.
            /// </summary>
            /// <param name="onNext">Delegate to invoke for an OnNext notification.</param>
            /// <param name="onError">Delegate to invoke for an OnError notification.</param>
            /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
            public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                if (onNext == null)
                {
                    throw new ArgumentNullException(nameof(onNext));
                }

                if (onError == null)
                {
                    throw new ArgumentNullException(nameof(onError));
                }

                if (onCompleted == null)
                {
                    throw new ArgumentNullException(nameof(onCompleted));
                }

                onError(Exception);
            }

            /// <summary>
            /// Invokes the delegate corresponding to the notification and returns the produced result.
            /// </summary>
            /// <param name="onNext">Delegate to invoke for an OnNext notification.</param>
            /// <param name="onError">Delegate to invoke for an OnError notification.</param>
            /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
            /// <returns>Result produced by the observation.</returns>
            public override TResult Accept<TResult>(Func<T, TResult> onNext, Func<Exception, TResult> onError, Func<TResult> onCompleted)
            {
                if (onNext == null)
                {
                    throw new ArgumentNullException(nameof(onNext));
                }

                if (onError == null)
                {
                    throw new ArgumentNullException(nameof(onError));
                }

                if (onCompleted == null)
                {
                    throw new ArgumentNullException(nameof(onCompleted));
                }

                return onError(Exception);
            }
        }

        /// <summary>
        /// Represents an OnCompleted notification to an observer.
        /// </summary>
        [DebuggerDisplay("OnCompleted()")]
        [Serializable]
        internal sealed class OnCompletedNotification : Notification<T>
        {
            /// <summary>
            /// Complete notifications are stateless thus only one instance
            /// can ever exist per type.
            /// </summary>
            internal static readonly Notification<T> Instance = new OnCompletedNotification();

            /// <summary>
            /// Constructs a notification of the end of a sequence.
            /// </summary>
            private OnCompletedNotification()
            {
            }

            /// <summary>
            /// Throws an <see cref="InvalidOperationException"/>.
            /// </summary>
            public override T Value { get { throw new InvalidOperationException(Strings_Core.COMPLETED_NO_VALUE); } }

            /// <summary>
            /// Returns <c>null</c>.
            /// </summary>
            public override Exception? Exception => null;

            /// <summary>
            /// Returns <c>false</c>.
            /// </summary>
            public override bool HasValue => false;

            /// <summary>
            /// Returns <see cref="NotificationKind.OnCompleted"/>.
            /// </summary>
            public override NotificationKind Kind => NotificationKind.OnCompleted;

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            public override int GetHashCode() => typeof(T).GetHashCode() ^ 8510;

            /// <summary>
            /// Indicates whether this instance and other are equal.
            /// </summary>
            public override bool Equals(Notification<T>? other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (other is null)
                {
                    return false;
                }

                return other.Kind == NotificationKind.OnCompleted;
            }

            /// <summary>
            /// Returns a string representation of this instance.
            /// </summary>
            public override string ToString() => "OnCompleted()";

            /// <summary>
            /// Invokes the observer's method corresponding to the notification.
            /// </summary>
            /// <param name="observer">Observer to invoke the notification on.</param>
            public override void Accept(IObserver<T> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                observer.OnCompleted();
            }

            /// <summary>
            /// Invokes the observer's method corresponding to the notification and returns the produced result.
            /// </summary>
            /// <param name="observer">Observer to invoke the notification on.</param>
            /// <returns>Result produced by the observation.</returns>
            public override TResult Accept<TResult>(IObserver<T, TResult> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                return observer.OnCompleted();
            }

            /// <summary>
            /// Invokes the delegate corresponding to the notification.
            /// </summary>
            /// <param name="onNext">Delegate to invoke for an OnNext notification.</param>
            /// <param name="onError">Delegate to invoke for an OnError notification.</param>
            /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
            public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                if (onNext == null)
                {
                    throw new ArgumentNullException(nameof(onNext));
                }

                if (onError == null)
                {
                    throw new ArgumentNullException(nameof(onError));
                }

                if (onCompleted == null)
                {
                    throw new ArgumentNullException(nameof(onCompleted));
                }

                onCompleted();
            }

            /// <summary>
            /// Invokes the delegate corresponding to the notification and returns the produced result.
            /// </summary>
            /// <param name="onNext">Delegate to invoke for an OnNext notification.</param>
            /// <param name="onError">Delegate to invoke for an OnError notification.</param>
            /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
            /// <returns>Result produced by the observation.</returns>
            public override TResult Accept<TResult>(Func<T, TResult> onNext, Func<Exception, TResult> onError, Func<TResult> onCompleted)
            {
                if (onNext == null)
                {
                    throw new ArgumentNullException(nameof(onNext));
                }

                if (onError == null)
                {
                    throw new ArgumentNullException(nameof(onError));
                }

                if (onCompleted == null)
                {
                    throw new ArgumentNullException(nameof(onCompleted));
                }

                return onCompleted();
            }
        }

        /// <summary>
        /// Determines whether the current <see cref="Notification{T}"/> object has the same observer message payload as a specified <see cref="Notification{T}"/> value.
        /// </summary>
        /// <param name="other">An object to compare to the current <see cref="Notification{T}"/> object.</param>
        /// <returns><c>true</c> if both <see cref="Notification{T}"/> objects have the same observer message payload; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Equality of <see cref="Notification{T}"/> objects is based on the equality of the observer message payload they represent, including the notification Kind and the Value or Exception (if any).
        /// This means two <see cref="Notification{T}"/> objects can be equal even though they don't represent the same observer method call, but have the same Kind and have equal parameters passed to the observer method.
        /// In case one wants to determine whether two <see cref="Notification{T}"/> objects represent the same observer method call, use Object.ReferenceEquals identity equality instead.
        /// </remarks>
        public abstract bool Equals(Notification<T>? other);

        /// <summary>
        /// Determines whether the two specified <see cref="Notification{T}"/> objects have the same observer message payload.
        /// </summary>
        /// <param name="left">The first <see cref="Notification{T}"/> to compare, or <c>null</c>.</param>
        /// <param name="right">The second <see cref="Notification{T}"/> to compare, or <c>null</c>.</param>
        /// <returns><c>true</c> if the first <see cref="Notification{T}"/> value has the same observer message payload as the second <see cref="Notification{T}"/> value; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Equality of <see cref="Notification{T}"/> objects is based on the equality of the observer message payload they represent, including the notification Kind and the Value or Exception (if any).
        /// This means two <see cref="Notification{T}"/> objects can be equal even though they don't represent the same observer method call, but have the same Kind and have equal parameters passed to the observer method.
        /// In case one wants to determine whether two <see cref="Notification{T}"/> objects represent the same observer method call, use Object.ReferenceEquals identity equality instead.
        /// </remarks>
        public static bool operator ==(Notification<T> left, Notification<T> right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether the two specified <see cref="Notification{T}"/> objects have a different observer message payload.
        /// </summary>
        /// <param name="left">The first <see cref="Notification{T}"/> to compare, or <c>null</c>.</param>
        /// <param name="right">The second <see cref="Notification{T}"/> to compare, or <c>null</c>.</param>
        /// <returns><c>true</c> if the first <see cref="Notification{T}"/> value has a different observer message payload as the second <see cref="Notification{T}"/> value; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Equality of <see cref="Notification{T}"/> objects is based on the equality of the observer message payload they represent, including the notification Kind and the Value or Exception (if any).
        /// This means two <see cref="Notification{T}"/> objects can be equal even though they don't represent the same observer method call, but have the same Kind and have equal parameters passed to the observer method.
        /// In case one wants to determine whether two <see cref="Notification{T}"/> objects represent a different observer method call, use Object.ReferenceEquals identity equality instead.
        /// </remarks>
        public static bool operator !=(Notification<T> left, Notification<T> right) => !(left == right);

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current <see cref="Notification{T}"/>.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current <see cref="Notification{T}"/>.</param>
        /// <returns><c>true</c> if the specified System.Object is equal to the current <see cref="Notification{T}"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Equality of <see cref="Notification{T}"/> objects is based on the equality of the observer message payload they represent, including the notification Kind and the Value or Exception (if any).
        /// This means two <see cref="Notification{T}"/> objects can be equal even though they don't represent the same observer method call, but have the same Kind and have equal parameters passed to the observer method.
        /// In case one wants to determine whether two <see cref="Notification{T}"/> objects represent the same observer method call, use Object.ReferenceEquals identity equality instead.
        /// </remarks>
        public override bool Equals(object? obj) => Equals(obj as Notification<T>);

        /// <summary>
        /// Invokes the observer's method corresponding to the notification.
        /// </summary>
        /// <param name="observer">Observer to invoke the notification on.</param>
        public abstract void Accept(IObserver<T> observer);

        /// <summary>
        /// Invokes the observer's method corresponding to the notification and returns the produced result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned from the observer's notification handlers.</typeparam>
        /// <param name="observer">Observer to invoke the notification on.</param>
        /// <returns>Result produced by the observation.</returns>
        public abstract TResult Accept<TResult>(IObserver<T, TResult> observer);

        /// <summary>
        /// Invokes the delegate corresponding to the notification.
        /// </summary>
        /// <param name="onNext">Delegate to invoke for an OnNext notification.</param>
        /// <param name="onError">Delegate to invoke for an OnError notification.</param>
        /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
        public abstract void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted);

        /// <summary>
        /// Invokes the delegate corresponding to the notification and returns the produced result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned from the notification handler delegates.</typeparam>
        /// <param name="onNext">Delegate to invoke for an OnNext notification.</param>
        /// <param name="onError">Delegate to invoke for an OnError notification.</param>
        /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
        /// <returns>Result produced by the observation.</returns>
        public abstract TResult Accept<TResult>(Func<T, TResult> onNext, Func<Exception, TResult> onError, Func<TResult> onCompleted);

        /// <summary>
        /// Returns an observable sequence with a single notification, using the immediate scheduler.
        /// </summary>
        /// <returns>The observable sequence that surfaces the behavior of the notification upon subscription.</returns>
        public IObservable<T> ToObservable() => ToObservable(ImmediateScheduler.Instance);

        /// <summary>
        /// Returns an observable sequence with a single notification.
        /// </summary>
        /// <param name="scheduler">Scheduler to send out the notification calls on.</param>
        /// <returns>The observable sequence that surfaces the behavior of the notification upon subscription.</returns>
        public IObservable<T> ToObservable(IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new NotificationToObservable(scheduler, this);
        }

        private sealed class NotificationToObservable : ObservableBase<T>
        {
            private readonly IScheduler _scheduler;
            private readonly Notification<T> _parent;

            public NotificationToObservable(IScheduler scheduler, Notification<T> parent)
            {
                _scheduler = scheduler;
                _parent = parent;
            }

            protected override IDisposable SubscribeCore(IObserver<T> observer)
            {
                return _scheduler.ScheduleAction((_parent, observer), state =>
                {
                    var parent = state._parent;
                    var o = state.observer;

                    parent.Accept(o);

                    if (parent.Kind == NotificationKind.OnNext)
                    {
                        o.OnCompleted();
                    }
                });
            }
        }
    }

    /// <summary>
    /// Provides a set of static methods for constructing notifications.
    /// </summary>
    public static class Notification
    {
        /// <summary>
        /// Creates an object that represents an OnNext notification to an observer.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer. Upon dematerialization of the notifications into an observable sequence, this type is used as the element type for the sequence.</typeparam>
        /// <param name="value">The value contained in the notification.</param>
        /// <returns>The OnNext notification containing the value.</returns>
        public static Notification<T> CreateOnNext<T>(T value)
        {
            return new Notification<T>.OnNextNotification(value);
        }

        /// <summary>
        /// Creates an object that represents an OnError notification to an observer.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer. Upon dematerialization of the notifications into an observable sequence, this type is used as the element type for the sequence.</typeparam>
        /// <param name="error">The exception contained in the notification.</param>
        /// <returns>The OnError notification containing the exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public static Notification<T> CreateOnError<T>(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            return new Notification<T>.OnErrorNotification(error);
        }

        /// <summary>
        /// Creates an object that represents an OnCompleted notification to an observer.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer. Upon dematerialization of the notifications into an observable sequence, this type is used as the element type for the sequence.</typeparam>
        /// <returns>The OnCompleted notification.</returns>
        public static Notification<T> CreateOnCompleted<T>()
        {
            return Notification<T>.OnCompletedNotification.Instance;
        }
    }
}

#pragma warning restore CS0659
#pragma warning restore CS0661
