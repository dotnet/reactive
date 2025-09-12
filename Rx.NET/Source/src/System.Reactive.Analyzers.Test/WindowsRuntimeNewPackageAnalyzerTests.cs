// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Reactive.Analyzers.Test
{
    [TestClass]
    public sealed class WindowsRuntimeNewPackageAnalyzerTests
    {
        // This may need to look a bit different from the WPF and Windows Forms tests, because
        // this covers more types.
        // Then again, in the ADR, I recently made a change to say that actually the non-UI-framework-specific
        // Windows Runtime functionality should actually remain in System.Reactive.
        // There is an argumnet that anything that is inherently available as a result of having a Windows-specific TFM
        // could remain in System.Reactive. (These _don't_ bring in a whole extra framework dependency, so we don't have
        // the same absolute need to get rid of these as we do with WPF/Windows Forms,.) On the other hand, it would feel
        // odd for the WPF Dispatcher support to live in in the WPF component while the CoreDispatcher is in the main
        // System.Reactive.
        // The IEventPattern and IAsyncInfo bits are arguably an interesting special case.

        [TestMethod]
        public void ToDo()
        {
        }
    }
}
