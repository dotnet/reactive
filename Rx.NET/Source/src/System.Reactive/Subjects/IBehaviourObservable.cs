namespace System.Reactive.Subjects;

public interface IBehaviourObservable<out T> : IObservable<T>
{
    T Value { get; }
}
