using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace YewLib
{
    public class Yew
    {
        private static Dictionary<VisualElement, Node> nodes = new Dictionary<VisualElement, Node>();
        
        public static void Render(View e, VisualElement ve)
        {
            ve.Clear();
            var rootNode = new Node
            {
                VisualElement = ve,
                Children = new List<Node>()
            };
            var node = Node.PrepareNode(rootNode, e);
            rootNode.Children.Add(node);
            nodes[ve] = rootNode;
        }

        private static Dictionary<string, IState> atoms = new Dictionary<string, IState>();

        public static State<T> UseAtom<T>(string key, T initialValue)
        {
            return UseAtom<T>(key, () => initialValue);
        }
        
        public static State<T> UseAtom<T>(string key, Func<T> initialValue)
        {
            if (!atoms.ContainsKey(key))
            {
                atoms[key] = new State<T>() {Value = initialValue()};
            }
            return atoms[key] as State<T>;
        }
        
        public static State<T> UseAtom<T>(string key)
        {
            return atoms.ContainsKey(key) ? atoms[key] as State<T> : null;
        }
    }
}