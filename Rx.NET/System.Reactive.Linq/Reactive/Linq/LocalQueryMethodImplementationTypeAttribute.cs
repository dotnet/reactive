// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.ComponentModel;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Attribute applied to static classes providing expression tree forms of query methods,
    /// mapping those to the corresponding methods for local query execution on the specified
    /// target class type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class LocalQueryMethodImplementationTypeAttribute : Attribute
    {
        private readonly Type _targetType;

        /// <summary>
        /// Creates a new mapping to the specified local execution query method implementation type.
        /// </summary>
        /// <param name="targetType">Type with query methods for local execution.</param>
        public LocalQueryMethodImplementationTypeAttribute(Type targetType)
        {
            _targetType = targetType;
        }

        /// <summary>
        /// Gets the type with the implementation of local query methods.
        /// </summary>
        public Type TargetType
        {
            get { return _targetType; }
        }
    }
}
