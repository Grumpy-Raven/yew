using System;
using UnityEngine.UIElements;

namespace YewLib
{
    public class Primitive : View
    {
        public Primitive()
        {
        }

        public Primitive(string classNames) : base(classNames)
        {
        }

        public virtual VisualElement ToVisualElement()
        {
            return null;
        }

        public virtual void UpdateVisualElement(VisualElement ve)
        {
            throw new NotImplementedException($"type: {GetType()}");
        }
    }
}