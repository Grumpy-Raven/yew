using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YewLib.Util;

namespace YewLib
{
    public class Node
    {
        private VisualElement _visualElement;
        public Node Parent { get; set; }
        public View View { get; set; }
        public Component Component { get; set; }

        public int Depth { get; private set; } = 0;

        public VisualElement VisualElement
        {
            get => _visualElement;
            set
            {
                if (_visualElement != null)
                    UpdateVisualElement(_visualElement, value);
                _visualElement = value;
            }
        }

        public List<Node> Children { get; set; }
        
        void UpdateVisualElement(VisualElement old, VisualElement _new)
        {
            var pe = old.parent;
            pe.Add(_new);
            _new.PlaceBehind(old);
            pe.Remove(old);
        }

        public void Update(View view = null)
        {
            Depth = (Parent?.Depth ?? 0) + 1;
            view ??= View;
            if (view == null) return;
            if (view is Primitive primitive)
            {
                if (VisualElement != null)
                {
                    primitive.UpdateVisualElement(VisualElement);
                }
                else
                {
                    VisualElement = primitive.ToVisualElement();
                    if (primitive is ContainerView containerView)
                    {
                        containerView.MaybeSyncStyle(VisualElement);
                    }
                }
                View = view;
            } else
            {
                View = view;
                if (Component == null)
                {
                   VisualElement = new VisualElement();
                   Component = view.ToComponent(this);
                }
                else
                    Component.ReceiveProps(view);
                Component.PrepareRender();
                view = new StackLayout
                {
                    Component.Render()
                };
            }
            MaybeReconcileChildren(view);
        }

        private void MaybeReconcileChildren(View view)
        {
            if (view is ContainerView containerView && containerView.Children.Any())
            {
                if (Children == null)
                    Children = new List<Node>();
                var needToReconcile = false;
                var updatesNeeded = false;
                if (containerView.Children.Count != Children.Count)
                    needToReconcile = true;
                else
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        if (!Equals(Children[i].View, containerView.Children[i]))
                        {
                            needToReconcile = true;
                            break;
                        }

                        if (Children[i].View.NeedsUpdate(containerView.Children[i]))
                        {
                            updatesNeeded = true;
                        }
                    }
                }

                if (!needToReconcile)
                {
                    if (updatesNeeded)
                    {
                        for (int i = 0; i < Children.Count; i++)
                        {
                            if (Children[i].View.NeedsUpdate(containerView.Children[i]))
                            {
                                Children[i].Update(containerView.Children[i]);
                            }
                        }
                    }
                    return;
                }

                var lcs = new LCS();
                var srcPos = 0;
                var dstPos = 0;
                foreach (var action in
                    lcs.Path(Children.Select(c => c.View).ToArray(), containerView.Children))
                {
                    Node oldNode;
                    View newView;
                    switch (action)
                    {
                        case LCS.Op.Keep:
                            oldNode = Children[srcPos++];
                            newView = containerView.Children[dstPos++];
                            if (oldNode.View.NeedsUpdate(newView))
                            {
                                oldNode.Update(newView);
                                // Debug.Log(Indent + "updating " + newView.Key);
                            }
                            break;
                        case LCS.Op.Delete:
                            // Debug.Log("removing old elem at " + srcPos);
                            oldNode = Children[srcPos];
                            Children.RemoveAt(srcPos);
                            oldNode.VisualElement.parent.Remove(oldNode.VisualElement);
                            break;
                        case LCS.Op.Insert:
                            newView = containerView.Children[dstPos++];
                            // Debug.Log(Indent + "inserting " + newView.Key);
                            var newNode = PrepareNode(this, newView, srcPos);
                            Children.Insert(srcPos++, newNode);
                            break;
                    }
                }
            }
        }

        public static Node PrepareNode(Node parentNode, View view, int i = -1)
        {
            var node = new Node
            {
                Parent = parentNode,
                View = view
            };
            node.Update(view);
            if (i == -1)
                parentNode.VisualElement.Add(node.VisualElement);
            else
                parentNode.VisualElement.Insert(i, node.VisualElement);
            return node;
        }
        
        private string Indent => new string(' ', Depth * 2);
    }
}