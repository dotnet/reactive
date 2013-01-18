// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if NO_HASHSET
using System;
using System.Collections.Generic;

namespace System.Reactive
{
    class HashSet<T>
    {
        private readonly Dictionary<T, object> _set;
        private bool _hasNull;

        public HashSet(IEqualityComparer<T> comparer)
        {
            _set = new Dictionary<T, object>(comparer);
            _hasNull = false;
        }

        public bool Add(T value)
        {
            //
            // Note: The box instruction in the IL will be erased by the JIT in case T is
            //       a value type. See GroupBy for more information.
            //
            if (value == null)
            {
                if (_hasNull)
                    return false;

                _hasNull = true;
                return true;
            }
            else
            {
                if (_set.ContainsKey(value))
                    return false;

                _set[value] = null;
                return true;
            }
        }
    }
}
#endif