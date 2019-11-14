// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// Rewrites an expression tree representation using AsyncQueryable methods to the corresponding AsyncEnumerable equivalents.
    /// </summary>
    internal class AsyncEnumerableRewriter : ExpressionVisitor
    {
        private static volatile ILookup<string, MethodInfo>? _methods;

        protected override Expression VisitConstant(ConstantExpression node)
        {
            //
            // Not an expression representation obtained from the async enumerable query provider,
            // so just a plain constant that can be returned as-is.
            //
            if (!(node.Value is AsyncEnumerableQuery enumerableQuery))
            {
                return node;
            }

            //
            // Expression representation obtained from the async enumerable query provider, so first
            // check whether it wraps an enumerable sequence that has been evaluated already.
            //
            if (enumerableQuery.Enumerable != null)
            {
                var publicType = GetPublicType(enumerableQuery.Enumerable.GetType());
                return Expression.Constant(enumerableQuery.Enumerable, publicType);
            }

            //
            // If not evaluated yet, inline the expression representation.
            //
            return Visit(enumerableQuery.Expression);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var obj = Visit(node.Object);
            var args = Visit(node.Arguments);

            //
            // Nothing changed during the visit; just some unrelated method call that can
            // be returned as-is.
            //
            if (obj == node.Object && args == node.Arguments)
            {
                return node;
            }

            var declType = node.Method.DeclaringType;
            var typeArgs = node.Method.IsGenericMethod ? node.Method.GetGenericArguments() : null;

            //
            // Check whether the method is compatible with the recursively rewritten instance
            // and arguments expressions. If so, create a new call expression.
            //
            if ((node.Method.IsStatic || declType != null && declType.IsAssignableFrom(obj.Type)) && ArgsMatch(node.Method, args, typeArgs))
            {
                return Expression.Call(obj, node.Method, args);
            }

            MethodInfo method;

            //
            // Find a corresponding method in the non-expression world, e.g. rewriting from
            // the AsyncQueryable methods to the ones on AsyncEnumerable.
            //
            if (declType == typeof(AsyncQueryable))
            {
                method = FindEnumerableMethod(node.Method.Name, args, typeArgs);
                args = FixupQuotedArgs(method, args);
                return Expression.Call(obj, method, args);
            }
            else
            {
                if (declType == null)
                {
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Could not rewrite method with name '{0}' without a DeclaringType.", node.Method.Name));
                }

                method = FindMethod(declType, node.Method.Name, args, typeArgs, BindingFlags.Static | (node.Method.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic));
                args = FixupQuotedArgs(method, args);
            }

            return Expression.Call(obj, method, args);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            //
            // Don't recurse into lambdas; all the ones returning IAsyncQueryable<T>
            // are compatible with their IAsyncEnumerable<T> counterparts due to the
            // covariant return type.
            //
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            //
            // See remark on VisitLambda.
            //
            return node;
        }

        private static Type GetPublicType(Type type)
        {
            if (!type.GetTypeInfo().IsNestedPrivate)
            {
                return type;
            }

            foreach (var ifType in type.GetInterfaces())
            {
                if (ifType.GetTypeInfo().IsGenericType)
                {
                    var def = ifType.GetGenericTypeDefinition();
                    if (def == typeof(IAsyncEnumerable<>) || def == typeof(IAsyncGrouping<,>))
                    {
                        return ifType;
                    }
                }
            }

            //
            // NB: Add if we ever decide to add the non-generic interface.
            //
            //if (typeof(IAsyncEnumerable).IsAssignableFrom(type))
            //{
            //    return typeof(IAsyncEnumerable);
            //}

            return type;
        }

        private static bool ArgsMatch(MethodInfo method, ReadOnlyCollection<Expression> args, Type[]? typeArgs)
        {
            //
            // Number of parameters should match the number of arguments to bind.
            //
            var parameters = method.GetParameters();
            if (parameters.Length != args.Count)
            {
                return false;
            }

            //
            // Both should be generic or non-generic.
            //
            if (!method.IsGenericMethod && typeArgs != null && typeArgs.Length != 0)
            {
                return false;
            }

            //
            // Closed generic methods need to get converted to their open generic counterpart.
            //
            if (!method.IsGenericMethodDefinition && method.IsGenericMethod && method.ContainsGenericParameters)
            {
                method = method.GetGenericMethodDefinition();
            }

            //
            // For generic methods, close the candidate using the specified type arguments.
            //
            if (method.IsGenericMethodDefinition)
            {
                //
                // We should have at least 1 type argument.
                //
                if (typeArgs == null || typeArgs.Length == 0)
                {
                    return false;
                }

                //
                // The number of type arguments needed should match the specified type argument count.
                //
                if (method.GetGenericArguments().Length != typeArgs.Length)
                {
                    return false;
                }

                //
                // Close the generic method and re-obtain the parameters.
                //
                method = method.MakeGenericMethod(typeArgs);
                parameters = method.GetParameters();
            }

            //
            // Check for contravariant assignability of each parameter.
            //
            for (var i = 0; i < args.Count; i++)
            {
                var type = parameters[i].ParameterType;

                //
                // Hardening against reflection quirks.
                //
                if (type == null)
                {
                    return false;
                }

                //
                // Deal with ref or out parameters by using the element type which can
                // match the corresponding expression type (ref passing is not encoded
                // in the type of expression trees).
                //
                if (type.IsByRef)
                {
                    type = type.GetElementType()!;
                }

                var expression = args[i];

                //
                // If the expression is assignable to the parameter, all is good. If not,
                // it's possible there's a match because we're dealing with a quote that
                // needs to be unpacked.
                //
                if (!type.IsAssignableFrom(expression.Type))
                {
                    //
                    // Unpack the quote, if any. See AsyncQueryable for examples of operators
                    // that hit this case.
                    //
                    if (expression.NodeType == ExpressionType.Quote)
                    {
                        expression = ((UnaryExpression)expression).Operand;
                    }

                    //
                    // Try assigning the raw expression type or the quote-free expression type
                    // to the parameter. If none of these work, there's no match.
                    //
                    if (!type.IsAssignableFrom(expression.Type) && !type.IsAssignableFrom(StripExpression(expression.Type)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static ReadOnlyCollection<Expression> FixupQuotedArgs(MethodInfo method, ReadOnlyCollection<Expression> argList)
        {
            //
            // Get all of the method parameters. No fix-up needed if empty.
            //
            var parameters = method.GetParameters();
            if (parameters.Length != 0)
            {
                var list = default(List<Expression>);

                //
                // Process all parameters. If any fixup is needed, the list will
                // get assigned.
                //
                for (var i = 0; i < parameters.Length; i++)
                {
                    var expression = argList[i];
                    var parameterInfo = parameters[i];

                    //
                    // Perform the fix-up if needed and check the outcome. If a
                    // change was made, the list is lazily allocated.
                    //
                    expression = FixupQuotedExpression(parameterInfo.ParameterType, expression);

                    if (list == null && expression != argList[i])
                    {
                        list = new List<Expression>(argList.Count);

                        for (var j = 0; j < i; j++)
                        {
                            list.Add(argList[j]);
                        }
                    }

                    if (list != null)
                    {
                        list.Add(expression);
                    }
                }

                //
                // If any argument was fixed up, return a new argument list.
                //
                if (list != null)
                {
                    argList = new ReadOnlyCollection<Expression>(list);
                }
            }

            return argList;
        }

        private static Expression FixupQuotedExpression(Type type, Expression expression)
        {
            var res = expression;

            //
            // Keep unquoting until assignability checks pass.
            //
            while (!type.IsAssignableFrom(res.Type))
            {
                //
                // In case this is not a quote, bail out early.
                //
                if (res.NodeType != ExpressionType.Quote)
                {
                    //
                    // Array initialization expressions need special care by unquoting the elements.
                    //
                    if (!type.IsAssignableFrom(res.Type) && type.IsArray && res.NodeType == ExpressionType.NewArrayInit)
                    {
                        var unquotedType = StripExpression(res.Type);
                        if (type.IsAssignableFrom(unquotedType))
                        {
                            var newArrayExpression = (NewArrayExpression)res;

                            var count = newArrayExpression.Expressions.Count;
                            var elementType = type.GetElementType()!;
                            var list = new List<Expression>(count);

                            for (var i = 0; i < count; i++)
                            {
                                list.Add(FixupQuotedExpression(elementType, newArrayExpression.Expressions[i]));
                            }

                            expression = Expression.NewArrayInit(elementType, list);
                        }
                    }

                    return expression;
                }

                //
                // Unquote and try again; at most two passes should be needed.
                //
                res = ((UnaryExpression)res).Operand;
            }

            return res;
        }

        private static Type StripExpression(Type type)
        {
            //
            // Array of quotes need to be stripped, so extract the element type.
            //
            var elemType = type.IsArray ? type.GetElementType()! : type;

            //
            // Try to find Expression<T> and obtain T.
            //
            var genType = FindGenericType(typeof(Expression<>), elemType);
            if (genType != null)
            {
                elemType = genType.GetGenericArguments()[0];
            }

            //
            // Not an array, nothing to do here.
            //
            if (!type.IsArray)
            {
                return type;
            }

            //
            // Reconstruct the array type from the stripped element type.
            //
            var arrayRank = type.GetArrayRank();
            if (arrayRank != 1)
            {
                return elemType.MakeArrayType(arrayRank);
            }

            return elemType.MakeArrayType();
        }

        private static MethodInfo FindEnumerableMethod(string name, ReadOnlyCollection<Expression> args, params Type[]? typeArgs)
        {
            //
            // Ensure the cached lookup table for AsyncEnumerable methods is initialized.
            //
            if (_methods == null)
            {
                _methods = typeof(AsyncEnumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).ToLookup(m => m.Name);
            }

            //
            // Find a match based on the method name and the argument types.
            //
            var method = _methods[name].FirstOrDefault(m => ArgsMatch(m, args, typeArgs));
            if (method == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find method with name '{0}' on type '{1}'.", name, typeof(Enumerable)));
            }

            //
            // Close the generic method if needed.
            //
            if (typeArgs != null)
            {
                return method.MakeGenericMethod(typeArgs);
            }

            return method;
        }

        private static MethodInfo FindMethod(Type type, string name, ReadOnlyCollection<Expression> args, Type[]? typeArgs, BindingFlags flags)
        {
            //
            // Support the enumerable methods to be defined on another type.
            //
            var targetType = type.GetTypeInfo().GetCustomAttribute<LocalQueryMethodImplementationTypeAttribute>()?.TargetType ?? type;

            //
            // Get all the candidates based on name and fail if none are found.
            //
            var methods = targetType.GetMethods(flags).Where(m => m.Name == name).ToArray();
            if (methods.Length == 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find method with name '{0}' on type '{1}'.", name, type));
            }

            //
            // Find a match based on arguments and fail if no match is found.
            //
            var method = methods.FirstOrDefault(m => ArgsMatch(m, args, typeArgs));
            if (method == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find a matching method with name '{0}' on type '{1}'.", name, type));
            }

            //
            // Close the generic method if needed.
            //
            if (typeArgs != null)
            {
                return method.MakeGenericMethod(typeArgs);
            }

            return method;
        }

        private static Type? FindGenericType(Type definition, Type? type)
        {
            while (type != null && type != typeof(object))
            {
                //
                // If the current type matches the specified definition, return.
                //
                if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == definition)
                {
                    return type;
                }

                //
                // Probe all interfaces implemented by the current type.
                //
                if (definition.GetTypeInfo().IsInterface)
                {
                    foreach (var ifType in type.GetInterfaces())
                    {
                        var res = FindGenericType(definition, ifType);
                        if (res != null)
                        {
                            return res;
                        }
                    }
                }

                //
                // Continue up the type hierarchy.
                //
                type = type.GetTypeInfo().BaseType;
            }

            return null;
        }
    }
}
