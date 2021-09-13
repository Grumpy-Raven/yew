using System;
using UnityEngine.UIElements;
using YewLib.Util;

namespace YewLib
{
    public record Button : Primitive
    {
        public string Label { get; set; }
        public Action OnClick { get; set; }
        
        public float Opacity { get; set; }

        public static string DefaultClassName = "unity-button";
        
        public Button(string label, Action onClick, string className = null, float opacity = 1)
        {
            Label = label;
            OnClick = onClick;
            Opacity = opacity;
            AddClassName(DefaultClassName);
            if (className != null)
                AddClassName(className);
        }

        public override bool NeedsUpdate(View newView)
        {
            var otherButton = newView as Button;
            return Label != otherButton.Label || Opacity != otherButton.Opacity;
        }

        public override VisualElement ToVisualElement()
        {
            var button = new UnityEngine.UIElements.Button();
            SetClassNamesOnVisualElement(button);
            button.style.opacity = Opacity;
            button.text = Label;
            EventHelper.Bind(button, OnClick);
            return button;
        }

        public override void UpdateVisualElement(VisualElement ve)
        {
            var button = ve as UnityEngine.UIElements.Button;
            button.text = Label;
            button.style.opacity = Opacity;
            EventHelper.Unbind(button);
            EventHelper.Bind(button, OnClick);
        }
    }
}