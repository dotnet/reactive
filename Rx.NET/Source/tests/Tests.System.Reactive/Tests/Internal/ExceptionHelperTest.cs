// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using Xunit;

namespace ReactiveTests.Tests
{

    public class ExceptionHelperTest
    {
        private Exception _errors;

        [Fact]
        public void ExceptionHelper_TrySetException_Empty()
        {
            var ex = new InvalidOperationException();

            Assert.True(ExceptionHelper.TrySetException(ref _errors, ex));

            Assert.Equal(ex, _errors);
        }

        [Fact]
        public void ExceptionHelper_TrySetException_Not_Empty()
        {
            var ex1 = new InvalidOperationException();
            _errors = ex1;

            var ex2 = new NotSupportedException();

            Assert.False(ExceptionHelper.TrySetException(ref _errors, ex2));

            Assert.Equal(ex1, _errors);
        }

        [Fact]
        public void ExceptionHelper_TrySetException_Terminate_Empty()
        {
            var ex = ExceptionHelper.Terminate(ref _errors);

            Assert.Null(ex);
            Assert.Equal(_errors, ExceptionHelper.Terminated);
        }

        [Fact]
        public void ExceptionHelper_TrySetException_Terminate_Not_Empty()
        {
            var ex1 = new InvalidOperationException();
            _errors = ex1;

            var ex = ExceptionHelper.Terminate(ref _errors);

            Assert.Equal(ex, ex1);
            Assert.Equal(_errors, ExceptionHelper.Terminated);
        }


        [Fact]
        public void ExceptionHelper_TrySetException_Terminate_Twice()
        {
            var ex1 = new InvalidOperationException();
            _errors = ex1;

            var ex = ExceptionHelper.Terminate(ref _errors);

            Assert.Equal(ex, ex1);
            Assert.Equal(_errors, ExceptionHelper.Terminated);

            ex = ExceptionHelper.Terminate(ref _errors);

            Assert.Equal(ex, ExceptionHelper.Terminated);
            Assert.Equal(_errors, ExceptionHelper.Terminated);
        }

        [Fact]
        public void ExceptionHelper_TryAddException_Empty()
        {
            var ex1 = new InvalidOperationException();

            Assert.True(ExceptionHelper.TryAddException(ref _errors, ex1));

            Assert.Equal(ex1, _errors);
        }

        [Fact]
        public void ExceptionHelper_TryAddException_Not_Empty()
        {
            var ex1 = new InvalidOperationException();
            _errors = ex1;

            var ex2 = new NotImplementedException();

            Assert.True(ExceptionHelper.TryAddException(ref _errors, ex2));

            Assert.True(_errors is AggregateException);
            var x = _errors as AggregateException;

            Assert.Equal(2, x.InnerExceptions.Count);
            Assert.True(x.InnerExceptions[0] is InvalidOperationException);
            Assert.True(x.InnerExceptions[1] is NotImplementedException);
        }

        [Fact]
        public void ExceptionHelper_TryAddException_Aggregated()
        {
            var ex1 = new InvalidOperationException();
            var ex2 = new NotImplementedException();

            _errors = new AggregateException(ex1, ex2);

            var ex3 = new InvalidCastException();

            Assert.True(ExceptionHelper.TryAddException(ref _errors, ex3));

            Assert.True(_errors is AggregateException);
            var x = _errors as AggregateException;

            Assert.Equal(3, x.InnerExceptions.Count);
            Assert.True(x.InnerExceptions[0] is InvalidOperationException);
            Assert.True(x.InnerExceptions[1] is NotImplementedException);
            Assert.True(x.InnerExceptions[2] is InvalidCastException);
        }

        [Fact]
        public void ExceptionHelper_TryAddException_Terminated()
        {
            _errors = ExceptionHelper.Terminated;

            var ex = new InvalidCastException();

            Assert.False(ExceptionHelper.TryAddException(ref _errors, ex));

            Assert.Equal(_errors, ExceptionHelper.Terminated);
        }
    }
}
