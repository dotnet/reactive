// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if SILVERLIGHT && !SILVERLIGHTM7
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Browser;
using Microsoft.Silverlight.Testing;

namespace ReactiveTests
{
    public class App : Application
    {
        public App()
        {
            this.Startup += (o, e) =>
            {
                // TODO: Investigate UnitTestSettings configuration of TestService and LogProviders.
                // var settings = new UnitTestSettings { StartRunImmediately = true };
                RootVisual = UnitTestSystem.CreateTestPage(/* settings */);
            };

            this.UnhandledException += (o, e) =>
            {
                if (!Debugger.IsAttached)
                {
                    e.Handled = true;
                    Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
                }
            };
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
#endif