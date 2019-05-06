using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Xunit;

namespace ReactiveTests
{
    public class SingleAssignmentDisposableTests
    {
        [Fact]
        public async Task SingleAssignmentDisposable_DisposeAfterSet()
        {
            var disposed = false;

            var d = new SingleAssignmentAsyncDisposable();
            var dd = AsyncDisposable.Create(() => { disposed = true; return Task.CompletedTask; });
            await d.AssignAsync(dd);

            Assert.False(disposed);
            await d.DisposeAsync();
            Assert.True(disposed);
            await d.DisposeAsync();
            Assert.True(disposed);

            Assert.True(d.IsDisposed);
        }

        [Fact]
        public async Task SingleAssignmentDisposable_DisposeBeforeSet()
        {
            var disposed = false;

            var d = new SingleAssignmentAsyncDisposable();
            var dd = AsyncDisposable.Create(() => { disposed = true; return Task.CompletedTask; });

            Assert.False(disposed);
            await d.DisposeAsync();
            Assert.False(disposed);
            Assert.True(d.IsDisposed);

            await d.AssignAsync(dd);
            Assert.True(disposed);
            await d.DisposeAsync();        // This should be a nop.
            Assert.True(disposed);
        }

        [Fact]
        public async Task SingleAssignmentDisposable_SetMultipleTimes()
        {
            var d = new SingleAssignmentAsyncDisposable();

            await d.AssignAsync(AsyncDisposable.Nop);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await d.AssignAsync(AsyncDisposable.Nop));
        }
    }
}
