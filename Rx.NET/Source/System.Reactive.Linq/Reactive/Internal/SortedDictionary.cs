// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if NO_SORTEDDICTIONARY

using System;
using System.Diagnostics;
    
//
// Code ported from SortedDictionary.cs and SortedSet.cs in the BCL.
// Unused portions have been removed for brevity.
//

namespace System.Collections.Generic
{
    class SortedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly TreeSet<KeyValuePair<TKey, TValue>> _set;

        public SortedDictionary()
        {
            _set = new TreeSet<KeyValuePair<TKey, TValue>>(new KeyValuePairComparer());
        }

        public void Add(TKey key, TValue value)
        {
            _set.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public int Count
        {
            get
            {
                return _set.Count;
            }
        }

        public bool Remove(TKey key)
        {
            return _set.Remove(new KeyValuePair<TKey, TValue>(key, default(TValue)));
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly TreeSet<KeyValuePair<TKey, TValue>>.Enumerator _treeEnum;

            internal Enumerator(SortedDictionary<TKey, TValue> dictionary)
            {
                _treeEnum = dictionary._set.GetEnumerator();
            }

            public bool MoveNext()
            {
                return _treeEnum.MoveNext();
            }

            public void Dispose()
            {
                _treeEnum.Dispose();
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    return _treeEnum.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return new KeyValuePair<TKey, TValue>(Current.Key, Current.Value);
                }
            }

            internal void Reset()
            {
                _treeEnum.Reset();
            }

            void IEnumerator.Reset()
            {
                _treeEnum.Reset();
            }
        }

        internal class KeyValuePairComparer : Comparer<KeyValuePair<TKey, TValue>>
        {
            private readonly IComparer<TKey> _keyComparer = Comparer<TKey>.Default;

            public override int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return _keyComparer.Compare(x.Key, y.Key);
            }
        }

        internal class TreeSet<T> : SortedSet<T>
        {
            public TreeSet(IComparer<T> comparer)
                : base(comparer)
            {
            }

            internal override bool AddIfNotPresent(T item)
            {
                var ret = base.AddIfNotPresent(item);

                if (!ret)
                {
                    throw new ArgumentException("Duplicate entry added.");
                }

                return ret;
            }
        }
    }

    class SortedSet<T> : IEnumerable<T>
    {
        private readonly IComparer<T> _comparer;
        private Node _root;
        private int _count;
        private int _version;

        public SortedSet(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public bool Add(T item)
        {
            return AddIfNotPresent(item);
        }

        internal virtual bool AddIfNotPresent(T item)
        {
            if (_root == null)
            {
                _root = new Node(item, false);
                _count = 1;
                _version++;
                return true;
            }

            //
            // Search for a node at bottom to insert the new node. 
            // If we can guarantee the node we found is not a 4-node, it would be easy to do insertion.
            // We split 4-nodes along the search path.
            // 
            Node current = _root;
            Node parent = null;
            Node grandParent = null;
            Node greatGrandParent = null;

            //
            // Even if we don't actually add to the set, we may be altering its structure (by doing rotations
            // and such). so update version to disable any enumerators/subsets working on it.
            //
            _version++;

            int order = 0;
            while (current != null)
            {
                order = _comparer.Compare(item, current.Item);
                if (order == 0)
                {
                    // We could have changed root node to red during the search process.
                    // We need to set it to black before we return.
                    _root.IsRed = false;
                    return false;
                }

                // split a 4-node into two 2-nodes                
                if (Is4Node(current))
                {
                    Split4Node(current);
                    // We could have introduced two consecutive red nodes after split. Fix that by rotation.
                    if (IsRed(parent))
                    {
                        InsertionBalance(current, ref parent, grandParent, greatGrandParent);
                    }
                }

                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = (order < 0) ? current.Left : current.Right;
            }

            Debug.Assert(parent != null, "Parent node cannot be null here!");
            
            var node = new Node(item);
            if (order > 0)
            {
                parent.Right = node;
            }
            else
            {
                parent.Left = node;
            }

            //
            // The new node will be red, so we will need to adjust the colors if parent node is also red.
            //
            if (parent.IsRed)
            {
                InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            }

            //
            // Root node is always black.
            //
            _root.IsRed = false;
            ++_count;
            return true;
        }

        public bool Remove(T item)
        {
            return DoRemove(item); // hack so it can be made non-virtual
        }

        internal virtual bool DoRemove(T item)
        {
            if (_root == null)
            {
                return false;
            }

            // Search for a node and then find its succesor. 
            // Then copy the item from the succesor to the matching node and delete the successor. 
            // If a node doesn't have a successor, we can replace it with its left child (if not empty.) 
            // or delete the matching node.
            // 
            // In top-down implementation, it is important to make sure the node to be deleted is not a 2-node.
            // Following code will make sure the node on the path is not a 2 Node. 

            //
            // Even if we don't actually remove from the set, we may be altering its structure (by doing rotations
            // and such). so update version to disable any enumerators/subsets working on it.
            //
            _version++;

            Node current = _root;
            Node parent = null;
            Node grandParent = null;
            Node match = null;
            Node parentOfMatch = null;
            bool foundMatch = false;
            while (current != null)
            {
                if (Is2Node(current))
                { // fix up 2-Node
                    if (parent == null)
                    {   // current is root. Mark it as red
                        current.IsRed = true;
                    }
                    else
                    {
                        Node sibling = GetSibling(current, parent);
                        if (sibling.IsRed)
                        {
                            // If parent is a 3-node, flip the orientation of the red link. 
                            // We can acheive this by a single rotation        
                            // This case is converted to one of other cased below.
                            Debug.Assert(!parent.IsRed, "parent must be a black node!");
                            if (parent.Right == sibling)
                            {
                                RotateLeft(parent);
                            }
                            else
                            {
                                RotateRight(parent);
                            }

                            parent.IsRed = true;
                            sibling.IsRed = false;    // parent's color
                            // sibling becomes child of grandParent or root after rotation. Update link from grandParent or root
                            ReplaceChildOfNodeOrRoot(grandParent, parent, sibling);
                            // sibling will become grandParent of current node 
                            grandParent = sibling;
                            if (parent == match)
                            {
                                parentOfMatch = sibling;
                            }

                            // update sibling, this is necessary for following processing
                            sibling = (parent.Left == current) ? parent.Right : parent.Left;
                        }
                        Debug.Assert(sibling != null || sibling.IsRed == false, "sibling must not be null and it must be black!");

                        if (Is2Node(sibling))
                        {
                            Merge2Nodes(parent, current, sibling);
                        }
                        else
                        {
                            // current is a 2-node and sibling is either a 3-node or a 4-node.
                            // We can change the color of current to red by some rotation.
                            TreeRotation rotation = RotationNeeded(parent, current, sibling);
                            Node newGrandParent = null;
                            switch (rotation)
                            {
                                case TreeRotation.RightRotation:
                                    Debug.Assert(parent.Left == sibling, "sibling must be left child of parent!");
                                    Debug.Assert(sibling.Left.IsRed, "Left child of sibling must be red!");
                                    sibling.Left.IsRed = false;
                                    newGrandParent = RotateRight(parent);
                                    break;
                                case TreeRotation.LeftRotation:
                                    Debug.Assert(parent.Right == sibling, "sibling must be left child of parent!");
                                    Debug.Assert(sibling.Right.IsRed, "Right child of sibling must be red!");
                                    sibling.Right.IsRed = false;
                                    newGrandParent = RotateLeft(parent);
                                    break;

                                case TreeRotation.RightLeftRotation:
                                    Debug.Assert(parent.Right == sibling, "sibling must be left child of parent!");
                                    Debug.Assert(sibling.Left.IsRed, "Left child of sibling must be red!");
                                    newGrandParent = RotateRightLeft(parent);
                                    break;

                                case TreeRotation.LeftRightRotation:
                                    Debug.Assert(parent.Left == sibling, "sibling must be left child of parent!");
                                    Debug.Assert(sibling.Right.IsRed, "Right child of sibling must be red!");
                                    newGrandParent = RotateLeftRight(parent);
                                    break;
                            }

                            newGrandParent.IsRed = parent.IsRed;
                            parent.IsRed = false;
                            current.IsRed = true;
                            ReplaceChildOfNodeOrRoot(grandParent, parent, newGrandParent);
                            if (parent == match)
                            {
                                parentOfMatch = newGrandParent;
                            }
                            grandParent = newGrandParent;
                        }
                    }
                }

                // we don't need to compare any more once we found the match
                int order = foundMatch ? -1 : _comparer.Compare(item, current.Item);
                if (order == 0)
                {
                    // save the matching node
                    foundMatch = true;
                    match = current;
                    parentOfMatch = parent;
                }

                grandParent = parent;
                parent = current;

                if (order < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;       // continue the search in  right sub tree after we find a match
                }
            }

            // move successor to the matching node position and replace links
            if (match != null)
            {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
                --_count;
            }

            if (_root != null)
            {
                _root.IsRed = false;
            }

            return foundMatch;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private static Node GetSibling(Node node, Node parent)
        {
            if (parent.Left == node)
            {
                return parent.Right;
            }
            return parent.Left;
        }

        // After calling InsertionBalance, we need to make sure current and parent up-to-date.
        // It doesn't matter if we keep grandParent and greatGrantParent up-to-date 
        // because we won't need to split again in the next node.
        // By the time we need to split again, everything will be correctly set.
        //  
        private void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
        {
            Debug.Assert(grandParent != null, "Grand parent cannot be null here!");
            bool parentIsOnRight = (grandParent.Right == parent);
            bool currentIsOnRight = (parent.Right == current);

            Node newChildOfGreatGrandParent;
            if (parentIsOnRight == currentIsOnRight)
            { // same orientation, single rotation
                newChildOfGreatGrandParent = currentIsOnRight ? RotateLeft(grandParent) : RotateRight(grandParent);
            }
            else
            {  // different orientaton, double rotation
                newChildOfGreatGrandParent = currentIsOnRight ? RotateLeftRight(grandParent) : RotateRightLeft(grandParent);
                // current node now becomes the child of greatgrandparent 
                parent = greatGrandParent;
            }
            // grand parent will become a child of either parent of current.
            grandParent.IsRed = true;
            newChildOfGreatGrandParent.IsRed = false;

            ReplaceChildOfNodeOrRoot(greatGrandParent, grandParent, newChildOfGreatGrandParent);
        }

        private static bool Is2Node(Node node)
        {
            Debug.Assert(node != null, "node cannot be null!");
            return IsBlack(node) && IsNullOrBlack(node.Left) && IsNullOrBlack(node.Right);
        }

        private static bool Is4Node(Node node)
        {
            return IsRed(node.Left) && IsRed(node.Right);
        }

        private static bool IsBlack(Node node)
        {
            return (node != null && !node.IsRed);
        }

        private static bool IsNullOrBlack(Node node)
        {
            return (node == null || !node.IsRed);
        }

        private static bool IsRed(Node node)
        {
            return (node != null && node.IsRed);
        }

        private static void Merge2Nodes(Node parent, Node child1, Node child2)
        {
            Debug.Assert(IsRed(parent), "parent must be be red");
            // combing two 2-nodes into a 4-node
            parent.IsRed = false;
            child1.IsRed = true;
            child2.IsRed = true;
        }

        // Replace the child of a parent node. 
        // If the parent node is null, replace the root.        
        private void ReplaceChildOfNodeOrRoot(Node parent, Node child, Node newChild)
        {
            if (parent != null)
            {
                if (parent.Left == child)
                {
                    parent.Left = newChild;
                }
                else
                {
                    parent.Right = newChild;
                }
            }
            else
            {
                _root = newChild;
            }
        }

        // Replace the matching node with its succesor.
        private void ReplaceNode(Node match, Node parentOfMatch, Node successor, Node parentOfSuccessor)
        {
            if (successor == match)
            {  // this node has no successor, should only happen if right child of matching node is null.
                Debug.Assert(match.Right == null, "Right child must be null!");
                successor = match.Left;
            }
            else
            {
                Debug.Assert(parentOfSuccessor != null, "parent of successor cannot be null!");
                Debug.Assert(successor.Left == null, "Left child of succesor must be null!");
                Debug.Assert((successor.Right == null && successor.IsRed) || (successor.Right.IsRed && !successor.IsRed), "Succesor must be in valid state");
                if (successor.Right != null)
                {
                    successor.Right.IsRed = false;
                }

                if (parentOfSuccessor != match)
                {   // detach succesor from its parent and set its right child
                    parentOfSuccessor.Left = successor.Right;
                    successor.Right = match.Right;
                }

                successor.Left = match.Left;
            }

            if (successor != null)
            {
                successor.IsRed = match.IsRed;
            }

            ReplaceChildOfNodeOrRoot(parentOfMatch, match, successor);
        }

        internal void UpdateVersion()
        {
            ++_version;
        }

        private static Node RotateLeft(Node node)
        {
            var x = node.Right;
            node.Right = x.Left;
            x.Left = node;

            return x;
        }

        private static Node RotateLeftRight(Node node)
        {
            var child = node.Left;
            var grandChild = child.Right;

            node.Left = grandChild.Right;
            grandChild.Right = node;
            child.Right = grandChild.Left;
            grandChild.Left = child;

            return grandChild;
        }

        private static Node RotateRight(Node node)
        {
            var x = node.Left;
            node.Left = x.Right;
            x.Right = node;

            return x;
        }

        private static Node RotateRightLeft(Node node)
        {
            var child = node.Right;
            var grandChild = child.Left;

            node.Right = grandChild.Left;
            grandChild.Left = node;
            child.Left = grandChild.Right;
            grandChild.Right = child;

            return grandChild;
        }

        private static TreeRotation RotationNeeded(Node parent, Node current, Node sibling)
        {
            Debug.Assert(IsRed(sibling.Left) || IsRed(sibling.Right), "sibling must have at least one red child");
            if (IsRed(sibling.Left))
            {
                if (parent.Left == current)
                {
                    return TreeRotation.RightLeftRotation;
                }

                return TreeRotation.RightRotation;
            }
            else
            {
                if (parent.Left == current)
                {
                    return TreeRotation.LeftRotation;
                }

                return TreeRotation.LeftRightRotation;
            }
        }

        private static void Split4Node(Node node)
        {
            node.IsRed = true;
            node.Left.IsRed = false;
            node.Right.IsRed = false;
        }

        internal class Node
        {
            public bool IsRed;
            public T Item;
            public Node Left;
            public Node Right;

            public Node(T item)
            {
                // The default color will be red, we never need to create a black node directly.                
                Item = item;
                IsRed = true;
            }

            public Node(T item, bool isRed)
            {
                // The default color will be red, we never need to create a black node directly.                
                Item = item;
                IsRed = isRed;
            }
        }

        public struct Enumerator : IEnumerator<T>
        {
            private readonly SortedSet<T> _tree;
            private int _version;

            private readonly Stack<Node> _stack;
            private Node _current;

            internal Enumerator(SortedSet<T> set)
            {
                _tree = set;
                _version = _tree._version;

                // 2lg(n + 1) is the maximum height
                _stack = new Stack<Node>(2 * (int)SortedSet<T>.log2(set.Count + 1));
                _current = null;
                Intialize();
            }

            private void Intialize()
            {
                _current = null;

                Node node = _tree._root;
                Node next = null, other = null;

                while (node != null)
                {
                    next = node.Left;
                    other = node.Right;

                    _stack.Push(node);
                    node = next;
                }
            }

            public bool MoveNext()
            {
                if (_version != _tree._version)
                {
                    throw new InvalidOperationException("Collection has changed during enumeration.");
                }

                if (_stack.Count == 0)
                {
                    _current = null;
                    return false;
                }

                _current = _stack.Pop();

                Node node = _current.Right;
                Node next = null, other = null;

                while (node != null)
                {
                    next = node.Left;
                    other = node.Right;

                    _stack.Push(node);
                    node = next;
                }

                return true;
            }

            public void Dispose()
            {
            }

            public T Current
            {
                get
                {
                    if (_current != null)
                    {
                        return _current.Item;
                    }

                    return default(T);
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException("Collection has changed during enumeration.");
                    }

                    return _current.Item;
                }
            }

            internal bool NotStartedOrEnded
            {
                get
                {
                    return _current == null;
                }
            }

            internal void Reset()
            {
                if (_version != _tree._version)
                {
                    throw new InvalidOperationException("Collection has changed during enumeration.");
                }

                _stack.Clear();
                Intialize();
            }

            void IEnumerator.Reset()
            {
                Reset();
            }
        }

        private static int log2(int value)
        {
            int c = 0;

            while (value > 0)
            {
                c++;
                value >>= 1;
            }

            return c;
        }
    }

    internal enum TreeRotation
    {
        LeftRotation = 1,
        RightRotation = 2,
        RightLeftRotation = 3,
        LeftRightRotation = 4,
    }
}
#endif