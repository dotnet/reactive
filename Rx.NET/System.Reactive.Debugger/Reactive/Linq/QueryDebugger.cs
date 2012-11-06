// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

//
// This file acts as a placeholder for future extension with a debugger service
// intercepting all Observable[Ex] query operators, instrumenting those with a
// set of debugger hooks. The code would boil down to a wrapper implementation
// of IQueryLanguage[Ex] providing instrumentation for query operators, which
// ultimately calls into the original "baseImpl" passed to the Extend method.
//
// Likely we want this code to be auto-generated based on certain patterns that
// occur frequently in query operators. Also, to ensure debugger and target are
// not going out of sync, we should properly version the interfaces and possibly
// perform a runtime check for the loaded assembly versions to ensure everything
// lines up correctly.
//

namespace System.Reactive.Linq
{
    /// <summary>
    /// (Infrastructure) Implement query debugger services.
    /// </summary>
    public class QueryDebugger : IQueryServices
    {
        T IQueryServices.Extend<T>(T baseImpl)
        {
            return baseImpl;
        }
    }
}
