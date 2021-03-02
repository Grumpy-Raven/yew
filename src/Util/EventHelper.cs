using System.Collections.Generic;
using UnityEngine.UIElements;

namespace YewLib.Util
{
    internal class EventHelper<T> where T : EventBase<T>, new()
    {
        static Dictionary<VisualElement, EventCallback<T>> callbacks =
            new Dictionary<VisualElement, EventCallback<T>>();

        public static void Unbind(VisualElement ve)
        {
            if (callbacks.ContainsKey(ve))
                ve.UnregisterCallback(callbacks[ve]);
        }

        public static void Bind(VisualElement ve, EventCallback<T> callback)
        {
            callbacks[ve] = callback;
            ve.RegisterCallback(callback);
        }
    }
}