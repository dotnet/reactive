using System;
using System.Linq.Expressions;
using System.Reflection;
using Force.DeepCloner;

namespace Redux.NET
{
    public static class ReducerExtensions
    {
        public static T Spread<T, P>(this T target, Expression<Func<T, P>> property, P value)
        {
            var clone = target.DeepClone();
            var slice = property.Slice();

            ReflectionExtensions.Set(slice, clone, value);

            return clone;
        }
    }
}
