// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if UseNonUiFrameworkSpecificRxDirectly || UseUiFrameworkSpecificRxDirectly
using System.Reactive.Concurrency;
using System.Reactive.Linq;
#endif

#if UseUiFrameworkSpecificRxDirectly
using System.Windows.Threading;
#endif

#if InvokeLibraryMethodThatUsesNonFrameworkSpecificRxFeature || InvokeLibraryMethodThatUsesUiFrameworkSpecificRxFeature
using Transitive.Lib.UsesRx;
#endif

internal class Program
{
    // RxLib.UseRxWpf creates a Dispatcher, so it needs to be running on an STA thread.
    [STAThread]
    private static async Task Main(string[] args)
    {
        // In some configs, we would end up with no await, causing a compiler warning.
        await Task.Yield();

#if UseNonUiFrameworkSpecificRxDirectly
        Console.WriteLine("App using Rx directly start");
        Console.WriteLine($"Rx (via app): {typeof(CurrentThreadScheduler).Assembly.FullName}");
        Observable.Range(1, 1).Subscribe(x => Console.WriteLine($"Received {x} from Observable.Range"));
        Console.WriteLine("App using Rx directly end");
        Console.WriteLine();
#endif

#if UseUiFrameworkSpecificRxDirectly
        Console.WriteLine("App using Rx UI directly start");
        Console.WriteLine($"Rx WPF (via lib): {typeof(System.Reactive.Concurrency.DispatcherScheduler).Assembly.FullName}");
        Observable.Range(1, 1).ObserveOn(Dispatcher.CurrentDispatcher).Subscribe(x => Console.WriteLine($"Received {x} from Observable.Range"));
        Console.WriteLine("Draining message loop after subscribe to ObserveOn(dispatcher)");
        DispatcherFrame frame = new();
        _ = Dispatcher.CurrentDispatcher.BeginInvoke(() => frame.Continue = false, DispatcherPriority.ContextIdle);
        Dispatcher.PushFrame(frame);
        Console.WriteLine("App using Rx UI directly end");
        Console.WriteLine();
#endif

#if InvokeLibraryMethodThatUsesNonFrameworkSpecificRxFeature
        Console.WriteLine("RxLib non-UI start");
        RxLib.UseRx(() => { Console.WriteLine("Callback from RxLib.UseRx"); });
        Console.WriteLine("Yielding after RxLib.UseRx");
        await Task.Yield();
        Console.WriteLine("RxLib non-UI end");
        Console.WriteLine();
#endif

#if InvokeLibraryMethodThatUsesUiFrameworkSpecificRxFeature
        Console.WriteLine();
        Console.WriteLine("RxLib UI start");
        RxLib.UseRxWpf(() => {  Console.WriteLine("Callback from RxLib.UseRxWpf"); });
        Console.WriteLine("RxLib UI end");
#endif
    }
}
