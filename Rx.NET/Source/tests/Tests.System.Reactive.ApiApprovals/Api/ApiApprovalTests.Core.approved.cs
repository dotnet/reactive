[assembly: System.CLSCompliant(true)]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(@"Tests.System.Reactive, PublicKey=00240000048000009400000006020000002400005253413100040000010001008f5cff058631087031f8350f30a36fa078027e5df2316b564352dc9eb7af7ce856016d3c5e9d058036fe73bb5c83987bd3fc0793fbe25d633cc4f37c2bd5f1d717cd2a81661bec08f0971dc6078e17bde372b89005e7738a0ebd501b896ca3e8315270ff64df7809dd912c372df61785a5085b3553b7872e39b1b1cc0ff5a6bc")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(@"Tests.System.Reactive.Uwp.DeviceRunner, PublicKey=00240000048000009400000006020000002400005253413100040000010001008f5cff058631087031f8350f30a36fa078027e5df2316b564352dc9eb7af7ce856016d3c5e9d058036fe73bb5c83987bd3fc0793fbe25d633cc4f37c2bd5f1d717cd2a81661bec08f0971dc6078e17bde372b89005e7738a0ebd501b896ca3e8315270ff64df7809dd912c372df61785a5085b3553b7872e39b1b1cc0ff5a6bc")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
namespace System
{
    public static class ObservableExtensions
    {
        public static System.IDisposable Subscribe<T>(this System.IObservable<T> source) { }
        public static System.IDisposable Subscribe<T>(this System.IObservable<T> source, System.Action<T> onNext) { }
        public static void Subscribe<T>(this System.IObservable<T> source, System.Threading.CancellationToken token) { }
        public static System.IDisposable Subscribe<T>(this System.IObservable<T> source, System.Action<T> onNext, System.Action onCompleted) { }
        public static System.IDisposable Subscribe<T>(this System.IObservable<T> source, System.Action<T> onNext, System.Action<System.Exception> onError) { }
        public static void Subscribe<T>(this System.IObservable<T> source, System.Action<T> onNext, System.Threading.CancellationToken token) { }
        public static void Subscribe<T>(this System.IObservable<T> source, System.IObserver<T> observer, System.Threading.CancellationToken token) { }
        public static void Subscribe<T>(this System.IObservable<T> source, System.Action<T> onNext, System.Action onCompleted, System.Threading.CancellationToken token) { }
        public static System.IDisposable Subscribe<T>(this System.IObservable<T> source, System.Action<T> onNext, System.Action<System.Exception> onError, System.Action onCompleted) { }
        public static void Subscribe<T>(this System.IObservable<T> source, System.Action<T> onNext, System.Action<System.Exception> onError, System.Threading.CancellationToken token) { }
        public static void Subscribe<T>(this System.IObservable<T> source, System.Action<T> onNext, System.Action<System.Exception> onError, System.Action onCompleted, System.Threading.CancellationToken token) { }
        public static System.IDisposable SubscribeSafe<T>(this System.IObservable<T> source, System.IObserver<T> observer) { }
    }
}
namespace System.Reactive
{
    public sealed class AnonymousObservable<T> : System.Reactive.ObservableBase<T>
    {
        public AnonymousObservable(System.Func<System.IObserver<T>, System.IDisposable> subscribe) { }
        protected override System.IDisposable SubscribeCore(System.IObserver<T> observer) { }
    }
    public sealed class AnonymousObserver<T> : System.Reactive.ObserverBase<T>
    {
        public AnonymousObserver(System.Action<T> onNext) { }
        public AnonymousObserver(System.Action<T> onNext, System.Action onCompleted) { }
        public AnonymousObserver(System.Action<T> onNext, System.Action<System.Exception> onError) { }
        public AnonymousObserver(System.Action<T> onNext, System.Action<System.Exception> onError, System.Action onCompleted) { }
        protected override void OnCompletedCore() { }
        protected override void OnErrorCore(System.Exception error) { }
        protected override void OnNextCore(T value) { }
    }
    public abstract class EventPatternSourceBase<TSender, TEventArgs>
    {
        protected EventPatternSourceBase(System.IObservable<System.Reactive.EventPattern<TSender, TEventArgs>> source, System.Action<System.Action<TSender, TEventArgs>, System.Reactive.EventPattern<TSender, TEventArgs>> invokeHandler) { }
        protected void Add(System.Delegate handler, System.Action<TSender, TEventArgs> invoke) { }
        protected void Remove(System.Delegate handler) { }
    }
    public class EventPattern<TEventArgs> : System.Reactive.EventPattern<object, TEventArgs>
    {
        public EventPattern(object sender, TEventArgs e) { }
    }
    public class EventPattern<TSender, TEventArgs> : System.IEquatable<System.Reactive.EventPattern<TSender, TEventArgs>>, System.Reactive.IEventPattern<TSender, TEventArgs>
    {
        public EventPattern(TSender sender, TEventArgs e) { }
        public TEventArgs EventArgs { get; }
        public TSender Sender { get; }
        public override bool Equals(object obj) { }
        public bool Equals(System.Reactive.EventPattern<TSender, TEventArgs> other) { }
        public override int GetHashCode() { }
        public static bool !=(System.Reactive.EventPattern<TSender, TEventArgs> first, System.Reactive.EventPattern<TSender, TEventArgs> second) { }
        public static bool ==(System.Reactive.EventPattern<TSender, TEventArgs> first, System.Reactive.EventPattern<TSender, TEventArgs> second) { }
    }
    [System.AttributeUsage(System.AttributeTargets.Assembly | System.AttributeTargets.Module | System.AttributeTargets.Class | System.AttributeTargets.Struct | System.AttributeTargets.Enum | System.AttributeTargets.Constructor | System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.Field | System.AttributeTargets.Event | System.AttributeTargets.Interface | System.AttributeTargets.Parameter | System.AttributeTargets.Delegate | System.AttributeTargets.ReturnValue | System.AttributeTargets.GenericParameter | System.AttributeTargets.All)]
    [System.Reactive.Experimental]
    public sealed class ExperimentalAttribute : System.Attribute
    {
        public ExperimentalAttribute() { }
    }
    public interface IEventPatternSource<TEventArgs>
    {
        event System.EventHandler<TEventArgs> OnNext;
    }
    public interface IEventPattern<out TSender, out TEventArgs>
    {
        TEventArgs EventArgs { get; }
        TSender Sender { get; }
    }
    public interface IEventSource<out T>
    {
        event System.Action<T> OnNext;
    }
    public interface IObserver<in TValue, out TResult>
    {
        TResult OnCompleted();
        TResult OnError(System.Exception exception);
        TResult OnNext(TValue value);
    }
    public interface ITaskObservableAwaiter<out T> : System.Runtime.CompilerServices.INotifyCompletion
    {
        bool IsCompleted { get; }
        T GetResult();
    }
    [System.Runtime.CompilerServices.AsyncMethodBuilder(typeof(System.Runtime.CompilerServices.TaskObservableMethodBuilder<T>))]
    public interface ITaskObservable<out T> : System.IObservable<T>
    {
        System.Reactive.ITaskObservableAwaiter<T> GetAwaiter();
    }
    [System.Reactive.Experimental]
    public class ListObservable<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.Generic.IList<T>, System.Collections.IEnumerable, System.IObservable<object>
    {
        public ListObservable(System.IObservable<T> source) { }
        [System.Runtime.CompilerServices.IndexerName("Item")]
        public int Item { get; }
        [System.Runtime.CompilerServices.IndexerName("Item")]
        public bool Item { get; }
        public T this[int index] { get; set; }
        [System.Runtime.CompilerServices.IndexerName("Item")]
        public T Item { get; }
        public void Add(T item) { }
        public void Clear() { }
        public bool Contains(T item) { }
        public void CopyTo(T[] array, int arrayIndex) { }
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() { }
        public int IndexOf(T item) { }
        public void Insert(int index, T item) { }
        public bool Remove(T item) { }
        public void RemoveAt(int index) { }
        public System.IDisposable Subscribe(System.IObserver<object> observer) { }
    }
    public static class Notification
    {
        public static System.Reactive.Notification<T> CreateOnCompleted<T>() { }
        public static System.Reactive.Notification<T> CreateOnError<T>(System.Exception error) { }
        public static System.Reactive.Notification<T> CreateOnNext<T>(T value) { }
    }
    public enum NotificationKind
    {
        OnNext = 0,
        OnError = 1,
        OnCompleted = 2,
    }
    public abstract class Notification<T> : System.IEquatable<System.Reactive.Notification<T>>
    {
        protected Notification() { }
        public abstract System.Exception Exception { get; }
        public abstract bool HasValue { get; }
        public abstract System.Reactive.NotificationKind Kind { get; }
        public abstract T Value { get; }
        public abstract void Accept(System.IObserver<T> observer);
        public abstract void Accept(System.Action<T> onNext, System.Action<System.Exception> onError, System.Action onCompleted);
        public abstract TResult Accept<TResult>(System.Reactive.IObserver<T, TResult> observer);
        public abstract TResult Accept<TResult>(System.Func<T, TResult> onNext, System.Func<System.Exception, TResult> onError, System.Func<TResult> onCompleted);
        public override bool Equals(object obj) { }
        public abstract bool Equals(System.Reactive.Notification<T> other);
        public System.IObservable<T> ToObservable() { }
        public System.IObservable<T> ToObservable(System.Reactive.Concurrency.IScheduler scheduler) { }
        public static bool !=(System.Reactive.Notification<T> left, System.Reactive.Notification<T> right) { }
        public static bool ==(System.Reactive.Notification<T> left, System.Reactive.Notification<T> right) { }
    }
    public abstract class ObservableBase<T> : System.IObservable<T>
    {
        protected ObservableBase() { }
        public System.IDisposable Subscribe(System.IObserver<T> observer) { }
        protected abstract System.IDisposable SubscribeCore(System.IObserver<T> observer);
    }
    public static class Observer
    {
        public static System.IObserver<T> AsObserver<T>(this System.IObserver<T> observer) { }
        public static System.IObserver<T> Checked<T>(this System.IObserver<T> observer) { }
        public static System.IObserver<T> Create<T>(System.Action<T> onNext) { }
        public static System.IObserver<T> Create<T>(System.Action<T> onNext, System.Action onCompleted) { }
        public static System.IObserver<T> Create<T>(System.Action<T> onNext, System.Action<System.Exception> onError) { }
        public static System.IObserver<T> Create<T>(System.Action<T> onNext, System.Action<System.Exception> onError, System.Action onCompleted) { }
        public static System.IObserver<T> NotifyOn<T>(this System.IObserver<T> observer, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObserver<T> NotifyOn<T>(this System.IObserver<T> observer, System.Threading.SynchronizationContext context) { }
        public static System.IObserver<T> Synchronize<T>(System.IObserver<T> observer) { }
        public static System.IObserver<T> Synchronize<T>(System.IObserver<T> observer, bool preventReentrancy) { }
        public static System.IObserver<T> Synchronize<T>(System.IObserver<T> observer, object gate) { }
        public static System.IObserver<T> Synchronize<T>(System.IObserver<T> observer, System.Reactive.Concurrency.AsyncLock asyncLock) { }
        public static System.Action<System.Reactive.Notification<T>> ToNotifier<T>(this System.IObserver<T> observer) { }
        public static System.IObserver<T> ToObserver<T>(this System.Action<System.Reactive.Notification<T>> handler) { }
        public static System.IObserver<T> ToObserver<T>(this System.IProgress<T> progress) { }
        public static System.IProgress<T> ToProgress<T>(this System.IObserver<T> observer) { }
        public static System.IProgress<T> ToProgress<T>(this System.IObserver<T> observer, System.Reactive.Concurrency.IScheduler scheduler) { }
    }
    public abstract class ObserverBase<T> : System.IDisposable, System.IObserver<T>
    {
        protected ObserverBase() { }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public void OnCompleted() { }
        protected abstract void OnCompletedCore();
        public void OnError(System.Exception error) { }
        protected abstract void OnErrorCore(System.Exception error);
        public void OnNext(T value) { }
        protected abstract void OnNextCore(T value);
    }
    public struct TimeInterval<T> : System.IEquatable<System.Reactive.TimeInterval<T>>
    {
        public TimeInterval(T value, System.TimeSpan interval) { }
        public System.TimeSpan Interval { get; }
        public T Value { get; }
        public override bool Equals(object obj) { }
        public bool Equals(System.Reactive.TimeInterval<T> other) { }
        public override int GetHashCode() { }
        public override string ToString() { }
        public static bool !=(System.Reactive.TimeInterval<T> first, System.Reactive.TimeInterval<T> second) { }
        public static bool ==(System.Reactive.TimeInterval<T> first, System.Reactive.TimeInterval<T> second) { }
    }
    public static class Timestamped
    {
        public static System.Reactive.Timestamped<T> Create<T>(T value, System.DateTimeOffset timestamp) { }
    }
    public struct Timestamped<T> : System.IEquatable<System.Reactive.Timestamped<T>>
    {
        public Timestamped(T value, System.DateTimeOffset timestamp) { }
        public System.DateTimeOffset Timestamp { get; }
        public T Value { get; }
        public override bool Equals(object obj) { }
        public bool Equals(System.Reactive.Timestamped<T> other) { }
        public override int GetHashCode() { }
        public override string ToString() { }
        public static bool !=(System.Reactive.Timestamped<T> first, System.Reactive.Timestamped<T> second) { }
        public static bool ==(System.Reactive.Timestamped<T> first, System.Reactive.Timestamped<T> second) { }
    }
    public struct Unit : System.IEquatable<System.Reactive.Unit>
    {
        public static System.Reactive.Unit Default { get; }
        public override bool Equals(object obj) { }
        public bool Equals(System.Reactive.Unit other) { }
        public override int GetHashCode() { }
        public override string ToString() { }
        public static bool !=(System.Reactive.Unit first, System.Reactive.Unit second) { }
        public static bool ==(System.Reactive.Unit first, System.Reactive.Unit second) { }
    }
}
namespace System.Reactive.Concurrency
{
    public sealed class AsyncLock : System.IDisposable
    {
        public AsyncLock() { }
        public void Dispose() { }
        public void Wait(System.Action action) { }
    }
    public class ControlScheduler : System.Reactive.Concurrency.LocalScheduler, System.Reactive.Concurrency.ISchedulerPeriodic
    {
        public ControlScheduler(System.Windows.Forms.Control control) { }
        public System.Windows.Forms.Control Control { get; }
        public override System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable SchedulePeriodic<TState>(TState state, System.TimeSpan period, System.Func<TState, TState> action) { }
    }
    public sealed class CurrentThreadScheduler : System.Reactive.Concurrency.LocalScheduler
    {
        [System.Obsolete("This instance property is no longer supported. Use CurrentThreadScheduler.IsSched" +
            "uleRequired instead.")]
        public bool ScheduleRequired { get; }
        public static System.Reactive.Concurrency.CurrentThreadScheduler Instance { get; }
        public static bool IsScheduleRequired { get; }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
    }
    public sealed class DefaultScheduler : System.Reactive.Concurrency.LocalScheduler, System.Reactive.Concurrency.ISchedulerPeriodic
    {
        public static System.Reactive.Concurrency.DefaultScheduler Instance { get; }
        protected override object GetService(System.Type serviceType) { }
        public override System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable SchedulePeriodic<TState>(TState state, System.TimeSpan period, System.Func<TState, TState> action) { }
    }
    public class DispatcherScheduler : System.Reactive.Concurrency.LocalScheduler, System.Reactive.Concurrency.ISchedulerPeriodic
    {
        public DispatcherScheduler(System.Windows.Threading.Dispatcher dispatcher) { }
        public DispatcherScheduler(System.Windows.Threading.Dispatcher dispatcher, System.Windows.Threading.DispatcherPriority priority) { }
        public System.Windows.Threading.Dispatcher Dispatcher { get; }
        public System.Windows.Threading.DispatcherPriority Priority { get; }
        public static System.Reactive.Concurrency.DispatcherScheduler Current { get; }
        [System.Obsolete("Use the Current property to retrieve the DispatcherScheduler instance for the cur" +
            "rent thread\'s Dispatcher object.")]
        public static System.Reactive.Concurrency.DispatcherScheduler Instance { get; }
        public override System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable SchedulePeriodic<TState>(TState state, System.TimeSpan period, System.Func<TState, TState> action) { }
    }
    public sealed class EventLoopScheduler : System.Reactive.Concurrency.LocalScheduler, System.IDisposable, System.Reactive.Concurrency.ISchedulerPeriodic
    {
        public EventLoopScheduler() { }
        public EventLoopScheduler(System.Func<System.Threading.ThreadStart, System.Threading.Thread> threadFactory) { }
        public void Dispose() { }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable SchedulePeriodic<TState>(TState state, System.TimeSpan period, System.Func<TState, TState> action) { }
        public override System.Reactive.Concurrency.IStopwatch StartStopwatch() { }
    }
    [System.Diagnostics.DebuggerDisplay("\\{ Clock = {Clock} Now = {Now.ToString(\"O\")} \\}")]
    public class HistoricalScheduler : System.Reactive.Concurrency.HistoricalSchedulerBase
    {
        public HistoricalScheduler() { }
        public HistoricalScheduler(System.DateTimeOffset initialClock) { }
        public HistoricalScheduler(System.DateTimeOffset initialClock, System.Collections.Generic.IComparer<System.DateTimeOffset> comparer) { }
        protected override System.Reactive.Concurrency.IScheduledItem<System.DateTimeOffset> GetNext() { }
        public override System.IDisposable ScheduleAbsolute<TState>(TState state, System.DateTimeOffset dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
    }
    public abstract class HistoricalSchedulerBase : System.Reactive.Concurrency.VirtualTimeSchedulerBase<System.DateTimeOffset, System.TimeSpan>
    {
        protected HistoricalSchedulerBase() { }
        protected HistoricalSchedulerBase(System.DateTimeOffset initialClock) { }
        protected HistoricalSchedulerBase(System.DateTimeOffset initialClock, System.Collections.Generic.IComparer<System.DateTimeOffset> comparer) { }
        protected override System.DateTimeOffset Add(System.DateTimeOffset absolute, System.TimeSpan relative) { }
        protected override System.DateTimeOffset ToDateTimeOffset(System.DateTimeOffset absolute) { }
        protected override System.TimeSpan ToRelative(System.TimeSpan timeSpan) { }
    }
    public interface IConcurrencyAbstractionLayer
    {
        bool SupportsLongRunning { get; }
        System.IDisposable QueueUserWorkItem(System.Action<object> action, object state);
        void Sleep(System.TimeSpan timeout);
        System.IDisposable StartPeriodicTimer(System.Action action, System.TimeSpan period);
        System.Reactive.Concurrency.IStopwatch StartStopwatch();
        void StartThread(System.Action<object> action, object state);
        System.IDisposable StartTimer(System.Action<object> action, object state, System.TimeSpan dueTime);
    }
    public interface IScheduledItem<TAbsolute>
    {
        TAbsolute DueTime { get; }
        void Invoke();
    }
    public interface IScheduler
    {
        System.DateTimeOffset Now { get; }
        System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action);
        System.IDisposable Schedule<TState>(TState state, System.DateTimeOffset dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action);
        System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action);
    }
    public interface ISchedulerLongRunning
    {
        System.IDisposable ScheduleLongRunning<TState>(TState state, System.Action<TState, System.Reactive.Disposables.ICancelable> action);
    }
    public interface ISchedulerPeriodic
    {
        System.IDisposable SchedulePeriodic<TState>(TState state, System.TimeSpan period, System.Func<TState, TState> action);
    }
    public interface IStopwatch
    {
        System.TimeSpan Elapsed { get; }
    }
    public interface IStopwatchProvider
    {
        System.Reactive.Concurrency.IStopwatch StartStopwatch();
    }
    public sealed class ImmediateScheduler : System.Reactive.Concurrency.LocalScheduler
    {
        public static System.Reactive.Concurrency.ImmediateScheduler Instance { get; }
        public override System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
    }
    public abstract class LocalScheduler : System.IServiceProvider, System.Reactive.Concurrency.IScheduler, System.Reactive.Concurrency.IStopwatchProvider
    {
        protected LocalScheduler() { }
        public virtual System.DateTimeOffset Now { get; }
        protected virtual object GetService(System.Type serviceType) { }
        public virtual System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public virtual System.IDisposable Schedule<TState>(TState state, System.DateTimeOffset dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public abstract System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action);
        public virtual System.Reactive.Concurrency.IStopwatch StartStopwatch() { }
    }
    public sealed class NewThreadScheduler : System.Reactive.Concurrency.LocalScheduler, System.Reactive.Concurrency.ISchedulerLongRunning, System.Reactive.Concurrency.ISchedulerPeriodic
    {
        public NewThreadScheduler() { }
        public NewThreadScheduler(System.Func<System.Threading.ThreadStart, System.Threading.Thread> threadFactory) { }
        public static System.Reactive.Concurrency.NewThreadScheduler Default { get; }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable ScheduleLongRunning<TState>(TState state, System.Action<TState, System.Reactive.Disposables.ICancelable> action) { }
        public System.IDisposable SchedulePeriodic<TState>(TState state, System.TimeSpan period, System.Func<TState, TState> action) { }
        public override System.Reactive.Concurrency.IStopwatch StartStopwatch() { }
    }
    public abstract class ScheduledItem<TAbsolute> : System.IComparable<System.Reactive.Concurrency.ScheduledItem<TAbsolute>>, System.IDisposable, System.Reactive.Concurrency.IScheduledItem<TAbsolute>
        where TAbsolute : System.IComparable<TAbsolute>
    {
        protected ScheduledItem(TAbsolute dueTime, System.Collections.Generic.IComparer<TAbsolute> comparer) { }
        public TAbsolute DueTime { get; }
        public bool IsCanceled { get; }
        public void Cancel() { }
        public int CompareTo(System.Reactive.Concurrency.ScheduledItem<TAbsolute> other) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        public void Invoke() { }
        protected abstract System.IDisposable InvokeCore();
        public static bool !=(System.Reactive.Concurrency.ScheduledItem<TAbsolute> left, System.Reactive.Concurrency.ScheduledItem<TAbsolute> right) { }
        public static bool <(System.Reactive.Concurrency.ScheduledItem<TAbsolute> left, System.Reactive.Concurrency.ScheduledItem<TAbsolute> right) { }
        public static bool <=(System.Reactive.Concurrency.ScheduledItem<TAbsolute> left, System.Reactive.Concurrency.ScheduledItem<TAbsolute> right) { }
        public static bool ==(System.Reactive.Concurrency.ScheduledItem<TAbsolute> left, System.Reactive.Concurrency.ScheduledItem<TAbsolute> right) { }
        public static bool >(System.Reactive.Concurrency.ScheduledItem<TAbsolute> left, System.Reactive.Concurrency.ScheduledItem<TAbsolute> right) { }
        public static bool >=(System.Reactive.Concurrency.ScheduledItem<TAbsolute> left, System.Reactive.Concurrency.ScheduledItem<TAbsolute> right) { }
    }
    public sealed class ScheduledItem<TAbsolute, TValue> : System.Reactive.Concurrency.ScheduledItem<TAbsolute>
        where TAbsolute : System.IComparable<TAbsolute>
    {
        public ScheduledItem(System.Reactive.Concurrency.IScheduler scheduler, TValue state, System.Func<System.Reactive.Concurrency.IScheduler, TValue, System.IDisposable> action, TAbsolute dueTime) { }
        public ScheduledItem(System.Reactive.Concurrency.IScheduler scheduler, TValue state, System.Func<System.Reactive.Concurrency.IScheduler, TValue, System.IDisposable> action, TAbsolute dueTime, System.Collections.Generic.IComparer<TAbsolute> comparer) { }
        protected override System.IDisposable InvokeCore() { }
    }
    public static class Scheduler
    {
        public static System.Reactive.Concurrency.CurrentThreadScheduler CurrentThread { get; }
        public static System.Reactive.Concurrency.DefaultScheduler Default { get; }
        public static System.Reactive.Concurrency.ImmediateScheduler Immediate { get; }
        [System.Obsolete("This property is no longer supported due to refactoring of the API surface and el" +
            "imination of platform-specific dependencies. Please use NewThreadScheduler.Defau" +
            "lt to obtain an instance of this scheduler type.")]
        public static System.Reactive.Concurrency.IScheduler NewThread { get; }
        public static System.DateTimeOffset Now { get; }
        [System.Obsolete("This property is no longer supported due to refactoring of the API surface and el" +
            "imination of platform-specific dependencies. Please use TaskPoolScheduler.Defaul" +
            "t to obtain an instance of this scheduler type.")]
        public static System.Reactive.Concurrency.IScheduler TaskPool { get; }
        [System.Obsolete(@"This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Consider using Scheduler.Default to obtain the platform's most appropriate pool-based scheduler. In order to access a specific pool-based scheduler, please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use the appropriate scheduler in the System.Reactive.Concurrency namespace.")]
        public static System.Reactive.Concurrency.IScheduler ThreadPool { get; }
        public static System.Reactive.Concurrency.ISchedulerLongRunning AsLongRunning(this System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Concurrency.ISchedulerPeriodic AsPeriodic(this System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Concurrency.IStopwatchProvider AsStopwatchProvider(this System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Concurrency.IScheduler Catch<TException>(this System.Reactive.Concurrency.IScheduler scheduler, System.Func<TException, bool> handler)
            where TException : System.Exception { }
        public static System.Reactive.Concurrency.IScheduler DisableOptimizations(this System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Concurrency.IScheduler DisableOptimizations(this System.Reactive.Concurrency.IScheduler scheduler, params System.Type[] optimizationInterfaces) { }
        public static System.TimeSpan Normalize(System.TimeSpan timeSpan) { }
        public static System.IDisposable Schedule(this System.Reactive.Concurrency.IScheduler scheduler, System.Action action) { }
        public static System.IDisposable Schedule(this System.Reactive.Concurrency.IScheduler scheduler, System.Action<System.Action> action) { }
        public static System.IDisposable Schedule(this System.Reactive.Concurrency.IScheduler scheduler, System.DateTimeOffset dueTime, System.Action action) { }
        public static System.IDisposable Schedule(this System.Reactive.Concurrency.IScheduler scheduler, System.DateTimeOffset dueTime, System.Action<System.Action<System.DateTimeOffset>> action) { }
        public static System.IDisposable Schedule(this System.Reactive.Concurrency.IScheduler scheduler, System.TimeSpan dueTime, System.Action action) { }
        public static System.IDisposable Schedule(this System.Reactive.Concurrency.IScheduler scheduler, System.TimeSpan dueTime, System.Action<System.Action<System.TimeSpan>> action) { }
        public static System.IDisposable Schedule<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.Action<TState, System.Action<TState>> action) { }
        public static System.IDisposable Schedule<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.DateTimeOffset dueTime, System.Action<TState, System.Action<TState, System.DateTimeOffset>> action) { }
        public static System.IDisposable Schedule<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.TimeSpan dueTime, System.Action<TState, System.Action<TState, System.TimeSpan>> action) { }
        public static System.IDisposable ScheduleAsync(this System.Reactive.Concurrency.IScheduler scheduler, System.Func<System.Reactive.Concurrency.IScheduler, System.Threading.CancellationToken, System.Threading.Tasks.Task> action) { }
        public static System.IDisposable ScheduleAsync(this System.Reactive.Concurrency.IScheduler scheduler, System.Func<System.Reactive.Concurrency.IScheduler, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IDisposable>> action) { }
        public static System.IDisposable ScheduleAsync(this System.Reactive.Concurrency.IScheduler scheduler, System.DateTimeOffset dueTime, System.Func<System.Reactive.Concurrency.IScheduler, System.Threading.CancellationToken, System.Threading.Tasks.Task> action) { }
        public static System.IDisposable ScheduleAsync(this System.Reactive.Concurrency.IScheduler scheduler, System.DateTimeOffset dueTime, System.Func<System.Reactive.Concurrency.IScheduler, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IDisposable>> action) { }
        public static System.IDisposable ScheduleAsync(this System.Reactive.Concurrency.IScheduler scheduler, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, System.Threading.CancellationToken, System.Threading.Tasks.Task> action) { }
        public static System.IDisposable ScheduleAsync(this System.Reactive.Concurrency.IScheduler scheduler, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IDisposable>> action) { }
        public static System.IDisposable ScheduleAsync<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.Threading.CancellationToken, System.Threading.Tasks.Task> action) { }
        public static System.IDisposable ScheduleAsync<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IDisposable>> action) { }
        public static System.IDisposable ScheduleAsync<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.DateTimeOffset dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.Threading.CancellationToken, System.Threading.Tasks.Task> action) { }
        public static System.IDisposable ScheduleAsync<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.DateTimeOffset dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IDisposable>> action) { }
        public static System.IDisposable ScheduleAsync<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.Threading.CancellationToken, System.Threading.Tasks.Task> action) { }
        public static System.IDisposable ScheduleAsync<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IDisposable>> action) { }
        public static System.IDisposable ScheduleLongRunning(this System.Reactive.Concurrency.ISchedulerLongRunning scheduler, System.Action<System.Reactive.Disposables.ICancelable> action) { }
        public static System.IDisposable SchedulePeriodic(this System.Reactive.Concurrency.IScheduler scheduler, System.TimeSpan period, System.Action action) { }
        public static System.IDisposable SchedulePeriodic<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.TimeSpan period, System.Action<TState> action) { }
        public static System.IDisposable SchedulePeriodic<TState>(this System.Reactive.Concurrency.IScheduler scheduler, TState state, System.TimeSpan period, System.Func<TState, TState> action) { }
        public static System.Reactive.Concurrency.SchedulerOperation Sleep(this System.Reactive.Concurrency.IScheduler scheduler, System.DateTimeOffset dueTime) { }
        public static System.Reactive.Concurrency.SchedulerOperation Sleep(this System.Reactive.Concurrency.IScheduler scheduler, System.TimeSpan dueTime) { }
        public static System.Reactive.Concurrency.SchedulerOperation Sleep(this System.Reactive.Concurrency.IScheduler scheduler, System.DateTimeOffset dueTime, System.Threading.CancellationToken cancellationToken) { }
        public static System.Reactive.Concurrency.SchedulerOperation Sleep(this System.Reactive.Concurrency.IScheduler scheduler, System.TimeSpan dueTime, System.Threading.CancellationToken cancellationToken) { }
        public static System.Reactive.Concurrency.IStopwatch StartStopwatch(this System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Concurrency.SchedulerOperation Yield(this System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Concurrency.SchedulerOperation Yield(this System.Reactive.Concurrency.IScheduler scheduler, System.Threading.CancellationToken cancellationToken) { }
    }
    public sealed class SchedulerOperation
    {
        public System.Reactive.Concurrency.SchedulerOperation ConfigureAwait(bool continueOnCapturedContext) { }
        public System.Reactive.Concurrency.SchedulerOperationAwaiter GetAwaiter() { }
    }
    public sealed class SchedulerOperationAwaiter : System.Runtime.CompilerServices.INotifyCompletion
    {
        public bool IsCompleted { get; }
        public void GetResult() { }
        public void OnCompleted(System.Action continuation) { }
    }
    public class SchedulerQueue<TAbsolute>
        where TAbsolute : System.IComparable<TAbsolute>
    {
        public SchedulerQueue() { }
        public SchedulerQueue(int capacity) { }
        public int Count { get; }
        public System.Reactive.Concurrency.ScheduledItem<TAbsolute> Dequeue() { }
        public void Enqueue(System.Reactive.Concurrency.ScheduledItem<TAbsolute> scheduledItem) { }
        public System.Reactive.Concurrency.ScheduledItem<TAbsolute> Peek() { }
        public bool Remove(System.Reactive.Concurrency.ScheduledItem<TAbsolute> scheduledItem) { }
    }
    public static class Synchronization
    {
        public static System.IObservable<TSource> ObserveOn<TSource>(System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> ObserveOn<TSource>(System.IObservable<TSource> source, System.Threading.SynchronizationContext context) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(System.IObservable<TSource> source, System.Threading.SynchronizationContext context) { }
        public static System.IObservable<TSource> Synchronize<TSource>(System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> Synchronize<TSource>(System.IObservable<TSource> source, object gate) { }
    }
    public class SynchronizationContextScheduler : System.Reactive.Concurrency.LocalScheduler
    {
        public SynchronizationContextScheduler(System.Threading.SynchronizationContext context) { }
        public SynchronizationContextScheduler(System.Threading.SynchronizationContext context, bool alwaysPost) { }
        public override System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
    }
    public sealed class TaskPoolScheduler : System.Reactive.Concurrency.LocalScheduler, System.Reactive.Concurrency.ISchedulerLongRunning, System.Reactive.Concurrency.ISchedulerPeriodic
    {
        public TaskPoolScheduler(System.Threading.Tasks.TaskFactory taskFactory) { }
        public static System.Reactive.Concurrency.TaskPoolScheduler Default { get; }
        public override System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable ScheduleLongRunning<TState>(TState state, System.Action<TState, System.Reactive.Disposables.ICancelable> action) { }
        public System.IDisposable SchedulePeriodic<TState>(TState state, System.TimeSpan period, System.Func<TState, TState> action) { }
        public override System.Reactive.Concurrency.IStopwatch StartStopwatch() { }
    }
    public sealed class ThreadPoolScheduler : System.Reactive.Concurrency.LocalScheduler, System.Reactive.Concurrency.ISchedulerLongRunning, System.Reactive.Concurrency.ISchedulerPeriodic
    {
        public static System.Reactive.Concurrency.ThreadPoolScheduler Instance { get; }
        public override System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public override System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable ScheduleLongRunning<TState>(TState state, System.Action<TState, System.Reactive.Disposables.ICancelable> action) { }
        public System.IDisposable SchedulePeriodic<TState>(TState state, System.TimeSpan period, System.Func<TState, TState> action) { }
        public override System.Reactive.Concurrency.IStopwatch StartStopwatch() { }
    }
    public abstract class VirtualTimeSchedulerBase<TAbsolute, TRelative> : System.IServiceProvider, System.Reactive.Concurrency.IScheduler, System.Reactive.Concurrency.IStopwatchProvider
        where TAbsolute : System.IComparable<TAbsolute>
    {
        protected VirtualTimeSchedulerBase() { }
        protected VirtualTimeSchedulerBase(TAbsolute initialClock, System.Collections.Generic.IComparer<TAbsolute> comparer) { }
        public TAbsolute Clock { get; set; }
        protected System.Collections.Generic.IComparer<TAbsolute> Comparer { get; }
        public bool IsEnabled { get; }
        public System.DateTimeOffset Now { get; }
        protected abstract TAbsolute Add(TAbsolute absolute, TRelative relative);
        public void AdvanceBy(TRelative time) { }
        public void AdvanceTo(TAbsolute time) { }
        protected abstract System.Reactive.Concurrency.IScheduledItem<TAbsolute> GetNext();
        protected virtual object GetService(System.Type serviceType) { }
        public System.IDisposable Schedule<TState>(TState state, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable Schedule<TState>(TState state, System.DateTimeOffset dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public System.IDisposable Schedule<TState>(TState state, System.TimeSpan dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public abstract System.IDisposable ScheduleAbsolute<TState>(TState state, TAbsolute dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action);
        public System.IDisposable ScheduleRelative<TState>(TState state, TRelative dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
        public void Sleep(TRelative time) { }
        public void Start() { }
        public System.Reactive.Concurrency.IStopwatch StartStopwatch() { }
        public void Stop() { }
        protected abstract System.DateTimeOffset ToDateTimeOffset(TAbsolute absolute);
        protected abstract TRelative ToRelative(System.TimeSpan timeSpan);
    }
    public static class VirtualTimeSchedulerExtensions
    {
        public static System.IDisposable ScheduleAbsolute<TAbsolute, TRelative>(this System.Reactive.Concurrency.VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler, TAbsolute dueTime, System.Action action)
            where TAbsolute : System.IComparable<TAbsolute> { }
        public static System.IDisposable ScheduleRelative<TAbsolute, TRelative>(this System.Reactive.Concurrency.VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler, TRelative dueTime, System.Action action)
            where TAbsolute : System.IComparable<TAbsolute> { }
    }
    public abstract class VirtualTimeScheduler<TAbsolute, TRelative> : System.Reactive.Concurrency.VirtualTimeSchedulerBase<TAbsolute, TRelative>
        where TAbsolute : System.IComparable<TAbsolute>
    {
        protected VirtualTimeScheduler() { }
        protected VirtualTimeScheduler(TAbsolute initialClock, System.Collections.Generic.IComparer<TAbsolute> comparer) { }
        protected override System.Reactive.Concurrency.IScheduledItem<TAbsolute> GetNext() { }
        public override System.IDisposable ScheduleAbsolute<TState>(TState state, TAbsolute dueTime, System.Func<System.Reactive.Concurrency.IScheduler, TState, System.IDisposable> action) { }
    }
}
namespace System.Reactive.Disposables
{
    public sealed class BooleanDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public BooleanDisposable() { }
        public bool IsDisposed { get; }
        public void Dispose() { }
    }
    public sealed class CancellationDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public CancellationDisposable() { }
        public CancellationDisposable(System.Threading.CancellationTokenSource cts) { }
        public bool IsDisposed { get; }
        public System.Threading.CancellationToken Token { get; }
        public void Dispose() { }
    }
    public sealed class CompositeDisposable : System.Collections.Generic.ICollection<System.IDisposable>, System.Collections.Generic.IEnumerable<System.IDisposable>, System.Collections.IEnumerable, System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public CompositeDisposable() { }
        public CompositeDisposable(System.Collections.Generic.IEnumerable<System.IDisposable> disposables) { }
        public CompositeDisposable(params System.IDisposable[] disposables) { }
        public CompositeDisposable(int capacity) { }
        public int Count { get; }
        public bool IsDisposed { get; }
        public bool IsReadOnly { get; }
        public void Add(System.IDisposable item) { }
        public void Clear() { }
        public bool Contains(System.IDisposable item) { }
        public void CopyTo(System.IDisposable[] array, int arrayIndex) { }
        public void Dispose() { }
        public System.Collections.Generic.IEnumerator<System.IDisposable> GetEnumerator() { }
        public bool Remove(System.IDisposable item) { }
    }
    public sealed class ContextDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public ContextDisposable(System.Threading.SynchronizationContext context, System.IDisposable disposable) { }
        public System.Threading.SynchronizationContext Context { get; }
        public bool IsDisposed { get; }
        public void Dispose() { }
    }
    public static class Disposable
    {
        public static System.IDisposable Empty { get; }
        public static System.IDisposable Create(System.Action dispose) { }
        public static System.IDisposable Create<TState>(TState state, System.Action<TState> dispose) { }
    }
    public interface ICancelable : System.IDisposable
    {
        bool IsDisposed { get; }
    }
    public sealed class MultipleAssignmentDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public MultipleAssignmentDisposable() { }
        public System.IDisposable Disposable { get; set; }
        public bool IsDisposed { get; }
        public void Dispose() { }
    }
    public sealed class RefCountDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public RefCountDisposable(System.IDisposable disposable) { }
        public RefCountDisposable(System.IDisposable disposable, bool throwWhenDisposed) { }
        public bool IsDisposed { get; }
        public void Dispose() { }
        public System.IDisposable GetDisposable() { }
    }
    public sealed class ScheduledDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public ScheduledDisposable(System.Reactive.Concurrency.IScheduler scheduler, System.IDisposable disposable) { }
        public System.IDisposable Disposable { get; }
        public bool IsDisposed { get; }
        public System.Reactive.Concurrency.IScheduler Scheduler { get; }
        public void Dispose() { }
    }
    public sealed class SerialDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public SerialDisposable() { }
        public System.IDisposable Disposable { get; set; }
        public bool IsDisposed { get; }
        public void Dispose() { }
    }
    public sealed class SingleAssignmentDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        public SingleAssignmentDisposable() { }
        public System.IDisposable Disposable { get; set; }
        public bool IsDisposed { get; }
        public void Dispose() { }
    }
    public abstract class StableCompositeDisposable : System.IDisposable, System.Reactive.Disposables.ICancelable
    {
        protected StableCompositeDisposable() { }
        public abstract bool IsDisposed { get; }
        public abstract void Dispose();
        public static System.Reactive.Disposables.ICancelable Create(System.Collections.Generic.IEnumerable<System.IDisposable> disposables) { }
        public static System.Reactive.Disposables.ICancelable Create(params System.IDisposable[] disposables) { }
        public static System.Reactive.Disposables.ICancelable Create(System.IDisposable disposable1, System.IDisposable disposable2) { }
    }
}
namespace System.Reactive.Joins
{
    public abstract class Pattern { }
    public class Pattern<TSource1> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> And<TSource11>(System.IObservable<TSource11> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> And<TSource12>(System.IObservable<TSource12> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> And<TSource13>(System.IObservable<TSource13> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> And<TSource14>(System.IObservable<TSource14> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> And<TSource15>(System.IObservable<TSource15> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> And<TSource16>(System.IObservable<TSource16> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3> And<TSource3>(System.IObservable<TSource3> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4> And<TSource4>(System.IObservable<TSource4> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5> And<TSource5>(System.IObservable<TSource5> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> And<TSource6>(System.IObservable<TSource6> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> And<TSource7>(System.IObservable<TSource7> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> And<TSource8>(System.IObservable<TSource8> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> And<TSource9>(System.IObservable<TSource9> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> selector) { }
    }
    public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> : System.Reactive.Joins.Pattern
    {
        public System.Reactive.Joins.Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> And<TSource10>(System.IObservable<TSource10> other) { }
        public System.Reactive.Joins.Plan<TResult> Then<TResult>(System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> selector) { }
    }
    public abstract class Plan<TResult> { }
    public abstract class QueryablePattern
    {
        protected QueryablePattern(System.Linq.Expressions.Expression expression) { }
        public System.Linq.Expressions.Expression Expression { get; }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> And<TSource11>(System.IObservable<TSource11> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> And<TSource12>(System.IObservable<TSource12> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> And<TSource13>(System.IObservable<TSource13> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> And<TSource14>(System.IObservable<TSource14> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> And<TSource15>(System.IObservable<TSource15> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> And<TSource16>(System.IObservable<TSource16> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3> And<TSource3>(System.IObservable<TSource3> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4> And<TSource4>(System.IObservable<TSource4> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5> And<TSource5>(System.IObservable<TSource5> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> And<TSource6>(System.IObservable<TSource6> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> And<TSource7>(System.IObservable<TSource7> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> And<TSource8>(System.IObservable<TSource8> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> And<TSource9>(System.IObservable<TSource9> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>> selector) { }
    }
    public class QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> : System.Reactive.Joins.QueryablePattern
    {
        public System.Reactive.Joins.QueryablePattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> And<TSource10>(System.IObservable<TSource10> other) { }
        public System.Reactive.Joins.QueryablePlan<TResult> Then<TResult>(System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>> selector) { }
    }
    public class QueryablePlan<TResult>
    {
        public System.Linq.Expressions.Expression Expression { get; }
    }
}
namespace System.Reactive.Linq
{
    public static class ControlObservable
    {
        public static System.IObservable<TSource> ObserveOn<TSource>(this System.IObservable<TSource> source, System.Windows.Forms.Control control) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(this System.IObservable<TSource> source, System.Windows.Forms.Control control) { }
    }
    public static class DispatcherObservable
    {
        public static System.IObservable<TSource> ObserveOn<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.DispatcherScheduler scheduler) { }
        public static System.IObservable<TSource> ObserveOn<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.Dispatcher dispatcher) { }
        public static System.IObservable<TSource> ObserveOn<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.DispatcherObject dispatcherObject) { }
        public static System.IObservable<TSource> ObserveOn<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.Dispatcher dispatcher, System.Windows.Threading.DispatcherPriority priority) { }
        public static System.IObservable<TSource> ObserveOn<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.DispatcherObject dispatcherObject, System.Windows.Threading.DispatcherPriority priority) { }
        public static System.IObservable<TSource> ObserveOnDispatcher<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> ObserveOnDispatcher<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.DispatcherPriority priority) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.DispatcherScheduler scheduler) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.Dispatcher dispatcher) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.DispatcherObject dispatcherObject) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.Dispatcher dispatcher, System.Windows.Threading.DispatcherPriority priority) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.DispatcherObject dispatcherObject, System.Windows.Threading.DispatcherPriority priority) { }
        public static System.IObservable<TSource> SubscribeOnDispatcher<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> SubscribeOnDispatcher<TSource>(this System.IObservable<TSource> source, System.Windows.Threading.DispatcherPriority priority) { }
    }
    public interface IGroupedObservable<out TKey, out TElement> : System.IObservable<TElement>
    {
        TKey Key { get; }
    }
    public interface IQbservable
    {
        System.Type ElementType { get; }
        System.Linq.Expressions.Expression Expression { get; }
        System.Reactive.Linq.IQbservableProvider Provider { get; }
    }
    public interface IQbservableProvider
    {
        System.Reactive.Linq.IQbservable<TResult> CreateQuery<TResult>(System.Linq.Expressions.Expression expression);
    }
    public interface IQbservable<out T> : System.IObservable<T>, System.Reactive.Linq.IQbservable { }
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.All, Inherited=false)]
    public sealed class LocalQueryMethodImplementationTypeAttribute : System.Attribute
    {
        public LocalQueryMethodImplementationTypeAttribute(System.Type targetType) { }
        public System.Type TargetType { get; }
    }
    public static class Observable
    {
        public static System.IObservable<TSource> Aggregate<TSource>(this System.IObservable<TSource> source, System.Func<TSource, TSource, TSource> accumulator) { }
        public static System.IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(this System.IObservable<TSource> source, TAccumulate seed, System.Func<TAccumulate, TSource, TAccumulate> accumulator) { }
        public static System.IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this System.IObservable<TSource> source, TAccumulate seed, System.Func<TAccumulate, TSource, TAccumulate> accumulator, System.Func<TAccumulate, TResult> resultSelector) { }
        public static System.IObservable<bool> All<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> Amb<TSource>(params System.IObservable<>[] sources) { }
        public static System.IObservable<TSource> Amb<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TSource> Amb<TSource>(this System.IObservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.Reactive.Joins.Pattern<TLeft, TRight> And<TLeft, TRight>(this System.IObservable<TLeft> left, System.IObservable<TRight> right) { }
        public static System.IObservable<bool> Any<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<bool> Any<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> Append<TSource>(this System.IObservable<TSource> source, TSource value) { }
        public static System.IObservable<TSource> Append<TSource>(this System.IObservable<TSource> source, TSource value, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> AsObservable<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> AutoConnect<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source, int minObservers = 1, System.Action<System.IDisposable> onConnect = null) { }
        public static System.IObservable<decimal> Average(this System.IObservable<decimal> source) { }
        public static System.IObservable<double> Average(this System.IObservable<double> source) { }
        public static System.IObservable<double> Average(this System.IObservable<int> source) { }
        public static System.IObservable<double> Average(this System.IObservable<long> source) { }
        public static System.IObservable<float> Average(this System.IObservable<float> source) { }
        public static System.IObservable<decimal?> Average(this System.IObservable<decimal?> source) { }
        public static System.IObservable<double?> Average(this System.IObservable<double?> source) { }
        public static System.IObservable<float?> Average(this System.IObservable<float?> source) { }
        public static System.IObservable<double?> Average(this System.IObservable<int?> source) { }
        public static System.IObservable<double?> Average(this System.IObservable<long?> source) { }
        public static System.IObservable<decimal> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, decimal> selector) { }
        public static System.IObservable<double> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, double> selector) { }
        public static System.IObservable<double> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int> selector) { }
        public static System.IObservable<double> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, long> selector) { }
        public static System.IObservable<float> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, float> selector) { }
        public static System.IObservable<decimal?> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, decimal?> selector) { }
        public static System.IObservable<double?> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, double?> selector) { }
        public static System.IObservable<float?> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, float?> selector) { }
        public static System.IObservable<double?> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int?> selector) { }
        public static System.IObservable<double?> Average<TSource>(this System.IObservable<TSource> source, System.Func<TSource, long?> selector) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.IObservable<TSource> source, int count) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.IObservable<TSource> source, int count, int skip) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, int count) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, System.TimeSpan timeShift) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, System.TimeSpan timeShift, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource, TBufferClosing>(this System.IObservable<TSource> source, System.Func<System.IObservable<TBufferClosing>> bufferClosingSelector) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource, TBufferBoundary>(this System.IObservable<TSource> source, System.IObservable<TBufferBoundary> bufferBoundaries) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(this System.IObservable<TSource> source, System.IObservable<TBufferOpening> bufferOpenings, System.Func<TBufferOpening, System.IObservable<TBufferClosing>> bufferClosingSelector) { }
        public static System.IObservable<TResult> Case<TValue, TResult>(System.Func<TValue> selector, System.Collections.Generic.IDictionary<TValue, System.IObservable<TResult>> sources) { }
        public static System.IObservable<TResult> Case<TValue, TResult>(System.Func<TValue> selector, System.Collections.Generic.IDictionary<TValue, System.IObservable<TResult>> sources, System.IObservable<TResult> defaultSource) { }
        public static System.IObservable<TResult> Case<TValue, TResult>(System.Func<TValue> selector, System.Collections.Generic.IDictionary<TValue, System.IObservable<TResult>> sources, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Cast<TResult>(this System.IObservable<object> source) { }
        public static System.IObservable<TSource> Catch<TSource>(params System.IObservable<>[] sources) { }
        public static System.IObservable<TSource> Catch<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TSource> Catch<TSource>(this System.IObservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.IObservable<TSource> Catch<TSource, TException>(this System.IObservable<TSource> source, System.Func<TException, System.IObservable<TSource>> handler)
            where TException : System.Exception { }
        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.IList<TSource>> Chunkify<TSource>(this System.IObservable<TSource> source) { }
        public static System.Collections.Generic.IEnumerable<TResult> Collect<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TResult> newCollector, System.Func<TResult, TSource, TResult> merge) { }
        public static System.Collections.Generic.IEnumerable<TResult> Collect<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TResult> getInitialCollector, System.Func<TResult, TSource, TResult> merge, System.Func<TResult, TResult> getNewCollector) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> CombineLatest<TSource>(params System.IObservable<>[] sources) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> CombineLatest<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TResult> CombineLatest<TSource, TResult>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, System.Func<System.Collections.Generic.IList<TSource>, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TResult>(this System.IObservable<TSource1> first, System.IObservable<TSource2> second, System.Func<TSource1, TSource2, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.Func<TSource1, TSource2, TSource3, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.IObservable<TSource13> source13, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.IObservable<TSource13> source13, System.IObservable<TSource14> source14, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(
                    this System.IObservable<TSource1> source1,
                    System.IObservable<TSource2> source2,
                    System.IObservable<TSource3> source3,
                    System.IObservable<TSource4> source4,
                    System.IObservable<TSource5> source5,
                    System.IObservable<TSource6> source6,
                    System.IObservable<TSource7> source7,
                    System.IObservable<TSource8> source8,
                    System.IObservable<TSource9> source9,
                    System.IObservable<TSource10> source10,
                    System.IObservable<TSource11> source11,
                    System.IObservable<TSource12> source12,
                    System.IObservable<TSource13> source13,
                    System.IObservable<TSource14> source14,
                    System.IObservable<TSource15> source15,
                    System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> resultSelector) { }
        public static System.IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(
                    this System.IObservable<TSource1> source1,
                    System.IObservable<TSource2> source2,
                    System.IObservable<TSource3> source3,
                    System.IObservable<TSource4> source4,
                    System.IObservable<TSource5> source5,
                    System.IObservable<TSource6> source6,
                    System.IObservable<TSource7> source7,
                    System.IObservable<TSource8> source8,
                    System.IObservable<TSource9> source9,
                    System.IObservable<TSource10> source10,
                    System.IObservable<TSource11> source11,
                    System.IObservable<TSource12> source12,
                    System.IObservable<TSource13> source13,
                    System.IObservable<TSource14> source14,
                    System.IObservable<TSource15> source15,
                    System.IObservable<TSource16> source16,
                    System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> resultSelector) { }
        public static System.IObservable<TSource> Concat<TSource>(params System.IObservable<>[] sources) { }
        public static System.IObservable<TSource> Concat<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TSource> Concat<TSource>(this System.IObservable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TSource> Concat<TSource>(this System.IObservable<System.Threading.Tasks.Task<TSource>> sources) { }
        public static System.IObservable<TSource> Concat<TSource>(this System.IObservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.IObservable<bool> Contains<TSource>(this System.IObservable<TSource> source, TSource value) { }
        public static System.IObservable<bool> Contains<TSource>(this System.IObservable<TSource> source, TSource value, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.IObservable<int> Count<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<int> Count<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.Action> subscribe) { }
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.IDisposable> subscribe) { }
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.Threading.Tasks.Task> subscribeAsync) { }
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.Threading.Tasks.Task<System.Action>> subscribeAsync) { }
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.Threading.Tasks.Task<System.IDisposable>> subscribeAsync) { }
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.Threading.CancellationToken, System.Threading.Tasks.Task> subscribeAsync) { }
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.Action>> subscribeAsync) { }
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IDisposable>> subscribeAsync) { }
        public static System.IObservable<TSource> DefaultIfEmpty<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> DefaultIfEmpty<TSource>(this System.IObservable<TSource> source, TSource defaultValue) { }
        public static System.IObservable<TResult> Defer<TResult>(System.Func<System.IObservable<TResult>> observableFactory) { }
        public static System.IObservable<TResult> Defer<TResult>(System.Func<System.Threading.Tasks.Task<System.IObservable<TResult>>> observableFactoryAsync) { }
        public static System.IObservable<TResult> DeferAsync<TResult>(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IObservable<TResult>>> observableFactoryAsync) { }
        public static System.IObservable<TSource> Delay<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset dueTime) { }
        public static System.IObservable<TSource> Delay<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime) { }
        public static System.IObservable<TSource> Delay<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Delay<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Delay<TSource, TDelay>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TDelay>> delayDurationSelector) { }
        public static System.IObservable<TSource> Delay<TSource, TDelay>(this System.IObservable<TSource> source, System.IObservable<TDelay> subscriptionDelay, System.Func<TSource, System.IObservable<TDelay>> delayDurationSelector) { }
        public static System.IObservable<TSource> DelaySubscription<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset dueTime) { }
        public static System.IObservable<TSource> DelaySubscription<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime) { }
        public static System.IObservable<TSource> DelaySubscription<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> DelaySubscription<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Dematerialize<TSource>(this System.IObservable<System.Reactive.Notification<TSource>> source) { }
        public static System.IObservable<TSource> Distinct<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> Distinct<TSource>(this System.IObservable<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.IObservable<TSource> Distinct<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.IObservable<TSource> Distinct<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<TSource> DistinctUntilChanged<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> DistinctUntilChanged<TSource>(this System.IObservable<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.IObservable<TSource> DistinctUntilChanged<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.IObservable<TSource> DistinctUntilChanged<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<TSource> Do<TSource>(this System.IObservable<TSource> source, System.Action<TSource> onNext) { }
        public static System.IObservable<TSource> Do<TSource>(this System.IObservable<TSource> source, System.IObserver<TSource> observer) { }
        public static System.IObservable<TSource> Do<TSource>(this System.IObservable<TSource> source, System.Action<TSource> onNext, System.Action onCompleted) { }
        public static System.IObservable<TSource> Do<TSource>(this System.IObservable<TSource> source, System.Action<TSource> onNext, System.Action<System.Exception> onError) { }
        public static System.IObservable<TSource> Do<TSource>(this System.IObservable<TSource> source, System.Action<TSource> onNext, System.Action<System.Exception> onError, System.Action onCompleted) { }
        public static System.IObservable<TSource> DoWhile<TSource>(this System.IObservable<TSource> source, System.Func<bool> condition) { }
        public static System.IObservable<TSource> ElementAt<TSource>(this System.IObservable<TSource> source, int index) { }
        public static System.IObservable<TSource> ElementAtOrDefault<TSource>(this System.IObservable<TSource> source, int index) { }
        public static System.IObservable<TResult> Empty<TResult>() { }
        public static System.IObservable<TResult> Empty<TResult>(System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Empty<TResult>(TResult witness) { }
        public static System.IObservable<TResult> Empty<TResult>(System.Reactive.Concurrency.IScheduler scheduler, TResult witness) { }
        public static System.IObservable<TSource> Finally<TSource>(this System.IObservable<TSource> source, System.Action finallyAction) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource First<TSource>(this System.IObservable<TSource> source) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource First<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> FirstAsync<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> FirstAsync<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource FirstOrDefault<TSource>(this System.IObservable<TSource> source) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource FirstOrDefault<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> FirstOrDefaultAsync<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> FirstOrDefaultAsync<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TResult> For<TSource, TResult>(System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, System.IObservable<TResult>> resultSelector) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static void ForEach<TSource>(this System.IObservable<TSource> source, System.Action<TSource> onNext) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static void ForEach<TSource>(this System.IObservable<TSource> source, System.Action<TSource, int> onNext) { }
        public static System.Threading.Tasks.Task ForEachAsync<TSource>(this System.IObservable<TSource> source, System.Action<TSource> onNext) { }
        public static System.Threading.Tasks.Task ForEachAsync<TSource>(this System.IObservable<TSource> source, System.Action<TSource, int> onNext) { }
        public static System.Threading.Tasks.Task ForEachAsync<TSource>(this System.IObservable<TSource> source, System.Action<TSource> onNext, System.Threading.CancellationToken cancellationToken) { }
        public static System.Threading.Tasks.Task ForEachAsync<TSource>(this System.IObservable<TSource> source, System.Action<TSource, int> onNext, System.Threading.CancellationToken cancellationToken) { }
        public static System.IObservable<System.Reactive.Unit> FromAsync(System.Func<System.Threading.Tasks.Task> actionAsync) { }
        public static System.IObservable<System.Reactive.Unit> FromAsync(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task> actionAsync) { }
        public static System.IObservable<System.Reactive.Unit> FromAsync(System.Func<System.Threading.Tasks.Task> actionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.Unit> FromAsync(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task> actionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> FromAsync<TResult>(System.Func<System.Threading.Tasks.Task<TResult>> functionAsync) { }
        public static System.IObservable<TResult> FromAsync<TResult>(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>> functionAsync) { }
        public static System.IObservable<TResult> FromAsync<TResult>(System.Func<System.Threading.Tasks.Task<TResult>> functionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> FromAsync<TResult>(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>> functionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<System.IObservable<System.Reactive.Unit>> FromAsyncPattern(System.Func<System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<System.IObservable<TResult>> FromAsyncPattern<TResult>(System.Func<System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1>(System.Func<TArg1, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, System.IObservable<TResult>> FromAsyncPattern<TArg1, TResult>(System.Func<TArg1, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2>(System.Func<TArg1, TArg2, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TResult>(System.Func<TArg1, TArg2, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3>(System.Func<TArg1, TArg2, TArg3, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TResult>(System.Func<TArg1, TArg2, TArg3, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4>(System.Func<TArg1, TArg2, TArg3, TArg4, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.IObservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.AsyncCallback, object, System.IAsyncResult> begin, System.Action<System.IAsyncResult> end) { }
        [System.Obsolete(@"This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.AsyncCallback, object, System.IAsyncResult> begin, System.Func<System.IAsyncResult, TResult> end) { }
        public static System.IObservable<System.Reactive.Unit> FromEvent(System.Action<System.Action> addHandler, System.Action<System.Action> removeHandler) { }
        public static System.IObservable<System.Reactive.Unit> FromEvent(System.Action<System.Action> addHandler, System.Action<System.Action> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TEventArgs> FromEvent<TEventArgs>(System.Action<System.Action<TEventArgs>> addHandler, System.Action<System.Action<TEventArgs>> removeHandler) { }
        public static System.IObservable<TEventArgs> FromEvent<TEventArgs>(System.Action<System.Action<TEventArgs>> addHandler, System.Action<System.Action<TEventArgs>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler) { }
        public static System.IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(System.Func<System.Action<TEventArgs>, TDelegate> conversion, System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler) { }
        public static System.IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(System.Func<System.Action<TEventArgs>, TDelegate> conversion, System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<object>> FromEventPattern(System.Action<System.EventHandler> addHandler, System.Action<System.EventHandler> removeHandler) { }
        public static System.IObservable<System.Reactive.EventPattern<object>> FromEventPattern(object target, string eventName) { }
        public static System.IObservable<System.Reactive.EventPattern<object>> FromEventPattern(System.Type type, string eventName) { }
        public static System.IObservable<System.Reactive.EventPattern<object>> FromEventPattern(System.Action<System.EventHandler> addHandler, System.Action<System.EventHandler> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<object>> FromEventPattern(object target, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<object>> FromEventPattern(System.Type type, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(System.Action<System.EventHandler<TEventArgs>> addHandler, System.Action<System.EventHandler<TEventArgs>> removeHandler) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(System.Type type, string eventName) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(System.Action<System.EventHandler<TEventArgs>> addHandler, System.Action<System.EventHandler<TEventArgs>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(System.Type type, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler) { }
        public static System.IObservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName) { }
        public static System.IObservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(System.Type type, string eventName) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(System.Func<System.EventHandler<TEventArgs>, TDelegate> conversion, System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler) { }
        public static System.IObservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(System.Type type, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(System.Func<System.EventHandler<TEventArgs>, TDelegate> conversion, System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler) { }
        public static System.IObservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(System.Action<TDelegate> addHandler, System.Action<TDelegate> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Generate<TState, TResult>(TState initialState, System.Func<TState, bool> condition, System.Func<TState, TState> iterate, System.Func<TState, TResult> resultSelector) { }
        public static System.IObservable<TResult> Generate<TState, TResult>(TState initialState, System.Func<TState, bool> condition, System.Func<TState, TState> iterate, System.Func<TState, TResult> resultSelector, System.Func<TState, System.DateTimeOffset> timeSelector) { }
        public static System.IObservable<TResult> Generate<TState, TResult>(TState initialState, System.Func<TState, bool> condition, System.Func<TState, TState> iterate, System.Func<TState, TResult> resultSelector, System.Func<TState, System.TimeSpan> timeSelector) { }
        public static System.IObservable<TResult> Generate<TState, TResult>(TState initialState, System.Func<TState, bool> condition, System.Func<TState, TState> iterate, System.Func<TState, TResult> resultSelector, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Generate<TState, TResult>(TState initialState, System.Func<TState, bool> condition, System.Func<TState, TState> iterate, System.Func<TState, TResult> resultSelector, System.Func<TState, System.DateTimeOffset> timeSelector, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Generate<TState, TResult>(TState initialState, System.Func<TState, bool> condition, System.Func<TState, TState> iterate, System.Func<TState, TResult> resultSelector, System.Func<TState, System.TimeSpan> timeSelector, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Subjects.AsyncSubject<TSource> GetAwaiter<TSource>(this System.IObservable<TSource> source) { }
        public static System.Reactive.Subjects.AsyncSubject<TSource> GetAwaiter<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source) { }
        public static System.Collections.Generic.IEnumerator<TSource> GetEnumerator<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, int capacity) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, int capacity) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TSource>, System.IObservable<TDuration>> durationSelector) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TSource>, System.IObservable<TDuration>> durationSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TSource>, System.IObservable<TDuration>> durationSelector, int capacity) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TSource>, System.IObservable<TDuration>> durationSelector, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TElement>, System.IObservable<TDuration>> durationSelector) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TElement>, System.IObservable<TDuration>> durationSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TElement>, System.IObservable<TDuration>> durationSelector, int capacity) { }
        public static System.IObservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TElement>, System.IObservable<TDuration>> durationSelector, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(this System.IObservable<TLeft> left, System.IObservable<TRight> right, System.Func<TLeft, System.IObservable<TLeftDuration>> leftDurationSelector, System.Func<TRight, System.IObservable<TRightDuration>> rightDurationSelector, System.Func<TLeft, System.IObservable<TRight>, TResult> resultSelector) { }
        public static System.IObservable<TResult> If<TResult>(System.Func<bool> condition, System.IObservable<TResult> thenSource) { }
        public static System.IObservable<TResult> If<TResult>(System.Func<bool> condition, System.IObservable<TResult> thenSource, System.IObservable<TResult> elseSource) { }
        public static System.IObservable<TResult> If<TResult>(System.Func<bool> condition, System.IObservable<TResult> thenSource, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> IgnoreElements<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<long> Interval(System.TimeSpan period) { }
        public static System.IObservable<long> Interval(System.TimeSpan period, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<bool> IsEmpty<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(this System.IObservable<TLeft> left, System.IObservable<TRight> right, System.Func<TLeft, System.IObservable<TLeftDuration>> leftDurationSelector, System.Func<TRight, System.IObservable<TRightDuration>> rightDurationSelector, System.Func<TLeft, TRight, TResult> resultSelector) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource Last<TSource>(this System.IObservable<TSource> source) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource Last<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> LastAsync<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> LastAsync<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource LastOrDefault<TSource>(this System.IObservable<TSource> source) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource LastOrDefault<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> LastOrDefaultAsync<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> LastOrDefaultAsync<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.Collections.Generic.IEnumerable<TSource> Latest<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<long> LongCount<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<long> LongCount<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<System.Reactive.Notification<TSource>> Materialize<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<decimal> Max(this System.IObservable<decimal> source) { }
        public static System.IObservable<double> Max(this System.IObservable<double> source) { }
        public static System.IObservable<int> Max(this System.IObservable<int> source) { }
        public static System.IObservable<long> Max(this System.IObservable<long> source) { }
        public static System.IObservable<float> Max(this System.IObservable<float> source) { }
        public static System.IObservable<decimal?> Max(this System.IObservable<decimal?> source) { }
        public static System.IObservable<double?> Max(this System.IObservable<double?> source) { }
        public static System.IObservable<float?> Max(this System.IObservable<float?> source) { }
        public static System.IObservable<int?> Max(this System.IObservable<int?> source) { }
        public static System.IObservable<long?> Max(this System.IObservable<long?> source) { }
        public static System.IObservable<TSource> Max<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> Max<TSource>(this System.IObservable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        public static System.IObservable<decimal> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, decimal> selector) { }
        public static System.IObservable<double> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, double> selector) { }
        public static System.IObservable<int> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int> selector) { }
        public static System.IObservable<long> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, long> selector) { }
        public static System.IObservable<float> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, float> selector) { }
        public static System.IObservable<decimal?> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, decimal?> selector) { }
        public static System.IObservable<double?> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, double?> selector) { }
        public static System.IObservable<float?> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, float?> selector) { }
        public static System.IObservable<int?> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int?> selector) { }
        public static System.IObservable<long?> Max<TSource>(this System.IObservable<TSource> source, System.Func<TSource, long?> selector) { }
        public static System.IObservable<TResult> Max<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, TResult> selector) { }
        public static System.IObservable<TResult> Max<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, TResult> selector, System.Collections.Generic.IComparer<TResult> comparer) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> MaxBy<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> MaxBy<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.IObservable<TSource> Merge<TSource>(params System.IObservable<>[] sources) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.IObservable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.IObservable<System.Threading.Tasks.Task<TSource>> sources) { }
        public static System.IObservable<TSource> Merge<TSource>(System.Reactive.Concurrency.IScheduler scheduler, params System.IObservable<>[] sources) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, int maxConcurrent) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.IObservable<System.IObservable<TSource>> sources, int maxConcurrent) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.IObservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, int maxConcurrent, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Merge<TSource>(this System.IObservable<TSource> first, System.IObservable<TSource> second, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<decimal> Min(this System.IObservable<decimal> source) { }
        public static System.IObservable<double> Min(this System.IObservable<double> source) { }
        public static System.IObservable<int> Min(this System.IObservable<int> source) { }
        public static System.IObservable<long> Min(this System.IObservable<long> source) { }
        public static System.IObservable<float> Min(this System.IObservable<float> source) { }
        public static System.IObservable<decimal?> Min(this System.IObservable<decimal?> source) { }
        public static System.IObservable<double?> Min(this System.IObservable<double?> source) { }
        public static System.IObservable<float?> Min(this System.IObservable<float?> source) { }
        public static System.IObservable<int?> Min(this System.IObservable<int?> source) { }
        public static System.IObservable<long?> Min(this System.IObservable<long?> source) { }
        public static System.IObservable<TSource> Min<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> Min<TSource>(this System.IObservable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        public static System.IObservable<decimal> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, decimal> selector) { }
        public static System.IObservable<double> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, double> selector) { }
        public static System.IObservable<int> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int> selector) { }
        public static System.IObservable<long> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, long> selector) { }
        public static System.IObservable<float> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, float> selector) { }
        public static System.IObservable<decimal?> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, decimal?> selector) { }
        public static System.IObservable<double?> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, double?> selector) { }
        public static System.IObservable<float?> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, float?> selector) { }
        public static System.IObservable<int?> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int?> selector) { }
        public static System.IObservable<long?> Min<TSource>(this System.IObservable<TSource> source, System.Func<TSource, long?> selector) { }
        public static System.IObservable<TResult> Min<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, TResult> selector) { }
        public static System.IObservable<TResult> Min<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, TResult> selector, System.Collections.Generic.IComparer<TResult> comparer) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> MinBy<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> MinBy<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> MostRecent<TSource>(this System.IObservable<TSource> source, TSource initialValue) { }
        public static System.Reactive.Subjects.IConnectableObservable<TResult> Multicast<TSource, TResult>(this System.IObservable<TSource> source, System.Reactive.Subjects.ISubject<TSource, TResult> subject) { }
        public static System.IObservable<TResult> Multicast<TSource, TIntermediate, TResult>(this System.IObservable<TSource> source, System.Func<System.Reactive.Subjects.ISubject<TSource, TIntermediate>> subjectSelector, System.Func<System.IObservable<TIntermediate>, System.IObservable<TResult>> selector) { }
        public static System.IObservable<TResult> Never<TResult>() { }
        public static System.IObservable<TResult> Never<TResult>(TResult witness) { }
        public static System.Collections.Generic.IEnumerable<TSource> Next<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> ObserveOn<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> ObserveOn<TSource>(this System.IObservable<TSource> source, System.Threading.SynchronizationContext context) { }
        public static System.IObservable<TResult> OfType<TResult>(this System.IObservable<object> source) { }
        public static System.IObservable<TSource> OnErrorResumeNext<TSource>(params System.IObservable<>[] sources) { }
        public static System.IObservable<TSource> OnErrorResumeNext<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TSource> OnErrorResumeNext<TSource>(this System.IObservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.IObservable<TSource> Prepend<TSource>(this System.IObservable<TSource> source, TSource value) { }
        public static System.IObservable<TSource> Prepend<TSource>(this System.IObservable<TSource> source, TSource value, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Publish<TSource>(this System.IObservable<TSource> source) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Publish<TSource>(this System.IObservable<TSource> source, TSource initialValue) { }
        public static System.IObservable<TResult> Publish<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector) { }
        public static System.IObservable<TResult> Publish<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector, TSource initialValue) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> PublishLast<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TResult> PublishLast<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector) { }
        public static System.IObservable<int> Range(int start, int count) { }
        public static System.IObservable<int> Range(int start, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> RefCount<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source) { }
        public static System.IObservable<TSource> RefCount<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source, int minObservers) { }
        public static System.IObservable<TSource> RefCount<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source, System.TimeSpan disconnectDelay) { }
        public static System.IObservable<TSource> RefCount<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source, int minObservers, System.TimeSpan disconnectDelay) { }
        public static System.IObservable<TSource> RefCount<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source, System.TimeSpan disconnectDelay, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> RefCount<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source, int minObservers, System.TimeSpan disconnectDelay, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Repeat<TResult>(TResult value) { }
        public static System.IObservable<TSource> Repeat<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount) { }
        public static System.IObservable<TResult> Repeat<TResult>(TResult value, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Repeat<TSource>(this System.IObservable<TSource> source, int repeatCount) { }
        public static System.IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> RepeatWhen<TSource, TSignal>(this System.IObservable<TSource> source, System.Func<System.IObservable<object>, System.IObservable<TSignal>> handler) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Replay<TSource>(this System.IObservable<TSource> source) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Replay<TSource>(this System.IObservable<TSource> source, int bufferSize) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Replay<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Replay<TSource>(this System.IObservable<TSource> source, System.TimeSpan window) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Replay<TSource>(this System.IObservable<TSource> source, int bufferSize, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Replay<TSource>(this System.IObservable<TSource> source, int bufferSize, System.TimeSpan window) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Replay<TSource>(this System.IObservable<TSource> source, System.TimeSpan window, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Subjects.IConnectableObservable<TSource> Replay<TSource>(this System.IObservable<TSource> source, int bufferSize, System.TimeSpan window, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Replay<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector) { }
        public static System.IObservable<TResult> Replay<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector, int bufferSize) { }
        public static System.IObservable<TResult> Replay<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Replay<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector, System.TimeSpan window) { }
        public static System.IObservable<TResult> Replay<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector, int bufferSize, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Replay<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector, int bufferSize, System.TimeSpan window) { }
        public static System.IObservable<TResult> Replay<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector, System.TimeSpan window, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Replay<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector, int bufferSize, System.TimeSpan window, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Retry<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> Retry<TSource>(this System.IObservable<TSource> source, int retryCount) { }
        public static System.IObservable<TSource> RetryWhen<TSource, TSignal>(this System.IObservable<TSource> source, System.Func<System.IObservable<System.Exception>, System.IObservable<TSignal>> handler) { }
        public static System.IObservable<TResult> Return<TResult>(TResult value) { }
        public static System.IObservable<TResult> Return<TResult>(TResult value, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Subjects.AsyncSubject<TSource> RunAsync<TSource>(this System.IObservable<TSource> source, System.Threading.CancellationToken cancellationToken) { }
        public static System.Reactive.Subjects.AsyncSubject<TSource> RunAsync<TSource>(this System.Reactive.Subjects.IConnectableObservable<TSource> source, System.Threading.CancellationToken cancellationToken) { }
        public static System.IObservable<TSource> Sample<TSource>(this System.IObservable<TSource> source, System.TimeSpan interval) { }
        public static System.IObservable<TSource> Sample<TSource>(this System.IObservable<TSource> source, System.TimeSpan interval, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Sample<TSource, TSample>(this System.IObservable<TSource> source, System.IObservable<TSample> sampler) { }
        public static System.IObservable<TSource> Scan<TSource>(this System.IObservable<TSource> source, System.Func<TSource, TSource, TSource> accumulator) { }
        public static System.IObservable<TAccumulate> Scan<TSource, TAccumulate>(this System.IObservable<TSource> source, TAccumulate seed, System.Func<TAccumulate, TSource, TAccumulate> accumulator) { }
        public static System.IObservable<TResult> Select<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, TResult> selector) { }
        public static System.IObservable<TResult> Select<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, TResult> selector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TResult>> selector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.Threading.Tasks.Task<TResult>> selector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.Collections.Generic.IEnumerable<TResult>> selector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.IObservable<TResult>> selector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.Threading.Tasks.Task<TResult>> selector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>> selector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>> selector) { }
        public static System.IObservable<TOther> SelectMany<TSource, TOther>(this System.IObservable<TSource> source, System.IObservable<TOther> other) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TResult>> onNext, System.Func<System.Exception, System.IObservable<TResult>> onError, System.Func<System.IObservable<TResult>> onCompleted) { }
        public static System.IObservable<TResult> SelectMany<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.IObservable<TResult>> onNext, System.Func<System.Exception, System.IObservable<TResult>> onError, System.Func<System.IObservable<TResult>> onCompleted) { }
        public static System.IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.Collections.Generic.IEnumerable<TCollection>> collectionSelector, System.Func<TSource, TCollection, TResult> resultSelector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TCollection>> collectionSelector, System.Func<TSource, TCollection, TResult> resultSelector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.Threading.Tasks.Task<TTaskResult>> taskSelector, System.Func<TSource, TTaskResult, TResult> resultSelector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.Collections.Generic.IEnumerable<TCollection>> collectionSelector, System.Func<TSource, int, TCollection, int, TResult> resultSelector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.IObservable<TCollection>> collectionSelector, System.Func<TSource, int, TCollection, int, TResult> resultSelector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.Threading.Tasks.Task<TTaskResult>> taskSelector, System.Func<TSource, int, TTaskResult, TResult> resultSelector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this System.IObservable<TSource> source, System.Func<TSource, System.Threading.CancellationToken, System.Threading.Tasks.Task<TTaskResult>> taskSelector, System.Func<TSource, TTaskResult, TResult> resultSelector) { }
        public static System.IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this System.IObservable<TSource> source, System.Func<TSource, int, System.Threading.CancellationToken, System.Threading.Tasks.Task<TTaskResult>> taskSelector, System.Func<TSource, int, TTaskResult, TResult> resultSelector) { }
        public static System.IObservable<bool> SequenceEqual<TSource>(this System.IObservable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second) { }
        public static System.IObservable<bool> SequenceEqual<TSource>(this System.IObservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.IObservable<bool> SequenceEqual<TSource>(this System.IObservable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.IObservable<bool> SequenceEqual<TSource>(this System.IObservable<TSource> first, System.IObservable<TSource> second, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource Single<TSource>(this System.IObservable<TSource> source) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource Single<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> SingleAsync<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> SingleAsync<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource SingleOrDefault<TSource>(this System.IObservable<TSource> source) { }
        [System.Obsolete(@"This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
        public static TSource SingleOrDefault<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> SingleOrDefaultAsync<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> SingleOrDefaultAsync<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> Skip<TSource>(this System.IObservable<TSource> source, int count) { }
        public static System.IObservable<TSource> Skip<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration) { }
        public static System.IObservable<TSource> Skip<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> SkipLast<TSource>(this System.IObservable<TSource> source, int count) { }
        public static System.IObservable<TSource> SkipLast<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration) { }
        public static System.IObservable<TSource> SkipLast<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> SkipUntil<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset startTime) { }
        public static System.IObservable<TSource> SkipUntil<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset startTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> SkipUntil<TSource, TOther>(this System.IObservable<TSource> source, System.IObservable<TOther> other) { }
        public static System.IObservable<TSource> SkipWhile<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> SkipWhile<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int, bool> predicate) { }
        public static System.IObservable<System.Reactive.Unit> Start(System.Action action) { }
        public static System.IObservable<System.Reactive.Unit> Start(System.Action action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Start<TResult>(System.Func<TResult> function) { }
        public static System.IObservable<TResult> Start<TResult>(System.Func<TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.Unit> StartAsync(System.Func<System.Threading.Tasks.Task> actionAsync) { }
        public static System.IObservable<System.Reactive.Unit> StartAsync(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task> actionAsync) { }
        public static System.IObservable<System.Reactive.Unit> StartAsync(System.Func<System.Threading.Tasks.Task> actionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.Unit> StartAsync(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task> actionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> StartAsync<TResult>(System.Func<System.Threading.Tasks.Task<TResult>> functionAsync) { }
        public static System.IObservable<TResult> StartAsync<TResult>(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>> functionAsync) { }
        public static System.IObservable<TResult> StartAsync<TResult>(System.Func<System.Threading.Tasks.Task<TResult>> functionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> StartAsync<TResult>(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>> functionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> StartWith<TSource>(this System.IObservable<TSource> source, System.Collections.Generic.IEnumerable<TSource> values) { }
        public static System.IObservable<TSource> StartWith<TSource>(this System.IObservable<TSource> source, params TSource[] values) { }
        public static System.IObservable<TSource> StartWith<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler, System.Collections.Generic.IEnumerable<TSource> values) { }
        public static System.IObservable<TSource> StartWith<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler, params TSource[] values) { }
        public static System.IDisposable Subscribe<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.IObserver<TSource> observer) { }
        public static System.IDisposable Subscribe<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.IObserver<TSource> observer, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> SubscribeOn<TSource>(this System.IObservable<TSource> source, System.Threading.SynchronizationContext context) { }
        public static System.IObservable<decimal> Sum(this System.IObservable<decimal> source) { }
        public static System.IObservable<double> Sum(this System.IObservable<double> source) { }
        public static System.IObservable<int> Sum(this System.IObservable<int> source) { }
        public static System.IObservable<long> Sum(this System.IObservable<long> source) { }
        public static System.IObservable<float> Sum(this System.IObservable<float> source) { }
        public static System.IObservable<decimal?> Sum(this System.IObservable<decimal?> source) { }
        public static System.IObservable<double?> Sum(this System.IObservable<double?> source) { }
        public static System.IObservable<float?> Sum(this System.IObservable<float?> source) { }
        public static System.IObservable<int?> Sum(this System.IObservable<int?> source) { }
        public static System.IObservable<long?> Sum(this System.IObservable<long?> source) { }
        public static System.IObservable<decimal> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, decimal> selector) { }
        public static System.IObservable<double> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, double> selector) { }
        public static System.IObservable<int> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int> selector) { }
        public static System.IObservable<long> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, long> selector) { }
        public static System.IObservable<float> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, float> selector) { }
        public static System.IObservable<decimal?> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, decimal?> selector) { }
        public static System.IObservable<double?> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, double?> selector) { }
        public static System.IObservable<float?> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, float?> selector) { }
        public static System.IObservable<int?> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int?> selector) { }
        public static System.IObservable<long?> Sum<TSource>(this System.IObservable<TSource> source, System.Func<TSource, long?> selector) { }
        public static System.IObservable<TSource> Switch<TSource>(this System.IObservable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TSource> Switch<TSource>(this System.IObservable<System.Threading.Tasks.Task<TSource>> sources) { }
        public static System.IObservable<TSource> Synchronize<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TSource> Synchronize<TSource>(this System.IObservable<TSource> source, object gate) { }
        public static System.IObservable<TSource> Take<TSource>(this System.IObservable<TSource> source, int count) { }
        public static System.IObservable<TSource> Take<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration) { }
        public static System.IObservable<TSource> Take<TSource>(this System.IObservable<TSource> source, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Take<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> TakeLast<TSource>(this System.IObservable<TSource> source, int count) { }
        public static System.IObservable<TSource> TakeLast<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration) { }
        public static System.IObservable<TSource> TakeLast<TSource>(this System.IObservable<TSource> source, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> TakeLast<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> TakeLast<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler timerScheduler, System.Reactive.Concurrency.IScheduler loopScheduler) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> TakeLastBuffer<TSource>(this System.IObservable<TSource> source, int count) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> TakeLastBuffer<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> TakeLastBuffer<TSource>(this System.IObservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> TakeUntil<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset endTime) { }
        public static System.IObservable<TSource> TakeUntil<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> stopPredicate) { }
        public static System.IObservable<TSource> TakeUntil<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset endTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> TakeUntil<TSource, TOther>(this System.IObservable<TSource> source, System.IObservable<TOther> other) { }
        public static System.IObservable<TSource> TakeWhile<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> TakeWhile<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int, bool> predicate) { }
        public static System.Reactive.Joins.Plan<TResult> Then<TSource, TResult>(this System.IObservable<TSource> source, System.Func<TSource, TResult> selector) { }
        public static System.IObservable<TSource> Throttle<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime) { }
        public static System.IObservable<TSource> Throttle<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Throttle<TSource, TThrottle>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TThrottle>> throttleDurationSelector) { }
        public static System.IObservable<TResult> Throw<TResult>(System.Exception exception) { }
        public static System.IObservable<TResult> Throw<TResult>(System.Exception exception, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Throw<TResult>(System.Exception exception, TResult witness) { }
        public static System.IObservable<TResult> Throw<TResult>(System.Exception exception, System.Reactive.Concurrency.IScheduler scheduler, TResult witness) { }
        public static System.IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Timeout<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset dueTime) { }
        public static System.IObservable<TSource> Timeout<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime) { }
        public static System.IObservable<TSource> Timeout<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset dueTime, System.IObservable<TSource> other) { }
        public static System.IObservable<TSource> Timeout<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Timeout<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime, System.IObservable<TSource> other) { }
        public static System.IObservable<TSource> Timeout<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Timeout<TSource>(this System.IObservable<TSource> source, System.DateTimeOffset dueTime, System.IObservable<TSource> other, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Timeout<TSource>(this System.IObservable<TSource> source, System.TimeSpan dueTime, System.IObservable<TSource> other, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> Timeout<TSource, TTimeout>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TTimeout>> timeoutDurationSelector) { }
        public static System.IObservable<TSource> Timeout<TSource, TTimeout>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TTimeout>> timeoutDurationSelector, System.IObservable<TSource> other) { }
        public static System.IObservable<TSource> Timeout<TSource, TTimeout>(this System.IObservable<TSource> source, System.IObservable<TTimeout> firstTimeout, System.Func<TSource, System.IObservable<TTimeout>> timeoutDurationSelector) { }
        public static System.IObservable<TSource> Timeout<TSource, TTimeout>(this System.IObservable<TSource> source, System.IObservable<TTimeout> firstTimeout, System.Func<TSource, System.IObservable<TTimeout>> timeoutDurationSelector, System.IObservable<TSource> other) { }
        public static System.IObservable<long> Timer(System.DateTimeOffset dueTime) { }
        public static System.IObservable<long> Timer(System.TimeSpan dueTime) { }
        public static System.IObservable<long> Timer(System.DateTimeOffset dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<long> Timer(System.DateTimeOffset dueTime, System.TimeSpan period) { }
        public static System.IObservable<long> Timer(System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<long> Timer(System.TimeSpan dueTime, System.TimeSpan period) { }
        public static System.IObservable<long> Timer(System.DateTimeOffset dueTime, System.TimeSpan period, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<long> Timer(System.TimeSpan dueTime, System.TimeSpan period, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Reactive.Timestamped<TSource>> Timestamp<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<System.Reactive.Timestamped<TSource>> Timestamp<TSource>(this System.IObservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource[]> ToArray<TSource>(this System.IObservable<TSource> source) { }
        public static System.Func<System.IObservable<System.Reactive.Unit>> ToAsync(this System.Action action) { }
        public static System.Func<System.IObservable<System.Reactive.Unit>> ToAsync(this System.Action action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1>(this System.Action<TArg1> action) { }
        public static System.Func<System.IObservable<TResult>> ToAsync<TResult>(this System.Func<TResult> function) { }
        public static System.Func<TArg1, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1>(this System.Action<TArg1> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<System.IObservable<TResult>> ToAsync<TResult>(this System.Func<TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2>(this System.Action<TArg1, TArg2> action) { }
        public static System.Func<TArg1, System.IObservable<TResult>> ToAsync<TArg1, TResult>(this System.Func<TArg1, TResult> function) { }
        public static System.Func<TArg1, TArg2, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2>(this System.Action<TArg1, TArg2> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, System.IObservable<TResult>> ToAsync<TArg1, TResult>(this System.Func<TArg1, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3>(this System.Action<TArg1, TArg2, TArg3> action) { }
        public static System.Func<TArg1, TArg2, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TResult>(this System.Func<TArg1, TArg2, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3>(this System.Action<TArg1, TArg2, TArg3> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TResult>(this System.Func<TArg1, TArg2, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4>(this System.Action<TArg1, TArg2, TArg3, TArg4> action) { }
        public static System.Func<TArg1, TArg2, TArg3, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TResult>(this System.Func<TArg1, TArg2, TArg3, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4>(this System.Action<TArg1, TArg2, TArg3, TArg4> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TResult>(this System.Func<TArg1, TArg2, TArg3, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, System.IObservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(this System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, System.IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(this System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.Collections.Generic.IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.IObservable<System.Collections.Generic.IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Collections.Generic.IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector) { }
        public static System.IObservable<System.Collections.Generic.IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Collections.Generic.IEnumerable<TSource> ToEnumerable<TSource>(this System.IObservable<TSource> source) { }
        public static System.Reactive.IEventSource<System.Reactive.Unit> ToEvent(this System.IObservable<System.Reactive.Unit> source) { }
        public static System.Reactive.IEventSource<TSource> ToEvent<TSource>(this System.IObservable<TSource> source) { }
        public static System.Reactive.IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(this System.IObservable<System.Reactive.EventPattern<TEventArgs>> source) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> ToList<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<System.Linq.ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector) { }
        public static System.IObservable<System.Linq.ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<System.Linq.ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector) { }
        public static System.IObservable<System.Linq.ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this System.IObservable<TSource> source, System.Func<TSource, TKey> keySelector, System.Func<TSource, TElement> elementSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.IObservable<TSource> ToObservable<TSource>(this System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.IObservable<TSource> ToObservable<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> Using<TResult, TResource>(System.Func<TResource> resourceFactory, System.Func<TResource, System.IObservable<TResult>> observableFactory)
            where TResource : System.IDisposable { }
        public static System.IObservable<TResult> Using<TResult, TResource>(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResource>> resourceFactoryAsync, System.Func<TResource, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IObservable<TResult>>> observableFactoryAsync)
            where TResource : System.IDisposable { }
        public static TSource Wait<TSource>(this System.IObservable<TSource> source) { }
        public static System.IObservable<TResult> When<TResult>(params System.Reactive.Joins.Plan<>[] plans) { }
        public static System.IObservable<TResult> When<TResult>(this System.Collections.Generic.IEnumerable<System.Reactive.Joins.Plan<TResult>> plans) { }
        public static System.IObservable<TSource> Where<TSource>(this System.IObservable<TSource> source, System.Func<TSource, bool> predicate) { }
        public static System.IObservable<TSource> Where<TSource>(this System.IObservable<TSource> source, System.Func<TSource, int, bool> predicate) { }
        public static System.IObservable<TSource> While<TSource>(System.Func<bool> condition, System.IObservable<TSource> source) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource>(this System.IObservable<TSource> source, int count) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource>(this System.IObservable<TSource> source, int count, int skip) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, int count) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, System.TimeSpan timeShift) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource>(this System.IObservable<TSource> source, System.TimeSpan timeSpan, System.TimeSpan timeShift, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource, TWindowClosing>(this System.IObservable<TSource> source, System.Func<System.IObservable<TWindowClosing>> windowClosingSelector) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource, TWindowBoundary>(this System.IObservable<TSource> source, System.IObservable<TWindowBoundary> windowBoundaries) { }
        public static System.IObservable<System.IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(this System.IObservable<TSource> source, System.IObservable<TWindowOpening> windowOpenings, System.Func<TWindowOpening, System.IObservable<TWindowClosing>> windowClosingSelector) { }
        public static System.IObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(this System.IObservable<TFirst> first, System.IObservable<TSecond> second, System.Func<TFirst, TSecond, TResult> resultSelector) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Zip<TSource>(params System.IObservable<>[] sources) { }
        public static System.IObservable<System.Collections.Generic.IList<TSource>> Zip<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.IObservable<TResult> Zip<TSource, TResult>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, System.Func<System.Collections.Generic.IList<TSource>, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TResult>(this System.IObservable<TSource1> first, System.Collections.Generic.IEnumerable<TSource2> second, System.Func<TSource1, TSource2, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TResult>(this System.IObservable<TSource1> first, System.IObservable<TSource2> second, System.Func<TSource1, TSource2, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.Func<TSource1, TSource2, TSource3, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.IObservable<TSource13> source13, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(this System.IObservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.IObservable<TSource13> source13, System.IObservable<TSource14> source14, System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(
                    this System.IObservable<TSource1> source1,
                    System.IObservable<TSource2> source2,
                    System.IObservable<TSource3> source3,
                    System.IObservable<TSource4> source4,
                    System.IObservable<TSource5> source5,
                    System.IObservable<TSource6> source6,
                    System.IObservable<TSource7> source7,
                    System.IObservable<TSource8> source8,
                    System.IObservable<TSource9> source9,
                    System.IObservable<TSource10> source10,
                    System.IObservable<TSource11> source11,
                    System.IObservable<TSource12> source12,
                    System.IObservable<TSource13> source13,
                    System.IObservable<TSource14> source14,
                    System.IObservable<TSource15> source15,
                    System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> resultSelector) { }
        public static System.IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(
                    this System.IObservable<TSource1> source1,
                    System.IObservable<TSource2> source2,
                    System.IObservable<TSource3> source3,
                    System.IObservable<TSource4> source4,
                    System.IObservable<TSource5> source5,
                    System.IObservable<TSource6> source6,
                    System.IObservable<TSource7> source7,
                    System.IObservable<TSource8> source8,
                    System.IObservable<TSource9> source9,
                    System.IObservable<TSource10> source10,
                    System.IObservable<TSource11> source11,
                    System.IObservable<TSource12> source12,
                    System.IObservable<TSource13> source13,
                    System.IObservable<TSource14> source14,
                    System.IObservable<TSource15> source15,
                    System.IObservable<TSource16> source16,
                    System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> resultSelector) { }
    }
    public static class ObservableEx
    {
        [System.Reactive.Experimental]
        public static System.IObservable<System.Reactive.Unit> Create(System.Func<System.Collections.Generic.IEnumerable<System.IObservable<object>>> iteratorMethod) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TResult> Create<TResult>(System.Func<System.IObserver<TResult>, System.Collections.Generic.IEnumerable<System.IObservable<object>>> iteratorMethod) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TSource> Expand<TSource>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TSource>> selector) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TSource> Expand<TSource>(this System.IObservable<TSource> source, System.Func<TSource, System.IObservable<TSource>> selector, System.Reactive.Concurrency.IScheduler scheduler) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TSource[]> ForkJoin<TSource>(params System.IObservable<>[] sources) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TSource[]> ForkJoin<TSource>(this System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TResult> ForkJoin<TSource1, TSource2, TResult>(this System.IObservable<TSource1> first, System.IObservable<TSource2> second, System.Func<TSource1, TSource2, TResult> resultSelector) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TResult> Let<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, System.IObservable<TResult>> selector) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TResult> ManySelect<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, TResult> selector) { }
        [System.Reactive.Experimental]
        public static System.IObservable<TResult> ManySelect<TSource, TResult>(this System.IObservable<TSource> source, System.Func<System.IObservable<TSource>, TResult> selector, System.Reactive.Concurrency.IScheduler scheduler) { }
        [System.Reactive.Experimental]
        public static System.Reactive.ListObservable<TSource> ToListObservable<TSource>(this System.IObservable<TSource> source) { }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Qbservable
    {
        public static System.Reactive.Linq.IQbservableProvider Provider { get; }
        public static System.Reactive.Linq.IQbservable<TSource> Aggregate<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TSource, TSource>> accumulator) { }
        public static System.Reactive.Linq.IQbservable<TAccumulate> Aggregate<TSource, TAccumulate>(this System.Reactive.Linq.IQbservable<TSource> source, TAccumulate seed, System.Linq.Expressions.Expression<System.Func<TAccumulate, TSource, TAccumulate>> accumulator) { }
        public static System.Reactive.Linq.IQbservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, TAccumulate seed, System.Linq.Expressions.Expression<System.Func<TAccumulate, TSource, TAccumulate>> accumulator, System.Linq.Expressions.Expression<System.Func<TAccumulate, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<bool> All<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> Amb<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Amb<TSource>(this System.Reactive.Linq.IQbservableProvider provider, params System.IObservable<>[] sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Amb<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.Reactive.Joins.QueryablePattern<TLeft, TRight> And<TLeft, TRight>(this System.Reactive.Linq.IQbservable<TLeft> left, System.IObservable<TRight> right) { }
        public static System.Reactive.Linq.IQbservable<bool> Any<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<bool> Any<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> Append<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, TSource value) { }
        public static System.Reactive.Linq.IQbservable<TSource> Append<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, TSource value, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TSource> AsObservable<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> AsQbservable<TSource>(this System.IObservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> AutoConnect<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Subjects.IConnectableObservable<TSource> source, int minObservers, System.Linq.Expressions.Expression<System.Action<System.IDisposable>> onConnect) { }
        public static System.Reactive.Linq.IQbservable<decimal> Average(this System.Reactive.Linq.IQbservable<decimal> source) { }
        public static System.Reactive.Linq.IQbservable<double> Average(this System.Reactive.Linq.IQbservable<double> source) { }
        public static System.Reactive.Linq.IQbservable<double> Average(this System.Reactive.Linq.IQbservable<int> source) { }
        public static System.Reactive.Linq.IQbservable<double> Average(this System.Reactive.Linq.IQbservable<long> source) { }
        public static System.Reactive.Linq.IQbservable<float> Average(this System.Reactive.Linq.IQbservable<float> source) { }
        public static System.Reactive.Linq.IQbservable<decimal?> Average(this System.Reactive.Linq.IQbservable<decimal?> source) { }
        public static System.Reactive.Linq.IQbservable<double?> Average(this System.Reactive.Linq.IQbservable<double?> source) { }
        public static System.Reactive.Linq.IQbservable<float?> Average(this System.Reactive.Linq.IQbservable<float?> source) { }
        public static System.Reactive.Linq.IQbservable<double?> Average(this System.Reactive.Linq.IQbservable<int?> source) { }
        public static System.Reactive.Linq.IQbservable<double?> Average(this System.Reactive.Linq.IQbservable<long?> source) { }
        public static System.Reactive.Linq.IQbservable<decimal> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, decimal>> selector) { }
        public static System.Reactive.Linq.IQbservable<double> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, double>> selector) { }
        public static System.Reactive.Linq.IQbservable<double> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int>> selector) { }
        public static System.Reactive.Linq.IQbservable<double> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, long>> selector) { }
        public static System.Reactive.Linq.IQbservable<float> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, float>> selector) { }
        public static System.Reactive.Linq.IQbservable<decimal?> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, decimal?>> selector) { }
        public static System.Reactive.Linq.IQbservable<double?> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, double?>> selector) { }
        public static System.Reactive.Linq.IQbservable<float?> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, float?>> selector) { }
        public static System.Reactive.Linq.IQbservable<double?> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int?>> selector) { }
        public static System.Reactive.Linq.IQbservable<double?> Average<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, long?>> selector) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count, int skip) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, int count) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, System.TimeSpan timeShift) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, System.TimeSpan timeShift, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource, TBufferBoundary>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TBufferBoundary> bufferBoundaries) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource, TBufferClosing>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TBufferClosing>>> bufferClosingSelector) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TBufferOpening> bufferOpenings, System.Linq.Expressions.Expression<System.Func<TBufferOpening, System.IObservable<TBufferClosing>>> bufferClosingSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Case<TValue, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TValue>> selector, System.Collections.Generic.IDictionary<TValue, System.IObservable<TResult>> sources) { }
        public static System.Reactive.Linq.IQbservable<TResult> Case<TValue, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TValue>> selector, System.Collections.Generic.IDictionary<TValue, System.IObservable<TResult>> sources, System.IObservable<TResult> defaultSource) { }
        public static System.Reactive.Linq.IQbservable<TResult> Case<TValue, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TValue>> selector, System.Collections.Generic.IDictionary<TValue, System.IObservable<TResult>> sources, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Cast<TResult>(this System.Reactive.Linq.IQbservable<object> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Catch<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Catch<TSource>(this System.Reactive.Linq.IQbservableProvider provider, params System.IObservable<>[] sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Catch<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.Reactive.Linq.IQbservable<TSource> Catch<TSource, TException>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TException, System.IObservable<TSource>>> handler)
            where TException : System.Exception { }
        public static System.Linq.IQueryable<System.Collections.Generic.IList<TSource>> Chunkify<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Linq.IQueryable<TResult> Collect<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TResult>> newCollector, System.Linq.Expressions.Expression<System.Func<TResult, TSource, TResult>> merge) { }
        public static System.Linq.IQueryable<TResult> Collect<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TResult>> getInitialCollector, System.Linq.Expressions.Expression<System.Func<TResult, TSource, TResult>> merge, System.Linq.Expressions.Expression<System.Func<TResult, TResult>> getNewCollector) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> CombineLatest<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> CombineLatest<TSource>(this System.Reactive.Linq.IQbservableProvider provider, params System.IObservable<>[] sources) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IList<TSource>, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TResult>(this System.Reactive.Linq.IQbservable<TSource1> first, System.IObservable<TSource2> second, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.IObservable<TSource13> source13, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.IObservable<TSource13> source13, System.IObservable<TSource14> source14, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(
                    this System.Reactive.Linq.IQbservable<TSource1> source1,
                    System.IObservable<TSource2> source2,
                    System.IObservable<TSource3> source3,
                    System.IObservable<TSource4> source4,
                    System.IObservable<TSource5> source5,
                    System.IObservable<TSource6> source6,
                    System.IObservable<TSource7> source7,
                    System.IObservable<TSource8> source8,
                    System.IObservable<TSource9> source9,
                    System.IObservable<TSource10> source10,
                    System.IObservable<TSource11> source11,
                    System.IObservable<TSource12> source12,
                    System.IObservable<TSource13> source13,
                    System.IObservable<TSource14> source14,
                    System.IObservable<TSource15> source15,
                    System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(
                    this System.Reactive.Linq.IQbservable<TSource1> source1,
                    System.IObservable<TSource2> source2,
                    System.IObservable<TSource3> source3,
                    System.IObservable<TSource4> source4,
                    System.IObservable<TSource5> source5,
                    System.IObservable<TSource6> source6,
                    System.IObservable<TSource7> source7,
                    System.IObservable<TSource8> source8,
                    System.IObservable<TSource9> source9,
                    System.IObservable<TSource10> source10,
                    System.IObservable<TSource11> source11,
                    System.IObservable<TSource12> source12,
                    System.IObservable<TSource13> source13,
                    System.IObservable<TSource14> source14,
                    System.IObservable<TSource15> source15,
                    System.IObservable<TSource16> source16,
                    System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TSource> Concat<TSource>(this System.Reactive.Linq.IQbservable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Concat<TSource>(this System.Reactive.Linq.IQbservable<System.Threading.Tasks.Task<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Concat<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Concat<TSource>(this System.Reactive.Linq.IQbservableProvider provider, params System.IObservable<>[] sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Concat<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.Reactive.Linq.IQbservable<bool> Contains<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, TSource value) { }
        public static System.Reactive.Linq.IQbservable<bool> Contains<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, TSource value, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.Reactive.Linq.IQbservable<int> Count<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<int> Count<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.Action>> subscribe) { }
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.IDisposable>> subscribe) { }
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.Threading.Tasks.Task>> subscribeAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.Threading.Tasks.Task<System.Action>>> subscribeAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.Threading.Tasks.Task<System.IDisposable>>> subscribeAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.Threading.CancellationToken, System.Threading.Tasks.Task>> subscribeAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.Action>>> subscribeAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IDisposable>>> subscribeAsync) { }
        public static System.Reactive.Linq.IQbservable<TSource> DefaultIfEmpty<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> DefaultIfEmpty<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, TSource defaultValue) { }
        public static System.Reactive.Linq.IQbservable<TResult> Defer<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObservable<TResult>>> observableFactory) { }
        public static System.Reactive.Linq.IQbservable<TResult> Defer<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task<System.IObservable<TResult>>>> observableFactoryAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> DeferAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IObservable<TResult>>>> observableFactoryAsync) { }
        public static System.Reactive.Linq.IQbservable<TSource> Delay<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset dueTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> Delay<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> Delay<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Delay<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Delay<TSource, TDelay>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TDelay>>> delayDurationSelector) { }
        public static System.Reactive.Linq.IQbservable<TSource> Delay<TSource, TDelay>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TDelay> subscriptionDelay, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TDelay>>> delayDurationSelector) { }
        public static System.Reactive.Linq.IQbservable<TSource> DelaySubscription<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset dueTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> DelaySubscription<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> DelaySubscription<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> DelaySubscription<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Dematerialize<TSource>(this System.Reactive.Linq.IQbservable<System.Reactive.Notification<TSource>> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Distinct<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Distinct<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.Reactive.Linq.IQbservable<TSource> Distinct<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Reactive.Linq.IQbservable<TSource> Distinct<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<TSource> DistinctUntilChanged<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> DistinctUntilChanged<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.Reactive.Linq.IQbservable<TSource> DistinctUntilChanged<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Reactive.Linq.IQbservable<TSource> DistinctUntilChanged<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<TSource> Do<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObserver<TSource> observer) { }
        public static System.Reactive.Linq.IQbservable<TSource> Do<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Action<TSource>> onNext) { }
        public static System.Reactive.Linq.IQbservable<TSource> Do<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Action<TSource>> onNext, System.Linq.Expressions.Expression<System.Action> onCompleted) { }
        public static System.Reactive.Linq.IQbservable<TSource> Do<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Action<TSource>> onNext, System.Linq.Expressions.Expression<System.Action<System.Exception>> onError) { }
        public static System.Reactive.Linq.IQbservable<TSource> Do<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Action<TSource>> onNext, System.Linq.Expressions.Expression<System.Action<System.Exception>> onError, System.Linq.Expressions.Expression<System.Action> onCompleted) { }
        public static System.Reactive.Linq.IQbservable<TSource> DoWhile<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<bool>> condition) { }
        public static System.Reactive.Linq.IQbservable<TSource> ElementAt<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int index) { }
        public static System.Reactive.Linq.IQbservable<TSource> ElementAtOrDefault<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int index) { }
        public static System.Reactive.Linq.IQbservable<TResult> Empty<TResult>(this System.Reactive.Linq.IQbservableProvider provider) { }
        public static System.Reactive.Linq.IQbservable<TResult> Empty<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Empty<TResult>(this System.Reactive.Linq.IQbservableProvider provider, TResult witness) { }
        public static System.Reactive.Linq.IQbservable<TResult> Empty<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Concurrency.IScheduler scheduler, TResult witness) { }
        public static System.Reactive.Linq.IQbservable<TSource> Finally<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Action> finallyAction) { }
        public static System.Reactive.Linq.IQbservable<TSource> FirstAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> FirstAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> FirstOrDefaultAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> FirstOrDefaultAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TResult> For<TSource, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TResult>>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> FromAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task>> actionAsync) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> FromAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task>> actionAsync) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> FromAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task>> actionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> FromAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task>> actionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> FromAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task<TResult>>> functionAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> FromAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>>> functionAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> FromAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task<TResult>>> functionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> FromAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>>> functionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Action<System.IAsyncResult>> end) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.Reactive.Linq.IQbservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.AsyncCallback, object, System.IAsyncResult>> begin, System.Linq.Expressions.Expression<System.Func<System.IAsyncResult, TResult>> end) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> FromEvent(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<System.Action>> addHandler, System.Linq.Expressions.Expression<System.Action<System.Action>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> FromEvent(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<System.Action>> addHandler, System.Linq.Expressions.Expression<System.Action<System.Action>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TEventArgs> FromEvent<TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<System.Action<TEventArgs>>> addHandler, System.Linq.Expressions.Expression<System.Action<System.Action<TEventArgs>>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<TEventArgs> FromEvent<TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<System.Action<TEventArgs>>> addHandler, System.Linq.Expressions.Expression<System.Action<System.Action<TEventArgs>>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Action<TEventArgs>, TDelegate>> conversion, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Action<TEventArgs>, TDelegate>> conversion, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<object>> FromEventPattern(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<System.EventHandler>> addHandler, System.Linq.Expressions.Expression<System.Action<System.EventHandler>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<object>> FromEventPattern(this System.Reactive.Linq.IQbservableProvider provider, object target, string eventName) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<object>> FromEventPattern(this System.Reactive.Linq.IQbservableProvider provider, System.Type type, string eventName) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<object>> FromEventPattern(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<System.EventHandler>> addHandler, System.Linq.Expressions.Expression<System.Action<System.EventHandler>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<object>> FromEventPattern(this System.Reactive.Linq.IQbservableProvider provider, object target, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<object>> FromEventPattern(this System.Reactive.Linq.IQbservableProvider provider, System.Type type, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<System.EventHandler<TEventArgs>>> addHandler, System.Linq.Expressions.Expression<System.Action<System.EventHandler<TEventArgs>>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, object target, string eventName) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Type type, string eventName) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<System.EventHandler<TEventArgs>>> addHandler, System.Linq.Expressions.Expression<System.Action<System.EventHandler<TEventArgs>>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, object target, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Type type, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, object target, string eventName) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Type type, string eventName) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.EventHandler<TEventArgs>, TDelegate>> conversion, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, object target, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Type type, string eventName, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.EventHandler<TEventArgs>, TDelegate>> conversion, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TDelegate>> addHandler, System.Linq.Expressions.Expression<System.Action<TDelegate>> removeHandler, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Generate<TState, TResult>(this System.Reactive.Linq.IQbservableProvider provider, TState initialState, System.Linq.Expressions.Expression<System.Func<TState, bool>> condition, System.Linq.Expressions.Expression<System.Func<TState, TState>> iterate, System.Linq.Expressions.Expression<System.Func<TState, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Generate<TState, TResult>(this System.Reactive.Linq.IQbservableProvider provider, TState initialState, System.Linq.Expressions.Expression<System.Func<TState, bool>> condition, System.Linq.Expressions.Expression<System.Func<TState, TState>> iterate, System.Linq.Expressions.Expression<System.Func<TState, TResult>> resultSelector, System.Linq.Expressions.Expression<System.Func<TState, System.DateTimeOffset>> timeSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Generate<TState, TResult>(this System.Reactive.Linq.IQbservableProvider provider, TState initialState, System.Linq.Expressions.Expression<System.Func<TState, bool>> condition, System.Linq.Expressions.Expression<System.Func<TState, TState>> iterate, System.Linq.Expressions.Expression<System.Func<TState, TResult>> resultSelector, System.Linq.Expressions.Expression<System.Func<TState, System.TimeSpan>> timeSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Generate<TState, TResult>(this System.Reactive.Linq.IQbservableProvider provider, TState initialState, System.Linq.Expressions.Expression<System.Func<TState, bool>> condition, System.Linq.Expressions.Expression<System.Func<TState, TState>> iterate, System.Linq.Expressions.Expression<System.Func<TState, TResult>> resultSelector, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Generate<TState, TResult>(this System.Reactive.Linq.IQbservableProvider provider, TState initialState, System.Linq.Expressions.Expression<System.Func<TState, bool>> condition, System.Linq.Expressions.Expression<System.Func<TState, TState>> iterate, System.Linq.Expressions.Expression<System.Func<TState, TResult>> resultSelector, System.Linq.Expressions.Expression<System.Func<TState, System.DateTimeOffset>> timeSelector, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Generate<TState, TResult>(this System.Reactive.Linq.IQbservableProvider provider, TState initialState, System.Linq.Expressions.Expression<System.Func<TState, bool>> condition, System.Linq.Expressions.Expression<System.Func<TState, TState>> iterate, System.Linq.Expressions.Expression<System.Func<TState, TResult>> resultSelector, System.Linq.Expressions.Expression<System.Func<TState, System.TimeSpan>> timeSelector, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, int capacity) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, int capacity) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TSource>, System.IObservable<TDuration>>> durationSelector) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TSource>, System.IObservable<TDuration>>> durationSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TSource>, System.IObservable<TDuration>>> durationSelector, int capacity) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TSource>, System.IObservable<TDuration>>> durationSelector, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, System.Linq.Expressions.Expression<System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TElement>, System.IObservable<TDuration>>> durationSelector) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, System.Linq.Expressions.Expression<System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TElement>, System.IObservable<TDuration>>> durationSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, System.Linq.Expressions.Expression<System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TElement>, System.IObservable<TDuration>>> durationSelector, int capacity) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Linq.IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, System.Linq.Expressions.Expression<System.Func<System.Reactive.Linq.IGroupedObservable<TKey, TElement>, System.IObservable<TDuration>>> durationSelector, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(this System.Reactive.Linq.IQbservable<TLeft> left, System.IObservable<TRight> right, System.Linq.Expressions.Expression<System.Func<TLeft, System.IObservable<TLeftDuration>>> leftDurationSelector, System.Linq.Expressions.Expression<System.Func<TRight, System.IObservable<TRightDuration>>> rightDurationSelector, System.Linq.Expressions.Expression<System.Func<TLeft, System.IObservable<TRight>, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> If<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<bool>> condition, System.IObservable<TResult> thenSource) { }
        public static System.Reactive.Linq.IQbservable<TResult> If<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<bool>> condition, System.IObservable<TResult> thenSource, System.IObservable<TResult> elseSource) { }
        public static System.Reactive.Linq.IQbservable<TResult> If<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<bool>> condition, System.IObservable<TResult> thenSource, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> IgnoreElements<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<long> Interval(this System.Reactive.Linq.IQbservableProvider provider, System.TimeSpan period) { }
        public static System.Reactive.Linq.IQbservable<long> Interval(this System.Reactive.Linq.IQbservableProvider provider, System.TimeSpan period, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<bool> IsEmpty<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(this System.Reactive.Linq.IQbservable<TLeft> left, System.IObservable<TRight> right, System.Linq.Expressions.Expression<System.Func<TLeft, System.IObservable<TLeftDuration>>> leftDurationSelector, System.Linq.Expressions.Expression<System.Func<TRight, System.IObservable<TRightDuration>>> rightDurationSelector, System.Linq.Expressions.Expression<System.Func<TLeft, TRight, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TSource> LastAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> LastAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> LastOrDefaultAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> LastOrDefaultAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Linq.IQueryable<TSource> Latest<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<long> LongCount<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<long> LongCount<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Notification<TSource>> Materialize<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<decimal> Max(this System.Reactive.Linq.IQbservable<decimal> source) { }
        public static System.Reactive.Linq.IQbservable<double> Max(this System.Reactive.Linq.IQbservable<double> source) { }
        public static System.Reactive.Linq.IQbservable<int> Max(this System.Reactive.Linq.IQbservable<int> source) { }
        public static System.Reactive.Linq.IQbservable<long> Max(this System.Reactive.Linq.IQbservable<long> source) { }
        public static System.Reactive.Linq.IQbservable<float> Max(this System.Reactive.Linq.IQbservable<float> source) { }
        public static System.Reactive.Linq.IQbservable<decimal?> Max(this System.Reactive.Linq.IQbservable<decimal?> source) { }
        public static System.Reactive.Linq.IQbservable<double?> Max(this System.Reactive.Linq.IQbservable<double?> source) { }
        public static System.Reactive.Linq.IQbservable<float?> Max(this System.Reactive.Linq.IQbservable<float?> source) { }
        public static System.Reactive.Linq.IQbservable<int?> Max(this System.Reactive.Linq.IQbservable<int?> source) { }
        public static System.Reactive.Linq.IQbservable<long?> Max(this System.Reactive.Linq.IQbservable<long?> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        public static System.Reactive.Linq.IQbservable<decimal> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, decimal>> selector) { }
        public static System.Reactive.Linq.IQbservable<double> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, double>> selector) { }
        public static System.Reactive.Linq.IQbservable<int> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int>> selector) { }
        public static System.Reactive.Linq.IQbservable<long> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, long>> selector) { }
        public static System.Reactive.Linq.IQbservable<float> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, float>> selector) { }
        public static System.Reactive.Linq.IQbservable<decimal?> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, decimal?>> selector) { }
        public static System.Reactive.Linq.IQbservable<double?> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, double?>> selector) { }
        public static System.Reactive.Linq.IQbservable<float?> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, float?>> selector) { }
        public static System.Reactive.Linq.IQbservable<int?> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int?>> selector) { }
        public static System.Reactive.Linq.IQbservable<long?> Max<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, long?>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Max<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TResult>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Max<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TResult>> selector, System.Collections.Generic.IComparer<TResult> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> MaxBy<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> MaxBy<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservable<System.Threading.Tasks.Task<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservableProvider provider, params System.IObservable<>[] sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservable<System.IObservable<TSource>> sources, int maxConcurrent) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, int maxConcurrent) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Concurrency.IScheduler scheduler, params System.IObservable<>[] sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.IObservable<TSource> second, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Merge<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, int maxConcurrent, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<decimal> Min(this System.Reactive.Linq.IQbservable<decimal> source) { }
        public static System.Reactive.Linq.IQbservable<double> Min(this System.Reactive.Linq.IQbservable<double> source) { }
        public static System.Reactive.Linq.IQbservable<int> Min(this System.Reactive.Linq.IQbservable<int> source) { }
        public static System.Reactive.Linq.IQbservable<long> Min(this System.Reactive.Linq.IQbservable<long> source) { }
        public static System.Reactive.Linq.IQbservable<float> Min(this System.Reactive.Linq.IQbservable<float> source) { }
        public static System.Reactive.Linq.IQbservable<decimal?> Min(this System.Reactive.Linq.IQbservable<decimal?> source) { }
        public static System.Reactive.Linq.IQbservable<double?> Min(this System.Reactive.Linq.IQbservable<double?> source) { }
        public static System.Reactive.Linq.IQbservable<float?> Min(this System.Reactive.Linq.IQbservable<float?> source) { }
        public static System.Reactive.Linq.IQbservable<int?> Min(this System.Reactive.Linq.IQbservable<int?> source) { }
        public static System.Reactive.Linq.IQbservable<long?> Min(this System.Reactive.Linq.IQbservable<long?> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Collections.Generic.IComparer<TSource> comparer) { }
        public static System.Reactive.Linq.IQbservable<decimal> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, decimal>> selector) { }
        public static System.Reactive.Linq.IQbservable<double> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, double>> selector) { }
        public static System.Reactive.Linq.IQbservable<int> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int>> selector) { }
        public static System.Reactive.Linq.IQbservable<long> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, long>> selector) { }
        public static System.Reactive.Linq.IQbservable<float> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, float>> selector) { }
        public static System.Reactive.Linq.IQbservable<decimal?> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, decimal?>> selector) { }
        public static System.Reactive.Linq.IQbservable<double?> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, double?>> selector) { }
        public static System.Reactive.Linq.IQbservable<float?> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, float?>> selector) { }
        public static System.Reactive.Linq.IQbservable<int?> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int?>> selector) { }
        public static System.Reactive.Linq.IQbservable<long?> Min<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, long?>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Min<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TResult>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Min<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TResult>> selector, System.Collections.Generic.IComparer<TResult> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> MinBy<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> MinBy<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static System.Linq.IQueryable<TSource> MostRecent<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, TSource initialValue) { }
        public static System.Reactive.Linq.IQbservable<TResult> Multicast<TSource, TIntermediate, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.Reactive.Subjects.ISubject<TSource, TIntermediate>>> subjectSelector, System.Linq.Expressions.Expression<System.Func<System.IObservable<TIntermediate>, System.IObservable<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Never<TResult>(this System.Reactive.Linq.IQbservableProvider provider) { }
        public static System.Reactive.Linq.IQbservable<TResult> Never<TResult>(this System.Reactive.Linq.IQbservableProvider provider, TResult witness) { }
        public static System.Linq.IQueryable<TSource> Next<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> ObserveOn<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> ObserveOn<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Threading.SynchronizationContext context) { }
        public static System.Reactive.Linq.IQbservable<TResult> OfType<TResult>(this System.Reactive.Linq.IQbservable<object> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> OnErrorResumeNext<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> OnErrorResumeNext<TSource>(this System.Reactive.Linq.IQbservableProvider provider, params System.IObservable<>[] sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> OnErrorResumeNext<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.Reactive.Linq.IQbservable<TSource> Prepend<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, TSource value) { }
        public static System.Reactive.Linq.IQbservable<TSource> Prepend<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, TSource value, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Publish<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Publish<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector, TSource initialValue) { }
        public static System.Reactive.Linq.IQbservable<TResult> PublishLast<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<int> Range(this System.Reactive.Linq.IQbservableProvider provider, int start, int count) { }
        public static System.Reactive.Linq.IQbservable<int> Range(this System.Reactive.Linq.IQbservableProvider provider, int start, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> RefCount<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Subjects.IConnectableObservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> RefCount<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Subjects.IConnectableObservable<TSource> source, int minObservers) { }
        public static System.Reactive.Linq.IQbservable<TSource> RefCount<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Subjects.IConnectableObservable<TSource> source, System.TimeSpan disconnectDelay) { }
        public static System.Reactive.Linq.IQbservable<TSource> RefCount<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Subjects.IConnectableObservable<TSource> source, int minObservers, System.TimeSpan disconnectDelay) { }
        public static System.Reactive.Linq.IQbservable<TSource> RefCount<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Subjects.IConnectableObservable<TSource> source, System.TimeSpan disconnectDelay, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> RefCount<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Reactive.Subjects.IConnectableObservable<TSource> source, int minObservers, System.TimeSpan disconnectDelay, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Repeat<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TResult> Repeat<TResult>(this System.Reactive.Linq.IQbservableProvider provider, TResult value) { }
        public static System.Reactive.Linq.IQbservable<TSource> Repeat<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int repeatCount) { }
        public static System.Reactive.Linq.IQbservable<TResult> Repeat<TResult>(this System.Reactive.Linq.IQbservableProvider provider, TResult value, int repeatCount) { }
        public static System.Reactive.Linq.IQbservable<TResult> Repeat<TResult>(this System.Reactive.Linq.IQbservableProvider provider, TResult value, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Repeat<TResult>(this System.Reactive.Linq.IQbservableProvider provider, TResult value, int repeatCount, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> RepeatWhen<TSource, TSignal>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<object>, System.IObservable<TSignal>>> handler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Replay<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Replay<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector, int bufferSize) { }
        public static System.Reactive.Linq.IQbservable<TResult> Replay<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Replay<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector, System.TimeSpan window) { }
        public static System.Reactive.Linq.IQbservable<TResult> Replay<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector, int bufferSize, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Replay<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector, int bufferSize, System.TimeSpan window) { }
        public static System.Reactive.Linq.IQbservable<TResult> Replay<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector, System.TimeSpan window, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Replay<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector, int bufferSize, System.TimeSpan window, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Retry<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Retry<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int retryCount) { }
        public static System.Reactive.Linq.IQbservable<TSource> RetryWhen<TSource, TSignal>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<System.Exception>, System.IObservable<TSignal>>> handler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Return<TResult>(this System.Reactive.Linq.IQbservableProvider provider, TResult value) { }
        public static System.Reactive.Linq.IQbservable<TResult> Return<TResult>(this System.Reactive.Linq.IQbservableProvider provider, TResult value, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Sample<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan interval) { }
        public static System.Reactive.Linq.IQbservable<TSource> Sample<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan interval, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Sample<TSource, TSample>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TSample> sampler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Scan<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TSource, TSource>> accumulator) { }
        public static System.Reactive.Linq.IQbservable<TAccumulate> Scan<TSource, TAccumulate>(this System.Reactive.Linq.IQbservable<TSource> source, TAccumulate seed, System.Linq.Expressions.Expression<System.Func<TAccumulate, TSource, TAccumulate>> accumulator) { }
        public static System.Reactive.Linq.IQbservable<TResult> Select<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TResult>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Select<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, TResult>> selector) { }
        public static System.Reactive.Linq.IQbservable<TOther> SelectMany<TSource, TOther>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TOther> other) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.Collections.Generic.IEnumerable<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.Threading.Tasks.Task<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.Collections.Generic.IEnumerable<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.IObservable<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.Threading.Tasks.Task<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>>> selector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TResult>>> onNext, System.Linq.Expressions.Expression<System.Func<System.Exception, System.IObservable<TResult>>> onError, System.Linq.Expressions.Expression<System.Func<System.IObservable<TResult>>> onCompleted) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.IObservable<TResult>>> onNext, System.Linq.Expressions.Expression<System.Func<System.Exception, System.IObservable<TResult>>> onError, System.Linq.Expressions.Expression<System.Func<System.IObservable<TResult>>> onCompleted) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TCollection, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.Collections.Generic.IEnumerable<TCollection>>> collectionSelector, System.Linq.Expressions.Expression<System.Func<TSource, TCollection, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TCollection, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TCollection>>> collectionSelector, System.Linq.Expressions.Expression<System.Func<TSource, TCollection, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.Threading.Tasks.Task<TTaskResult>>> taskSelector, System.Linq.Expressions.Expression<System.Func<TSource, TTaskResult, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TCollection, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.Collections.Generic.IEnumerable<TCollection>>> collectionSelector, System.Linq.Expressions.Expression<System.Func<TSource, int, TCollection, int, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TCollection, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.IObservable<TCollection>>> collectionSelector, System.Linq.Expressions.Expression<System.Func<TSource, int, TCollection, int, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.Threading.Tasks.Task<TTaskResult>>> taskSelector, System.Linq.Expressions.Expression<System.Func<TSource, int, TTaskResult, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.Threading.CancellationToken, System.Threading.Tasks.Task<TTaskResult>>> taskSelector, System.Linq.Expressions.Expression<System.Func<TSource, TTaskResult, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, System.Threading.CancellationToken, System.Threading.Tasks.Task<TTaskResult>>> taskSelector, System.Linq.Expressions.Expression<System.Func<TSource, int, TTaskResult, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<bool> SequenceEqual<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second) { }
        public static System.Reactive.Linq.IQbservable<bool> SequenceEqual<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.IObservable<TSource> second) { }
        public static System.Reactive.Linq.IQbservable<bool> SequenceEqual<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.Collections.Generic.IEnumerable<TSource> second, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.Reactive.Linq.IQbservable<bool> SequenceEqual<TSource>(this System.Reactive.Linq.IQbservable<TSource> first, System.IObservable<TSource> second, System.Collections.Generic.IEqualityComparer<TSource> comparer) { }
        public static System.Reactive.Linq.IQbservable<TSource> SingleAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> SingleAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> SingleOrDefaultAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> SingleOrDefaultAsync<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> Skip<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count) { }
        public static System.Reactive.Linq.IQbservable<TSource> Skip<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration) { }
        public static System.Reactive.Linq.IQbservable<TSource> Skip<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> SkipLast<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count) { }
        public static System.Reactive.Linq.IQbservable<TSource> SkipLast<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration) { }
        public static System.Reactive.Linq.IQbservable<TSource> SkipLast<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> SkipUntil<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset startTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> SkipUntil<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset startTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> SkipUntil<TSource, TOther>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TOther> other) { }
        public static System.Reactive.Linq.IQbservable<TSource> SkipWhile<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> SkipWhile<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> Start(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action> action) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> Start(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Start<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TResult>> function) { }
        public static System.Reactive.Linq.IQbservable<TResult> Start<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> StartAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task>> actionAsync) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> StartAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task>> actionAsync) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> StartAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task>> actionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> StartAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task>> actionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> StartAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task<TResult>>> functionAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> StartAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>>> functionAsync) { }
        public static System.Reactive.Linq.IQbservable<TResult> StartAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.Tasks.Task<TResult>>> functionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> StartAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResult>>> functionAsync, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> StartWith<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Collections.Generic.IEnumerable<TSource> values) { }
        public static System.Reactive.Linq.IQbservable<TSource> StartWith<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, params TSource[] values) { }
        public static System.Reactive.Linq.IQbservable<TSource> StartWith<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler, System.Collections.Generic.IEnumerable<TSource> values) { }
        public static System.Reactive.Linq.IQbservable<TSource> StartWith<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler, params TSource[] values) { }
        public static System.Reactive.Linq.IQbservable<TSource> SubscribeOn<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> SubscribeOn<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Threading.SynchronizationContext context) { }
        public static System.Reactive.Linq.IQbservable<decimal> Sum(this System.Reactive.Linq.IQbservable<decimal> source) { }
        public static System.Reactive.Linq.IQbservable<double> Sum(this System.Reactive.Linq.IQbservable<double> source) { }
        public static System.Reactive.Linq.IQbservable<int> Sum(this System.Reactive.Linq.IQbservable<int> source) { }
        public static System.Reactive.Linq.IQbservable<long> Sum(this System.Reactive.Linq.IQbservable<long> source) { }
        public static System.Reactive.Linq.IQbservable<float> Sum(this System.Reactive.Linq.IQbservable<float> source) { }
        public static System.Reactive.Linq.IQbservable<decimal?> Sum(this System.Reactive.Linq.IQbservable<decimal?> source) { }
        public static System.Reactive.Linq.IQbservable<double?> Sum(this System.Reactive.Linq.IQbservable<double?> source) { }
        public static System.Reactive.Linq.IQbservable<float?> Sum(this System.Reactive.Linq.IQbservable<float?> source) { }
        public static System.Reactive.Linq.IQbservable<int?> Sum(this System.Reactive.Linq.IQbservable<int?> source) { }
        public static System.Reactive.Linq.IQbservable<long?> Sum(this System.Reactive.Linq.IQbservable<long?> source) { }
        public static System.Reactive.Linq.IQbservable<decimal> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, decimal>> selector) { }
        public static System.Reactive.Linq.IQbservable<double> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, double>> selector) { }
        public static System.Reactive.Linq.IQbservable<int> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int>> selector) { }
        public static System.Reactive.Linq.IQbservable<long> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, long>> selector) { }
        public static System.Reactive.Linq.IQbservable<float> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, float>> selector) { }
        public static System.Reactive.Linq.IQbservable<decimal?> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, decimal?>> selector) { }
        public static System.Reactive.Linq.IQbservable<double?> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, double?>> selector) { }
        public static System.Reactive.Linq.IQbservable<float?> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, float?>> selector) { }
        public static System.Reactive.Linq.IQbservable<int?> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int?>> selector) { }
        public static System.Reactive.Linq.IQbservable<long?> Sum<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, long?>> selector) { }
        public static System.Reactive.Linq.IQbservable<TSource> Switch<TSource>(this System.Reactive.Linq.IQbservable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Switch<TSource>(this System.Reactive.Linq.IQbservable<System.Threading.Tasks.Task<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<TSource> Synchronize<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Synchronize<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, object gate) { }
        public static System.Reactive.Linq.IQbservable<TSource> Take<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count) { }
        public static System.Reactive.Linq.IQbservable<TSource> Take<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration) { }
        public static System.Reactive.Linq.IQbservable<TSource> Take<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Take<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeLast<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeLast<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeLast<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeLast<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeLast<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler timerScheduler, System.Reactive.Concurrency.IScheduler loopScheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> TakeLastBuffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> TakeLastBuffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> TakeLastBuffer<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan duration, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeUntil<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset endTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeUntil<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> stopPredicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeUntil<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset endTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeUntil<TSource, TOther>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TOther> other) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeWhile<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> TakeWhile<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, bool>> predicate) { }
        public static System.Reactive.Joins.QueryablePlan<TResult> Then<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TResult>> selector) { }
        public static System.Reactive.Linq.IQbservable<TSource> Throttle<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> Throttle<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Throttle<TSource, TThrottle>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TThrottle>>> throttleDurationSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Throw<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Exception exception) { }
        public static System.Reactive.Linq.IQbservable<TResult> Throw<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Exception exception, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TResult> Throw<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Exception exception, TResult witness) { }
        public static System.Reactive.Linq.IQbservable<TResult> Throw<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Exception exception, System.Reactive.Concurrency.IScheduler scheduler, TResult witness) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset dueTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset dueTime, System.IObservable<TSource> other) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime, System.IObservable<TSource> other) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.DateTimeOffset dueTime, System.IObservable<TSource> other, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan dueTime, System.IObservable<TSource> other, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource, TTimeout>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TTimeout>>> timeoutDurationSelector) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource, TTimeout>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TTimeout> firstTimeout, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TTimeout>>> timeoutDurationSelector) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource, TTimeout>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TTimeout>>> timeoutDurationSelector, System.IObservable<TSource> other) { }
        public static System.Reactive.Linq.IQbservable<TSource> Timeout<TSource, TTimeout>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TTimeout> firstTimeout, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TTimeout>>> timeoutDurationSelector, System.IObservable<TSource> other) { }
        public static System.Reactive.Linq.IQbservable<long> Timer(this System.Reactive.Linq.IQbservableProvider provider, System.DateTimeOffset dueTime) { }
        public static System.Reactive.Linq.IQbservable<long> Timer(this System.Reactive.Linq.IQbservableProvider provider, System.TimeSpan dueTime) { }
        public static System.Reactive.Linq.IQbservable<long> Timer(this System.Reactive.Linq.IQbservableProvider provider, System.DateTimeOffset dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<long> Timer(this System.Reactive.Linq.IQbservableProvider provider, System.DateTimeOffset dueTime, System.TimeSpan period) { }
        public static System.Reactive.Linq.IQbservable<long> Timer(this System.Reactive.Linq.IQbservableProvider provider, System.TimeSpan dueTime, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<long> Timer(this System.Reactive.Linq.IQbservableProvider provider, System.TimeSpan dueTime, System.TimeSpan period) { }
        public static System.Reactive.Linq.IQbservable<long> Timer(this System.Reactive.Linq.IQbservableProvider provider, System.DateTimeOffset dueTime, System.TimeSpan period, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<long> Timer(this System.Reactive.Linq.IQbservableProvider provider, System.TimeSpan dueTime, System.TimeSpan period, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Timestamped<TSource>> Timestamp<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<System.Reactive.Timestamped<TSource>> Timestamp<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource[]> ToArray<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Func<System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action> action) { }
        public static System.Func<System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1>> action) { }
        public static System.Func<System.Reactive.Linq.IQbservable<TResult>> ToAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TResult>> function) { }
        public static System.Func<TArg1, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<System.Reactive.Linq.IQbservable<TResult>> ToAsync<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2>> action) { }
        public static System.Func<TArg1, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TResult>> function) { }
        public static System.Func<TArg1, TArg2, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3>> action) { }
        public static System.Func<TArg1, TArg2, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>> action) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, System.Reactive.Linq.IQbservable<System.Reactive.Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>> action, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>> function) { }
        public static System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, System.Reactive.Linq.IQbservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>> function, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> ToList<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<System.Linq.ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector) { }
        public static System.Reactive.Linq.IQbservable<System.Linq.ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<System.Linq.ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector) { }
        public static System.Reactive.Linq.IQbservable<System.Linq.ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, System.Linq.Expressions.Expression<System.Func<TSource, TElement>> elementSelector, System.Collections.Generic.IEqualityComparer<TKey> comparer) { }
        public static System.Reactive.Linq.IQbservable<TSource> ToObservable<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> ToObservable<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<TSource> ToQbservable<TSource>(this System.Linq.IQueryable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> ToQbservable<TSource>(this System.Linq.IQueryable<TSource> source, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Linq.IQueryable<TSource> ToQueryable<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TResult> Using<TResult, TResource>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<TResource>> resourceFactory, System.Linq.Expressions.Expression<System.Func<TResource, System.IObservable<TResult>>> observableFactory)
            where TResource : System.IDisposable { }
        public static System.Reactive.Linq.IQbservable<TResult> Using<TResult, TResource>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<TResource>>> resourceFactoryAsync, System.Linq.Expressions.Expression<System.Func<TResource, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.IObservable<TResult>>>> observableFactoryAsync)
            where TResource : System.IDisposable { }
        public static System.Reactive.Linq.IQbservable<TResult> When<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.Reactive.Joins.QueryablePlan<TResult>> plans) { }
        public static System.Reactive.Linq.IQbservable<TResult> When<TResult>(this System.Reactive.Linq.IQbservableProvider provider, params System.Reactive.Joins.QueryablePlan<>[] plans) { }
        public static System.Reactive.Linq.IQbservable<TSource> Where<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> Where<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, int, bool>> predicate) { }
        public static System.Reactive.Linq.IQbservable<TSource> While<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<bool>> condition, System.IObservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, int count, int skip) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, int count) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, System.TimeSpan timeShift) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, int count, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.TimeSpan timeSpan, System.TimeSpan timeShift, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource, TWindowBoundary>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TWindowBoundary> windowBoundaries) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource, TWindowClosing>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TWindowClosing>>> windowClosingSelector) { }
        public static System.Reactive.Linq.IQbservable<System.IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(this System.Reactive.Linq.IQbservable<TSource> source, System.IObservable<TWindowOpening> windowOpenings, System.Linq.Expressions.Expression<System.Func<TWindowOpening, System.IObservable<TWindowClosing>>> windowClosingSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(this System.Reactive.Linq.IQbservable<TFirst> first, System.IObservable<TSecond> second, System.Linq.Expressions.Expression<System.Func<TFirst, TSecond, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Zip<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        public static System.Reactive.Linq.IQbservable<System.Collections.Generic.IList<TSource>> Zip<TSource>(this System.Reactive.Linq.IQbservableProvider provider, params System.IObservable<>[] sources) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource, TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IList<TSource>, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TResult>(this System.Reactive.Linq.IQbservable<TSource1> first, System.Collections.Generic.IEnumerable<TSource2> second, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TResult>(this System.Reactive.Linq.IQbservable<TSource1> first, System.IObservable<TSource2> second, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.IObservable<TSource13> source13, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(this System.Reactive.Linq.IQbservable<TSource1> source1, System.IObservable<TSource2> source2, System.IObservable<TSource3> source3, System.IObservable<TSource4> source4, System.IObservable<TSource5> source5, System.IObservable<TSource6> source6, System.IObservable<TSource7> source7, System.IObservable<TSource8> source8, System.IObservable<TSource9> source9, System.IObservable<TSource10> source10, System.IObservable<TSource11> source11, System.IObservable<TSource12> source12, System.IObservable<TSource13> source13, System.IObservable<TSource14> source14, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(
                    this System.Reactive.Linq.IQbservable<TSource1> source1,
                    System.IObservable<TSource2> source2,
                    System.IObservable<TSource3> source3,
                    System.IObservable<TSource4> source4,
                    System.IObservable<TSource5> source5,
                    System.IObservable<TSource6> source6,
                    System.IObservable<TSource7> source7,
                    System.IObservable<TSource8> source8,
                    System.IObservable<TSource9> source9,
                    System.IObservable<TSource10> source10,
                    System.IObservable<TSource11> source11,
                    System.IObservable<TSource12> source12,
                    System.IObservable<TSource13> source13,
                    System.IObservable<TSource14> source14,
                    System.IObservable<TSource15> source15,
                    System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>> resultSelector) { }
        public static System.Reactive.Linq.IQbservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(
                    this System.Reactive.Linq.IQbservable<TSource1> source1,
                    System.IObservable<TSource2> source2,
                    System.IObservable<TSource3> source3,
                    System.IObservable<TSource4> source4,
                    System.IObservable<TSource5> source5,
                    System.IObservable<TSource6> source6,
                    System.IObservable<TSource7> source7,
                    System.IObservable<TSource8> source8,
                    System.IObservable<TSource9> source9,
                    System.IObservable<TSource10> source10,
                    System.IObservable<TSource11> source11,
                    System.IObservable<TSource12> source12,
                    System.IObservable<TSource13> source13,
                    System.IObservable<TSource14> source14,
                    System.IObservable<TSource15> source15,
                    System.IObservable<TSource16> source16,
                    System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>> resultSelector) { }
    }
    [System.Reactive.Linq.LocalQueryMethodImplementationType(typeof(System.Reactive.Linq.ObservableEx))]
    public static class QbservableEx
    {
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<System.Reactive.Unit> Create(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<System.IObservable<object>>>> iteratorMethod) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TResult> Create<TResult>(this System.Reactive.Linq.IQbservableProvider provider, System.Linq.Expressions.Expression<System.Func<System.IObserver<TResult>, System.Collections.Generic.IEnumerable<System.IObservable<object>>>> iteratorMethod) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TSource> Expand<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TSource>>> selector) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TSource> Expand<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<TSource, System.IObservable<TSource>>> selector, System.Reactive.Concurrency.IScheduler scheduler) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TSource[]> ForkJoin<TSource>(this System.Reactive.Linq.IQbservableProvider provider, System.Collections.Generic.IEnumerable<System.IObservable<TSource>> sources) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TSource[]> ForkJoin<TSource>(this System.Reactive.Linq.IQbservableProvider provider, params System.IObservable<>[] sources) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TResult> ForkJoin<TSource1, TSource2, TResult>(this System.Reactive.Linq.IQbservable<TSource1> first, System.IObservable<TSource2> second, System.Linq.Expressions.Expression<System.Func<TSource1, TSource2, TResult>> resultSelector) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TResult> Let<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, System.IObservable<TResult>>> selector) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TResult> ManySelect<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, TResult>> selector) { }
        [System.Reactive.Experimental]
        public static System.Reactive.Linq.IQbservable<TResult> ManySelect<TSource, TResult>(this System.Reactive.Linq.IQbservable<TSource> source, System.Linq.Expressions.Expression<System.Func<System.IObservable<TSource>, TResult>> selector, System.Reactive.Concurrency.IScheduler scheduler) { }
    }
    public class QueryDebugger
    {
        public QueryDebugger() { }
    }
    public static class RemotingObservable
    {
        public static System.IObservable<TSource> Remotable<TSource>(this System.IObservable<TSource> source) { }
        public static System.Reactive.Linq.IQbservable<TSource> Remotable<TSource>(this System.Reactive.Linq.IQbservable<TSource> source) { }
        public static System.IObservable<TSource> Remotable<TSource>(this System.IObservable<TSource> source, System.Runtime.Remoting.Lifetime.ILease lease) { }
        public static System.Reactive.Linq.IQbservable<TSource> Remotable<TSource>(this System.Reactive.Linq.IQbservable<TSource> source, System.Runtime.Remoting.Lifetime.ILease lease) { }
    }
}
namespace System.Reactive.PlatformServices
{
    public class CurrentPlatformEnlightenmentProvider : System.Reactive.PlatformServices.IPlatformEnlightenmentProvider
    {
        public CurrentPlatformEnlightenmentProvider() { }
        public virtual T GetService<T>(object[] args)
            where T :  class { }
    }
    public class DefaultSystemClock : System.Reactive.PlatformServices.ISystemClock
    {
        public DefaultSystemClock() { }
        public System.DateTimeOffset UtcNow { get; }
    }
    public static class EnlightenmentProvider
    {
        public static bool EnsureLoaded() { }
    }
    public static class HostLifecycleService
    {
        public  static  event System.EventHandler<System.Reactive.PlatformServices.HostResumingEventArgs> Resuming;
        public  static  event System.EventHandler<System.Reactive.PlatformServices.HostSuspendingEventArgs> Suspending;
        public static void AddRef() { }
        public static void Release() { }
    }
    public class HostResumingEventArgs : System.EventArgs
    {
        public HostResumingEventArgs() { }
    }
    public class HostSuspendingEventArgs : System.EventArgs
    {
        public HostSuspendingEventArgs() { }
    }
    public interface IExceptionServices
    {
        void Rethrow(System.Exception exception);
    }
    public interface IHostLifecycleNotifications
    {
        event System.EventHandler<System.Reactive.PlatformServices.HostResumingEventArgs> Resuming;
        event System.EventHandler<System.Reactive.PlatformServices.HostSuspendingEventArgs> Suspending;
    }
    public interface INotifySystemClockChanged
    {
        event System.EventHandler<System.Reactive.PlatformServices.SystemClockChangedEventArgs> SystemClockChanged;
    }
    public interface IPlatformEnlightenmentProvider
    {
        T GetService<T>(params object[] args)
            where T :  class;
    }
    public interface ISystemClock
    {
        System.DateTimeOffset UtcNow { get; }
    }
    public class PeriodicTimerSystemClockMonitor : System.Reactive.PlatformServices.INotifySystemClockChanged
    {
        public PeriodicTimerSystemClockMonitor(System.TimeSpan period) { }
        public event System.EventHandler<System.Reactive.PlatformServices.SystemClockChangedEventArgs> SystemClockChanged;
    }
    public static class PlatformEnlightenmentProvider
    {
        [System.Obsolete("This mechanism will be removed in the next major version", false)]
        public static System.Reactive.PlatformServices.IPlatformEnlightenmentProvider Current { get; set; }
    }
    public static class SystemClock
    {
        public static System.DateTimeOffset UtcNow { get; }
        public static void AddRef() { }
        public static void Release() { }
    }
    public class SystemClockChangedEventArgs : System.EventArgs
    {
        public SystemClockChangedEventArgs() { }
        public SystemClockChangedEventArgs(System.DateTimeOffset oldTime, System.DateTimeOffset newTime) { }
        public System.DateTimeOffset NewTime { get; }
        public System.DateTimeOffset OldTime { get; }
    }
}
namespace System.Reactive.Subjects
{
    public sealed class AsyncSubject<T> : System.Reactive.Subjects.SubjectBase<T>, System.Runtime.CompilerServices.INotifyCompletion
    {
        public AsyncSubject() { }
        public override bool HasObservers { get; }
        public bool IsCompleted { get; }
        public override bool IsDisposed { get; }
        public override void Dispose() { }
        public System.Reactive.Subjects.AsyncSubject<T> GetAwaiter() { }
        public T GetResult() { }
        public override void OnCompleted() { }
        public void OnCompleted(System.Action continuation) { }
        public override void OnError(System.Exception error) { }
        public override void OnNext(T value) { }
        public override System.IDisposable Subscribe(System.IObserver<T> observer) { }
    }
    public sealed class BehaviorSubject<T> : System.Reactive.Subjects.SubjectBase<T>
    {
        public BehaviorSubject(T value) { }
        public override bool HasObservers { get; }
        public override bool IsDisposed { get; }
        public T Value { get; }
        public override void Dispose() { }
        public override void OnCompleted() { }
        public override void OnError(System.Exception error) { }
        public override void OnNext(T value) { }
        public override System.IDisposable Subscribe(System.IObserver<T> observer) { }
        public bool TryGetValue(out T value) { }
    }
    public interface IConnectableObservable<out T> : System.IObservable<T>
    {
        System.IDisposable Connect();
    }
    public interface ISubject<T> : System.IObservable<T>, System.IObserver<T>, System.Reactive.Subjects.ISubject<T, T> { }
    public interface ISubject<in TSource, out TResult> : System.IObservable<TResult>, System.IObserver<TSource> { }
    public sealed class ReplaySubject<T> : System.Reactive.Subjects.SubjectBase<T>
    {
        public ReplaySubject() { }
        public ReplaySubject(int bufferSize) { }
        public ReplaySubject(System.Reactive.Concurrency.IScheduler scheduler) { }
        public ReplaySubject(System.TimeSpan window) { }
        public ReplaySubject(int bufferSize, System.Reactive.Concurrency.IScheduler scheduler) { }
        public ReplaySubject(int bufferSize, System.TimeSpan window) { }
        public ReplaySubject(System.TimeSpan window, System.Reactive.Concurrency.IScheduler scheduler) { }
        public ReplaySubject(int bufferSize, System.TimeSpan window, System.Reactive.Concurrency.IScheduler scheduler) { }
        public override bool HasObservers { get; }
        public override bool IsDisposed { get; }
        public override void Dispose() { }
        public override void OnCompleted() { }
        public override void OnError(System.Exception error) { }
        public override void OnNext(T value) { }
        public override System.IDisposable Subscribe(System.IObserver<T> observer) { }
    }
    public static class Subject
    {
        public static System.Reactive.Subjects.ISubject<T> Create<T>(System.IObserver<T> observer, System.IObservable<T> observable) { }
        public static System.Reactive.Subjects.ISubject<TSource, TResult> Create<TSource, TResult>(System.IObserver<TSource> observer, System.IObservable<TResult> observable) { }
        public static System.Reactive.Subjects.ISubject<TSource> Synchronize<TSource>(System.Reactive.Subjects.ISubject<TSource> subject) { }
        public static System.Reactive.Subjects.ISubject<TSource> Synchronize<TSource>(System.Reactive.Subjects.ISubject<TSource> subject, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Reactive.Subjects.ISubject<TSource, TResult> Synchronize<TSource, TResult>(System.Reactive.Subjects.ISubject<TSource, TResult> subject) { }
        public static System.Reactive.Subjects.ISubject<TSource, TResult> Synchronize<TSource, TResult>(System.Reactive.Subjects.ISubject<TSource, TResult> subject, System.Reactive.Concurrency.IScheduler scheduler) { }
    }
    public abstract class SubjectBase<T> : System.IDisposable, System.IObservable<T>, System.IObserver<T>, System.Reactive.Subjects.ISubject<T>, System.Reactive.Subjects.ISubject<T, T>
    {
        protected SubjectBase() { }
        public abstract bool HasObservers { get; }
        public abstract bool IsDisposed { get; }
        public abstract void Dispose();
        public abstract void OnCompleted();
        public abstract void OnError(System.Exception error);
        public abstract void OnNext(T value);
        public abstract System.IDisposable Subscribe(System.IObserver<T> observer);
    }
    public sealed class Subject<T> : System.Reactive.Subjects.SubjectBase<T>
    {
        public Subject() { }
        public override bool HasObservers { get; }
        public override bool IsDisposed { get; }
        public override void Dispose() { }
        public override void OnCompleted() { }
        public override void OnError(System.Exception error) { }
        public override void OnNext(T value) { }
        public override System.IDisposable Subscribe(System.IObserver<T> observer) { }
    }
}
namespace System.Reactive.Threading.Tasks
{
    public static class TaskObservableExtensions
    {
        public static System.IObservable<System.Reactive.Unit> ToObservable(this System.Threading.Tasks.Task task) { }
        public static System.IObservable<System.Reactive.Unit> ToObservable(this System.Threading.Tasks.Task task, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.IObservable<TResult> ToObservable<TResult>(this System.Threading.Tasks.Task<TResult> task) { }
        public static System.IObservable<TResult> ToObservable<TResult>(this System.Threading.Tasks.Task<TResult> task, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Threading.Tasks.Task<TResult> ToTask<TResult>(this System.IObservable<TResult> observable) { }
        public static System.Threading.Tasks.Task<TResult> ToTask<TResult>(this System.IObservable<TResult> observable, object state) { }
        public static System.Threading.Tasks.Task<TResult> ToTask<TResult>(this System.IObservable<TResult> observable, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Threading.Tasks.Task<TResult> ToTask<TResult>(this System.IObservable<TResult> observable, System.Threading.CancellationToken cancellationToken) { }
        public static System.Threading.Tasks.Task<TResult> ToTask<TResult>(this System.IObservable<TResult> observable, object state, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Threading.Tasks.Task<TResult> ToTask<TResult>(this System.IObservable<TResult> observable, System.Threading.CancellationToken cancellationToken, object state) { }
        public static System.Threading.Tasks.Task<TResult> ToTask<TResult>(this System.IObservable<TResult> observable, System.Threading.CancellationToken cancellationToken, System.Reactive.Concurrency.IScheduler scheduler) { }
        public static System.Threading.Tasks.Task<TResult> ToTask<TResult>(this System.IObservable<TResult> observable, System.Threading.CancellationToken cancellationToken, object state, System.Reactive.Concurrency.IScheduler scheduler) { }
    }
}
namespace System.Runtime.CompilerServices
{
    public struct TaskObservableMethodBuilder<T>
    {
        public System.Reactive.ITaskObservable<T> Task { get; }
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : System.Runtime.CompilerServices.INotifyCompletion
            where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
        [System.Security.SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion
            where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
        public void SetException(System.Exception exception) { }
        public void SetResult(T result) { }
        public void SetStateMachine(System.Runtime.CompilerServices.IAsyncStateMachine stateMachine) { }
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
        public static System.Runtime.CompilerServices.TaskObservableMethodBuilder<T> Create() { }
    }
}