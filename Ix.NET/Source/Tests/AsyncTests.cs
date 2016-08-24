// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public partial class AsyncTests
    {
        public void AssertThrows<E>(Action a)
            where E : Exception
        {
            Assert.Throws<E>(a);
        }

        [Obsolete("Don't use this, use Assert.ThrowsAsync and await it", true)]
        public Task AssertThrows<E>(Func<Task> func)
            where E : Exception
        {
            return Assert.ThrowsAsync<E>(func);
        }

        public void AssertThrows<E>(Action a, Func<E, bool> assert)
            where E : Exception
        {

            var hasFailed = false;

            try
            {
                a();
            }
            catch (E e)
            {
                Assert.True(assert(e));
                hasFailed = true;
            }

            if (!hasFailed)
            {
                Assert.True(false);
            }
        }

        public void NoNext<T>(IAsyncEnumerator<T> e)
        {
            Assert.False(e.MoveNext().Result);
        }

        public void HasNext<T>(IAsyncEnumerator<T> e, T value)
        {
            Assert.True(e.MoveNext().Result);
            Assert.Equal(value, e.Current);
        }

        public async Task SequenceIdentity<T>(IAsyncEnumerable<T> enumerable)
        {
            Assert.True(await enumerable.SequenceEqual(enumerable, SequenceIdentityComparer<T>.Instance));
        }

        private class SequenceIdentityComparer<T> : IEqualityComparer<T>
        {
            readonly IEqualityComparer<T> innerComparer = EqualityComparer<T>.Default;
            public SequenceIdentityComparer()
            {
                var itemType = GetAnyElementType(typeof(T));

                // if not the same as T, then it's a list
                if (itemType != typeof(T))
                {
                    // invoke the Instance method of the type we need

                    var eqType = typeof(SequenceIdentityComparer<,>).MakeGenericType(typeof(T), itemType);
                    innerComparer = (IEqualityComparer<T>)eqType.GetRuntimeProperty("Instance").GetValue(null);
                }
            }


            public static SequenceIdentityComparer<T> Instance => new SequenceIdentityComparer<T>();
            public bool Equals(T x, T y)
            {
                return innerComparer.Equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return innerComparer.GetHashCode(obj);
            }

            static Type GetAnyElementType(Type type)
            {
                // Type is Array
                // short-circuit if you expect lots of arrays 
                if (typeof(Array).IsAssignableFrom(type))
                    return type.GetElementType();

                // type is IEnumerable<T>;
                if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return type.GetGenericArguments()[0];

                // type implements/extends IEnumerable<T>;
                var enumType = type.GetInterfaces()
                                        .Where(t => t.GetTypeInfo().IsGenericType &&
                                               t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                                        .Select(t => t.GenericTypeArguments[0]).FirstOrDefault();
                return enumType ?? type;
            }
        
        }


        private class SequenceIdentityComparer<TList, TItem> : IEqualityComparer<TList> where TList : IEnumerable<TItem>
        {
            readonly IEqualityComparer<TItem> innerComparer = EqualityComparer<TItem>.Default;


            public static IEqualityComparer<TList> Instance => new SequenceIdentityComparer<TList, TItem>();
            public bool Equals(TList x, TList y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(TList obj)
            {
                return obj.Aggregate(0, (i, item) =>
                                        {
                                            unchecked
                                            {
                                                i += innerComparer.GetHashCode(item);
                                            }
                                            return i;
                                        }
                                        );
            }
        }
    }
}