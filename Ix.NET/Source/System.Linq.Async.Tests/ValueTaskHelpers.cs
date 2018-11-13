using System;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace System.Linq
{
    internal static class ValueTaskHelpers
    {
        public static void Wait<T>(this ValueTask<T> task, int timeOut)
        {
            task.AsTask().Wait(timeOut);
        }
    }
}

namespace Xunit
{
    internal static class AssertX
    {
        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type).
        /// </summary>
        /// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
        /// <param name="testCode">A delegate to the task to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown</exception>
        public static async Task<T> ThrowsAsync<T>(Func<ValueTask> testCode)
            where T : Exception
        {
            return (T)Throws(typeof(T), await RecordExceptionAsync(testCode));
        }

        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type).
        /// </summary>
        /// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
        /// <param name="testCode">A delegate to the task to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown</exception>
        public static async Task<T> ThrowsAsync<T>(Func<ValueTask<bool>> testCode)
            where T : Exception
        {
            return (T)Throws(typeof(T), await RecordExceptionAsync(testCode));
        }

        /// <summary>
        /// Records any exception which is thrown by the given task.
        /// </summary>
        /// <param name="testCode">The task which may thrown an exception.</param>
        /// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
        private static async Task<Exception> RecordExceptionAsync(Func<ValueTask> testCode)
        {
            if (testCode == null)
            {
                throw new ArgumentNullException(nameof(testCode));
            }

            try
            {
                await testCode();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// Records any exception which is thrown by the given task.
        /// </summary>
        /// <param name="testCode">The task which may thrown an exception.</param>
        /// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
        private static async Task<Exception> RecordExceptionAsync<T>(Func<ValueTask<T>> testCode)
        {
            if (testCode == null)
            {
                throw new ArgumentNullException(nameof(testCode));
            }

            try
            {
                await testCode();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static Exception Throws(Type exceptionType, Exception exception)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException(nameof(exceptionType));
            }

            if (exception == null)
                throw new ThrowsException(exceptionType);

            if (!exceptionType.Equals(exception.GetType()))
                throw new ThrowsException(exceptionType, exception);

            return exception;
        }
    }

}
