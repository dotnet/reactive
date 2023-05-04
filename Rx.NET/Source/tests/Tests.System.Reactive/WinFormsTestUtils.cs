// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if HAS_WINFORMS

using System;
using System.Threading;
using System.Windows.Forms;

namespace ReactiveTests.Tests
{
    internal static class WinFormsTestUtils
    {
        private static readonly Semaphore s_oneWinForms = new(1, 1);

        public static IDisposable RunTest(out Label label)
        {
            s_oneWinForms.WaitOne();

            var loaded = new ManualResetEvent(false);
            var lbl = default(Label);

            var t = new Thread(() =>
            {
                lbl = new Label();
                var frm = new Form { Controls = { lbl }, Width = 0, Height = 0, FormBorderStyle = FormBorderStyle.None, ShowInTaskbar = false };
                frm.Load += (_, __) =>
                {
                    loaded.Set();
                };
                Application.Run(frm);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            loaded.WaitOne();

            label = lbl;
            return new RunWinFormsTest(label, t);
        }

        private sealed class RunWinFormsTest : IDisposable
        {
            private readonly Label _lbl;
            private readonly Thread _t;

            public RunWinFormsTest(Label lbl, Thread t)
            {
                _lbl = lbl;
                _t = t;
            }

            public void Dispose()
            {
                _lbl.Invoke(new Action(() =>
                {
                    Application.Exit();
                }));

                _t.Join();

                s_oneWinForms.Release();
            }
        }
    }
}

#endif
