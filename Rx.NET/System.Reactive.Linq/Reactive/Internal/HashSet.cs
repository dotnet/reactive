// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if NO_HASHSET
using System;
using System.Collections.Generic;

namespace System.Reactive
{
    class HashSet<T>
    {
        private Dictionary<T, object> _set;

        public HashSet(IEqualityComparer<T> comparer)
        {
            _set = new Dictionary<T, object>(comparer);
        }

        public bool Add(T value)
        {
            if (_set.ContainsKey(value))
                return false;

            _set[value] = null;
            return true;
        }
    }
}
#endif