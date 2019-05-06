namespace System.Reactive.Disposables
{
    public interface IAsyncCancelable : IAsyncDisposable
    {
        bool IsDisposed { get; }
    }
}
