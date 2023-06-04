namespace System.Reactive.Subjects;

public interface IBehaviorObservable<out T> : IObservable<T>
{
    T Value { get; }
}
