using System;
using System.Collections.Generic;

namespace YewLib
{
    public class Key<T>
    {
    }

    public static class Atom<T>
    {
        public static State<T> Use(Key<T> key, T initialValue)
        {
            return Use(key, () => initialValue);
        }
        
        public static State<T> Use(Key<T> key, Func<T> initialValue)
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
        
        public static State<T> Use(Key<T> key)
        {
            return atoms.ContainsKey(key) ? atoms[key] : null;
        }
        private static Dictionary<Key<T>, State<T>> atoms = new();

        private static Dictionary<Key<T>, List<Action<T>>> unassignedActions = new();
        public static void Observe(Key<T> key, Action<T> action)
        {
            if (!atoms.ContainsKey(key))
            {
                if (!unassignedActions.TryGetValue(key, out var actions))
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