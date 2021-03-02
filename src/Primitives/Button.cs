using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace YewLib
{
    public class Button : Primitive
    {
        public string Label { get; set; }
        public Action OnClick { get; set; }
        
        public Button(string label, Action onClick, string className = null)
        {
            Label = label;
            OnClick = onClick;
            ClassName = className;
        }

        public override VisualElement ToVisualElement()
        {
            var button = new UnityEngine.UIElements.Button(OnClick);
            if (!string.IsNullOrEmpty(ClassName))
                button.AddToClassList(ClassName);
            button.text = Label;
            return button;
        }

        public override void UpdateVisualElement(VisualElement ve)
        {
            var button = ve as UnityEngine.UIElements.Button;
            button.text = Label;
            // TODO: how to unregister prior callback?
            button.clicked += OnClick;
        }
    }
}