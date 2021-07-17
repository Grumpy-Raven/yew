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

        // TODO: move atom code out w/ partial
        private static Dictionary<string, IState> atoms = new Dictionary<string, IState>();

        public static State<T> UseAtom<T>(string key, T initialValue)
        {
            return UseAtom<T>(key, () => initialValue);
        }
        
        public static State<T> UseAtom<T>(string key, Func<T> initialValue)
        {
            if (!atoms.ContainsKey(key))
            {
                var atom = new State<T> {Value = initialValue()};
                atoms[key] = atom;
                if (unassignedActions.ContainsKey(key))
                {
                    foreach (var action in unassignedActions[key])
                    {
                        atoms[key].Subscribers.Add(new Updatable { Action = action });
                    }

                    unassignedActions.Remove(key);
                }

            }
            return atoms[key] as State<T>;
        }
        
        public static State<T> UseAtom<T>(string key)
        {
            return atoms.ContainsKey(key) ? atoms[key] as State<T> : null;
        }

        private static Dictionary<string, List<Action>> unassignedActions = new();
        public static void ObserveAtom(string key, Action action)
        {
            if (!atoms.ContainsKey(key))
            {
                if (unassignedActions.TryGetValue(key, out var actions))
                {
                    actions = new List<Action>();
                    unassignedActions[key] = actions;
                }
                actions.Add(action);
            }
            else
            {
                atoms[key].Subscribers.Add(new Updatable { Action = action });
            }
        }
    }

    public class Updatable : IUpdatable
    {
        public Action Action { get; set; }
        public void Update()
        {
            Action();
        }
    }
}