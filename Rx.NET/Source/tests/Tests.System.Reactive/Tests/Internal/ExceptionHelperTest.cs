// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using Xunit;
using System;

namespace ReactiveTests.Tests
{
    
    public class ExceptionHelperTest
    {
        Exception errors;

        [Fact]
        public void ExceptionHelper_TrySetException_Empty()
        {
            var ex = new InvalidOperationException();

            Assert.True(ExceptionHelper.TrySetException(ref errors, ex));

            Assert.Equal(ex, errors);
        }

        [Fact]
        public void ExceptionHelper_TrySetException_Not_Empty()
        {
            var ex1 = new InvalidOperationException();
            errors = ex1;

            var ex2 = new NotSupportedException();

            Assert.False(ExceptionHelper.TrySetException(ref errors, ex2));

            Assert.Equal(ex1, errors);
        }

        [Fact]
        public void ExceptionHelper_TrySetException_Terminate_Empty()
        {
            var ex = ExceptionHelper.Terminate(ref errors);

            Assert.Null(ex);
            Assert.Equal(errors, ExceptionHelper.Terminated);
        }

        [Fact]
        public void ExceptionHelper_TrySetException_Terminate_Not_Empty()
        {
            var ex1 = new InvalidOperationException();
            errors = ex1;

            var ex = ExceptionHelper.Terminate(ref errors);

            Assert.Equal(ex, ex1);
            Assert.Equal(errors, ExceptionHelper.Terminated);
        }


        [Fact]
        public void ExceptionHelper_TrySetException_Terminate_Twice()
        {
            var ex1 = new InvalidOperationException();
            errors = ex1;

            var ex = ExceptionHelper.Terminate(ref errors);

            Assert.Equal(ex, ex1);
            Assert.Equal(errors, ExceptionHelper.Terminated);

            ex = ExceptionHelper.Terminate(ref errors);

            Assert.Equal(ex, ExceptionHelper.Terminated);
            Assert.Equal(errors, ExceptionHelper.Terminated);
        }

        [Fact]
        public void ExceptionHelper_TryAddException_Empty()
        {
            var ex1 = new InvalidOperationException();

            Assert.True(ExceptionHelper.TryAddException(ref errors, ex1));

            Assert.Equal(ex1, errors);
        }

        [Fact]
        public void ExceptionHelper_TryAddException_Not_Empty()
        {
            var ex1 = new InvalidOperationException();
            errors = ex1;

            var ex2 = new NotImplementedException();

            Assert.True(ExceptionHelper.TryAddException(ref errors, ex2));

            Assert.True(errors is AggregateException);
            var x = errors as AggregateException;

            Assert.Equal(2, x.InnerExceptions.Count);
            Assert.True(x.InnerExceptions[0] is InvalidOperationException);
            Assert.True(x.InnerExceptions[1] is NotImplementedException);
        }

        [Fact]
        public void ExceptionHelper_TryAddException_Aggregated()
        {
            var ex1 = new InvalidOperationException();
            var ex2 = new NotImplementedException();

            errors = new AggregateException(ex1, ex2);

            var ex3 = new InvalidCastException();

            Assert.True(ExceptionHelper.TryAddException(ref errors, ex3));

            Assert.True(errors is AggregateException);
            var x = errors as AggregateException;

            Assert.Equal(3, x.InnerExceptions.Count);
            Assert.True(x.InnerExceptions[0] is InvalidOperationException);
            Assert.True(x.InnerExceptions[1] is NotImplementedException);
            Assert.True(x.InnerExceptions[2] is InvalidCastException);
        }

        [Fact]
        public void ExceptionHelper_TryAddException_Terminated()
        {
            errors = ExceptionHelper.Terminated;

            var ex = new InvalidCastException();

            Assert.False(ExceptionHelper.TryAddException(ref errors, ex));

            Assert.Equal(errors, ExceptionHelper.Terminated);
        }
    }
}
