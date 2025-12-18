using System;
using System.Collections.Generic;

namespace Eve.Model
{

    public class TreeNode<T>
    {
        public T Value { get; set; }
        public TreeNode<T>? Parent { get; private set; }
        public List<TreeNode<T>> Children { get; }

        public TreeNode(T value)
        {
            Value = value;
            Children = new List<TreeNode<T>>();
        }

        public TreeNode<T> AddChild(T value)
        {
            var child = new TreeNode<T>(value)
            {
                Parent = this
            };

            Children.Add(child);
            return child;
        }

        public bool RemoveChild(TreeNode<T> child)
        {
            if (Children.Remove(child))
            {
                child.Parent = null;
                return true;
            }

            return false;
        }

        public bool IsRoot => Parent == null;
        public bool IsLeaf => Children.Count == 0;
    }

}
