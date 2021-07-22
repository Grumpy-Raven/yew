using System;
using System.Collections.Generic;

namespace YewLib
{
    public static class Atom<T>
    {
        public static State<T> Use(string key, T initialValue)
        {
            return Use(key, () => initialValue);
        }
        
        public static State<T> Use(string key, Func<T> initialValue)
        {
            if (!atoms.ContainsKey(key))
            {
                var atom = new State<T> {Value = initialValue()};
                atoms[key] = atom;
                if (unassignedActions.ContainsKey(key))
                {
                    foreach (var action in unassignedActions[key])
                    {
                        atoms[key].Subscribers.Add(action);
                    }
                    unassignedActions.Remove(key);
                }

            }
            return atoms[key] as State<T>;
        }
        
        public static State<T> Use(string key)
        {
            return atoms.ContainsKey(key) ? atoms[key] : null;
        }
        private static Dictionary<string, State<T>> atoms = new();

        private static Dictionary<string, List<Action<T>>> unassignedActions = new();
        public static void Observe(string key, Action<T> action)
        {
            if (!atoms.ContainsKey(key))
            {
                if (unassignedActions.TryGetValue(key, out var actions))
                {
                    actions = new List<Action<T>>();
                    unassignedActions[key] = actions;
                }
                actions.Add(action);
            }
            else
            {
                atoms[key].Subscribers.Add(action);
            }
        }
    }
}