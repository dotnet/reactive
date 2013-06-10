// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Joins;
using System.Reactive.Linq;
using System.Reflection;

namespace System.Reactive
{
    internal class ObservableQueryProvider : IQbservableProvider, IQueryProvider
    {
        public IQbservable<TResult> CreateQuery<TResult>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            if (!typeof(IObservable<TResult>).IsAssignableFrom(expression.Type))
                throw new ArgumentException(Strings_Providers.INVALID_TREE_TYPE, "expression");

            return new ObservableQuery<TResult>(expression);
        }

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            //
            // Here we're on the edge between IQbservable and IQueryable for the local
            // execution case. E.g.:
            //
            //   observable.AsQbservable().<operators>.ToQueryable()
            //
            // This should be turned into a local execution, with the push-to-pull
            // adapter in the middle, so we rewrite to:
            //
            //   observable.AsQbservable().<operators>.ToEnumerable().AsQueryable()
            //
            var call = expression as MethodCallExpression;
            if (call == null || call.Method.DeclaringType != typeof(Qbservable) || call.Method.Name != "ToQueryable")
                throw new ArgumentException(Strings_Providers.EXPECTED_TOQUERYABLE_METHODCALL, "expression");

            //
            // This is the IQbservable<T> object corresponding to the lhs. Now wrap
            // it in two calls to get the local queryable.
            //
            var arg0 = call.Arguments[0];
            var res =
                Expression.Call(
                    AsQueryable.MakeGenericMethod(typeof(TElement)),
                    Expression.Call(
                        typeof(Observable).GetMethod("ToEnumerable").MakeGenericMethod(typeof(TElement)),
                        arg0
                    )
                );

            //
            // Queryable operator calls should be taken care of by the provider for
            // LINQ to Objects. So we compile and get the resulting IQueryable<T>
            // back to hand it out.
            //
            return Expression.Lambda<Func<IQueryable<TElement>>>(res).Compile()();
        }

        private static MethodInfo s_AsQueryable;

        private static MethodInfo AsQueryable
        {
            get
            {
                if (s_AsQueryable == null)
                    s_AsQueryable = Qbservable.InfoOf<object>(() => Queryable.AsQueryable<object>(null)).GetGenericMethodDefinition();

                return s_AsQueryable;
            }
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        object IQueryProvider.Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }

    internal class ObservableQuery
    {
        protected object _source;
        protected Expression _expression;

        public object Source
        {
            get { return _source; }
        }

        public Expression Expression
        {
            get { return _expression; }
        }
    }

    internal class ObservableQuery<TSource> : ObservableQuery, IQbservable<TSource>
    {
        internal ObservableQuery(IObservable<TSource> source)
        {
            _source = source;
            _expression = Expression.Constant(this);
        }

        internal ObservableQuery(Expression expression)
        {
            _expression = expression;
        }

        public Type ElementType
        {
            get { return typeof(TSource); }
        }

        public IQbservableProvider Provider
        {
            get { return Qbservable.Provider; }
        }

        public IDisposable Subscribe(IObserver<TSource> observer)
        {
            if (_source == null)
            {
                var rewriter = new ObservableRewriter();
                var body = rewriter.Visit(_expression);
                var f = Expression.Lambda<Func<IObservable<TSource>>>(body);
                _source = f.Compile()();
            }

            //
            // [OK] Use of unsafe Subscribe: non-pretentious mapping to IObservable<T> behavior equivalent to the expression tree.
            //
            return ((IObservable<TSource>)_source).Subscribe/*Unsafe*/(observer);
        }

        public override string ToString()
        {
            var c = _expression as ConstantExpression;
            if (c != null && c.Value == this)
            {
                if (_source != null)
                    return _source.ToString();

                return "null";
            }

            return _expression.ToString();
        }

        class ObservableRewriter : ExpressionVisitor
        {
            protected override Expression VisitConstant(ConstantExpression/*!*/ node)
            {
                var query = node.Value as ObservableQuery;
                if (query != null)
                {
                    var source = query.Source;
                    if (source != null)
                    {
                        return Expression.Constant(source);
                    }
                    else
                    {
                        return Visit(query.Expression);
                    }
                }

                return node;
            }

            protected override Expression VisitMethodCall(MethodCallExpression/*!*/ node)
            {
                var method = node.Method;
                var declaringType = method.DeclaringType;
#if (CRIPPLED_REFLECTION && HAS_WINRT)
                var baseType = declaringType.GetTypeInfo().BaseType;
#else
                var baseType = declaringType.BaseType;
#endif
                if (baseType == typeof(QueryablePattern))
                {
                    if (method.Name == "Then")
                    {
                        //
                        // Retarget Then to the corresponding pattern. Recursive visit of the lhs will rewrite
                        // the chain of And operators.
                        //
                        var pattern = Visit(node.Object);
                        var arguments = node.Arguments.Select(arg => Unquote(Visit(arg))).ToArray();
                        var then = Expression.Call(pattern, "Then", method.GetGenericArguments(), arguments);
                        return then;
                    }
                    else if (method.Name == "And")
                    {
                        //
                        // Retarget And to the corresponding pattern.
                        //
                        var lhs = Visit(node.Object);
                        var arguments = node.Arguments.Select(arg => Visit(arg)).ToArray();
                        var and = Expression.Call(lhs, "And", method.GetGenericArguments(), arguments);
                        return and;
                    }
                }
                else
                {
                    var arguments = node.Arguments.AsEnumerable();

                    //
                    // Checking for an IQbservable operator, being either:
                    // - an extension method on IQbservableProvider
                    // - an extension method on IQbservable<T>
                    //
                    var isOperator = false;

                    var firstParameter = method.GetParameters().FirstOrDefault();
                    if (firstParameter != null)
                    {
                        var firstParameterType = firstParameter.ParameterType;

                        //
                        // Operators like Qbservable.Amb have an n-ary form that take in an IQbservableProvider
                        // as the first argument. In such a case we need to make sure that the given provider is
                        // the one targeting regular Observable. If not, we keep the subtree as-is and let that
                        // provider handle the execution.
                        //
                        if (firstParameterType == typeof(IQbservableProvider))
                        {
                            isOperator = true;

                            //
                            // Since we could be inside a lambda expression where one tries to obtain a query
                            // provider, or that provider could be stored in an outer variable, we need to
                            // evaluate the expression to obtain an IQbservableProvider object.
                            //
                            var provider = Expression.Lambda<Func<IQbservableProvider>>(Visit(node.Arguments[0])).Compile()();

                            //
                            // Let's see whether the ObservableQuery provider is targeted. This one always goes
                            // to local execution. E.g.:
                            //
                            //   var xs = Observable.Return(1).AsQbservable();
                            //   var ys = Observable.Return(2).AsQbservable();
                            //   var zs = Observable.Return(3).AsQbservable();
                            //
                            //   var res = Qbservable.Provider.Amb(xs, ys, zs);
                            //             ^^^^^^^^^^^^^^^^^^^
                            //
                            if (provider is ObservableQueryProvider)
                            {
                                //
                                // For further rewrite, simply ignore the query provider argument now to match
                                // up with the Observable signature. E.g.:
                                //
                                //   var res = Qbservable.Provider.Amb(xs, ys, zs);
                                //           = Qbservable.Amb(Qbservable.Provider, xs, ys, zs)'
                                //                            ^^^^^^^^^^^^^^^^^^^
                                // ->
                                //   var res = Observable.Amb(xs, ys, zs);
                                //
                                arguments = arguments.Skip(1);
                            }
                            else
                            {
                                //
                                // We've hit an unknown provider and will defer further execution to it. Upon
                                // calling Subscribe to the node's output, that provider will take care of it.
                                //
                                return node;
                            }
                        }
                        else if (typeof(IQbservable).IsAssignableFrom(firstParameterType))
                        {
                            isOperator = true;
                        }
                    }

                    if (isOperator)
                    {
                        var args = VisitQbservableOperatorArguments(method, arguments);
                        return FindObservableMethod(method, args);
                    }
                }

                return base.VisitMethodCall(node);
            }

            private IList<Expression> VisitQbservableOperatorArguments(MethodInfo method, IEnumerable<Expression> arguments)
            {
                //
                // Recognize the Qbservable.When<TResult>(IQbservableProvider, QueryablePlan<TResult>[])
                // overload in order to substitute the array with a Plan<TResult>[].
                //
                if (method.Name == "When")
                {
                    var lastArgument = arguments.Last();
                    if (lastArgument.NodeType == ExpressionType.NewArrayInit)
                    {
                        var paramsArray = (NewArrayExpression)lastArgument;
                        return new List<Expression>
                        {
                            Expression.NewArrayInit(
                                typeof(Plan<>).MakeGenericType(method.GetGenericArguments()[0]),
                                paramsArray.Expressions.Select(param => Visit(param))
                            )
                        };
                    }
                }

                return arguments.Select(arg => Visit(arg)).ToList();
            }

            class Lazy<T>
            {
                private readonly Func<T> _factory;
                private T _value;
                private bool _initialized;

                public Lazy(Func<T> factory)
                {
                    _factory = factory;
                }

                public T Value
                {
                    get
                    {
                        lock (_factory)
                        {
                            if (!_initialized)
                            {
                                _value = _factory();
                                _initialized = true;
                            }
                        }

                        return _value;
                    }
                }
            }

            private static Lazy<ILookup<string, MethodInfo>> _observableMethods = new Lazy<ILookup<string, MethodInfo>>(() => GetMethods(typeof(Observable)));

            private static MethodCallExpression FindObservableMethod(MethodInfo method, IList<Expression> arguments)
            {
                //
                // Where to look for the matching operator?
                //
                var targetType = default(Type);
                var methods = default(ILookup<string, MethodInfo>);
                if (method.DeclaringType == typeof(Qbservable))
                {
                    targetType = typeof(Observable);
                    methods = _observableMethods.Value;
                }
                else
                {
                    targetType = method.DeclaringType;

#if (CRIPPLED_REFLECTION && HAS_WINRT)
                    var typeInfo = targetType.GetTypeInfo();
                    if (typeInfo.IsDefined(typeof(LocalQueryMethodImplementationTypeAttribute), false))
                    {
                        var mapping = (LocalQueryMethodImplementationTypeAttribute)typeInfo.GetCustomAttributes(typeof(LocalQueryMethodImplementationTypeAttribute), false).Single();
                        targetType = mapping.TargetType;
                    }
#else
                    if (targetType.IsDefined(typeof(LocalQueryMethodImplementationTypeAttribute), false))
                    {
                        var mapping = (LocalQueryMethodImplementationTypeAttribute)targetType.GetCustomAttributes(typeof(LocalQueryMethodImplementationTypeAttribute), false)[0];
                        targetType = mapping.TargetType;
                    }
#endif

                    methods = GetMethods(targetType);
                }

                //
                // From all the operators with the method's name, find the one that matches all arguments.
                //
                var typeArgs = method.IsGenericMethod ? method.GetGenericArguments() : null;
                var targetMethod = methods[method.Name].FirstOrDefault(candidateMethod => ArgsMatch(candidateMethod, arguments, typeArgs));
                if (targetMethod == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings_Providers.NO_MATCHING_METHOD_FOUND, method.Name, targetType.Name));

                //
                // Restore generic arguments.
                //
                if (typeArgs != null)
                    targetMethod = targetMethod.MakeGenericMethod(typeArgs);

                //
                // Finally, we need to deal with mismatches on Expression<Func<...>> versus Func<...>.
                //
                var parameters = targetMethod.GetParameters();
                for (int i = 0, n = parameters.Length; i < n; i++)
                {
                    arguments[i] = Unquote(arguments[i]);
                }

                //
                // Emit a new call to the discovered target method.
                //
                return Expression.Call(null, targetMethod, arguments);
            }

            private static ILookup<string, MethodInfo> GetMethods(Type type)
            {
#if !(CRIPPLED_REFLECTION && HAS_WINRT)
                return type.GetMethods(BindingFlags.Static | BindingFlags.Public).ToLookup(m => m.Name);
#else
                return type.GetTypeInfo().DeclaredMethods.Where(m => m.IsStatic && m.IsPublic).ToLookup(m => m.Name);
#endif
            }

            private static bool ArgsMatch(MethodInfo method, IList<Expression> arguments, Type[] typeArgs)
            {
                //
                // Number of parameters should match. Notice we've sanitized IQbservableProvider "this"
                // parameters first (see VisitMethodCall).
                //
                var parameters = method.GetParameters();
                if (parameters.Length != arguments.Count)
                    return false;

                //
                // Genericity should match too.
                //
                if (!method.IsGenericMethod && typeArgs != null && typeArgs.Length > 0)
                    return false;

                //
                // Reconstruct the generic method if needed.
                //
                if (method.IsGenericMethodDefinition)
                {
                    if (typeArgs == null)
                        return false;

                    if (method.GetGenericArguments().Length != typeArgs.Length)
                        return false;

                    var result = method.MakeGenericMethod(typeArgs);
                    parameters = result.GetParameters();
                }

                //
                // Check compatibility for the parameter types.
                //
                for (int i = 0, n = arguments.Count; i < n; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    var argument = arguments[i];

                    //
                    // For operators that take a function (like Where, Select), we'll be faced
                    // with a quoted argument and a discrepancy between Expression<Func<...>>
                    // and the underlying Func<...>.
                    //
                    if (!parameterType.IsAssignableFrom(argument.Type))
                    {
                        argument = Unquote(argument);
                        if (!parameterType.IsAssignableFrom(argument.Type))
                            return false;
                    }
                }

                return true;
            }

            private static Expression Unquote(Expression expression)
            {
                //
                // Get rid of all outer quotes around an expression.
                //
                while (expression.NodeType == ExpressionType.Quote)
                    expression = ((UnaryExpression)expression).Operand;

                return expression;
            }
        }
    }

#if (CRIPPLED_REFLECTION && HAS_WINRT)
    static class Helpers
    {
        public static MethodInfo GetMethod(this Type type, string name)
        {
            return type.GetTypeInfo().GetDeclaredMethod(name);
        }

        public static bool IsAssignableFrom(this Type type1, Type type2)
        {
            return type1.GetTypeInfo().IsAssignableFrom(type2.GetTypeInfo());
        }
    }
#endif
}