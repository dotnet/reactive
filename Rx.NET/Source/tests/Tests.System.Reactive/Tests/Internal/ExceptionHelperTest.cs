// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ExceptionHelperTest
    {
        private Exception _errors;

        [TestMethod]
        public void ExceptionHelper_TrySetException_Empty()
        {
            var ex = new InvalidOperationException();

            Assert.True(ExceptionHelper.TrySetException(ref _errors, ex));

            Assert.Equal(ex, _errors);
        }

        [TestMethod]
        public void ExceptionHelper_TrySetException_Not_Empty()
        {
            var ex1 = new InvalidOperationException();
            _errors = ex1;

            var ex2 = new NotSupportedException();

            Assert.False(ExceptionHelper.TrySetException(ref _errors, ex2));

            Assert.Equal(ex1, _errors);
        }
    }
}
