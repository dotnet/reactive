[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
namespace Microsoft.Reactive.Testing
{
    public interface ITestableObservable<T> : System.IObservable<T>
    {
        System.Collections.Generic.IList<Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>>> Messages { get; }
        System.Collections.Generic.IList<Microsoft.Reactive.Testing.Subscription> Subscriptions { get; }
    }
    public interface ITestableObserver<T> : System.IObserver<T>
    {
        System.Collections.Generic.IList<Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>>> Messages { get; }
    }
    public static class ReactiveAssert
    {
        public static void AreElementsEqual<T>(System.Collections.Generic.IEnumerable<T> expected, System.Collections.Generic.IEnumerable<T> actual) { }
        public static void AreElementsEqual<T>(System.IObservable<T> expected, System.IObservable<T> actual) { }
        public static void AreElementsEqual<T>(System.Collections.Generic.IEnumerable<T> expected, System.Collections.Generic.IEnumerable<T> actual, string message) { }
        public static void AreElementsEqual<T>(System.IObservable<T> expected, System.IObservable<T> actual, string message) { }
        public static void AssertEqual<T>(this System.Collections.Generic.IEnumerable<T> actual, System.Collections.Generic.IEnumerable<T> expected) { }
        public static void AssertEqual<T>(this System.Collections.Generic.IEnumerable<T> actual, params T[] expected) { }
        public static void AssertEqual<T>(this System.IObservable<T> actual, System.IObservable<T> expected) { }
        public static void Throws<TException>(System.Action action)
            where TException : System.Exception { }
        public static void Throws<TException>(System.Action action, string message)
            where TException : System.Exception { }
        public static void Throws<TException>(TException exception, System.Action action)
            where TException : System.Exception { }
        public static void Throws<TException>(TException exception, System.Action action, string message)
            where TException : System.Exception { }
    }
    public class ReactiveTest
    {
        public const long Created = 100;
        public const long Disposed = 1000;
        public const long Subscribed = 200;
        public ReactiveTest() { }
        public static Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>> OnCompleted<T>(long ticks) { }
        public static Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>> OnCompleted<T>(long ticks, T witness) { }
        public static Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>> OnError<T>(long ticks, System.Exception exception) { }
        public static Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>> OnError<T>(long ticks, System.Func<System.Exception, bool> predicate) { }
        public static Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>> OnError<T>(long ticks, System.Exception exception, T witness) { }
        public static Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>> OnError<T>(long ticks, System.Func<System.Exception, bool> predicate, T witness) { }
        public static Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>> OnNext<T>(long ticks, System.Func<T, bool> predicate) { }
        public static Microsoft.Reactive.Testing.Recorded<System.Reactive.Notification<T>> OnNext<T>(long ticks, T value) { }
        public static Microsoft.Reactive.Testing.Subscription Subscribe(long start) { }
        public static Microsoft.Reactive.Testing.Subscription Subscribe(long start, long end) { }
    }
    [System.Diagnostics.DebuggerDisplay("{Value}@{Time}")]
    public struct Recorded<T> : System.IEquatable<Microsoft.Reactive.Testing.Recorded<T>>
    {
        public Recorded(long time, T value) { }
        public long Time { get; }
        public T Value { get; }
        public bool Equals(Microsoft.Reactive.Testing.Recorded<T> other) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        public override string ToString() { }
        public static bool !=(Microsoft.Reactive.Testing.Recorded<T> left, Microsoft.Reactive.Testing.Recorded<T> right) { }
        public static bool ==(Microsoft.Reactive.Testing.Recorded<T> left, Microsoft.Reactive.Testing.Recorded<T> right) { }
    }
    [System.Diagnostics.DebuggerDisplay("({Subscribe}, {Unsubscribe})")]
    public struct Subscription : System.IEquatable<Microsoft.Reactive.Testing.Subscription>
    {
        public const long Infinite = 9223372036854775807;
        public Subscription(long subscribe) { }
        public Subscription(long subscribe, long unsubscribe) { }
        public long Subscribe { get; }
        public long Unsubscribe { get; }
        public bool Equals(Microsoft.Reactive.Testing.Subscription other) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        public override string ToString() { }
        public static bool !=(Microsoft.Reactive.Testing.Subscription left, Microsoft.Reactive.Testing.Subscription right) { }
        public static bool ==(Microsoft.Reactive.Testing.Subscription left, Microsoft.Reactive.Testing.Subscription right) { }
    }
    [System.Diagnostics.DebuggerDisplay("\\{ Clock = {Clock} Now = {Now.ToString(\"O\")} \\}")]
    public class TestScheduler : System.Reactive.Concurrency.VirtualTimeScheduler<long, long>
    {
        public TestScheduler() { }
        protected override long Add(long absolute, long relative) { }
        public Microsoft.Reactive.Testing.ITestableObservable<T> CreateColdObservable<T>(params Microsoft.Reactive.Testing.Recorded<>[] messages) { }
        public Microsoft.Reactive.Testing.ITestableObservable<T> CreateHotObservable<T>(params Microsoft.Reactive.Testing.Recorded<>[] messages) { }
        public Microsoft.Reactive.Testing.ITestableObserver<T> CreateObserver<T>() { }
        public override System.IDisposable ScheduleAbsolute<TState>(TState state, long dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public Microsoft.Reactive.Testing.ITestableObserver<T> Start<T>(System.Func<System.IObservable<T>> create) { }
        public Microsoft.Reactive.Testing.ITestableObserver<T> Start<T>(System.Func<System.IObservable<T>> create, long disposed) { }
        public Microsoft.Reactive.Testing.ITestableObserver<T> Start<T>(System.Func<System.IObservable<T>> create, long created, long subscribed, long disposed) { }
        protected override System.DateTimeOffset ToDateTimeOffset(long absolute) { }
        protected override long ToRelative(System.TimeSpan timeSpan) { }
    }
}