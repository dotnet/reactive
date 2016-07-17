// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CreateEnumerable(() =>
                          {
                              var i = initialState;
                              var started = false;
                              var current = default(TResult);

                              return CreateEnumerator(
                                  ct =>
                                  {
                                      var b = false;
                                      try
                                      {
                                          if (started)
                                              i = iterate(i);

                                          b = condition(i);

                                          if (b)
                                              current = resultSelector(i);
                                      }
                                      catch (Exception ex)
                                      {
                                          return TaskExt.Throw<bool>(ex);
                                      }

                                      if (!b)
                                          return TaskExt.False;

                                      if (!started)
                                          started = true;

                                      return TaskExt.True;
                                  },
                                  () => current,
                                  () => { }
                              );
                          });
        }
    }
}