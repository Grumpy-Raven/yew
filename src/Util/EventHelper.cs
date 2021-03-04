using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace YewLib.Util
{
    internal class EventHelper
    {
        private static Dictionary<UnityEngine.UIElements.Button, Action> actions =
            new Dictionary<UnityEngine.UIElements.Button, Action>();
        
        public static void Unbind(UnityEngine.UIElements.Button button)
        {
            if (actions.ContainsKey(button))
                button.clicked -= actions[button];
        }

        public static void Bind(UnityEngine.UIElements.Button button, Action action)
        {
            actions[button] = action;
            button.clicked += action;
        }
    }
    
    internal class EventHelper<T> where T : EventBase<T>, new()
    {
        static Dictionary<VisualElement, EventCallback<T>> callbacks =
            new Dictionary<VisualElement, EventCallback<T>>();

        public static void Unbind(VisualElement ve)
        {
            if (callbacks.ContainsKey(ve))
            {
                ve.UnregisterCallback(callbacks[ve]);
            }
        }

        public static void Bind(VisualElement ve, EventCallback<T> callback)
        {
            callbacks[ve] = callback;
            ve.RegisterCallback(callback);
        }
    }
}