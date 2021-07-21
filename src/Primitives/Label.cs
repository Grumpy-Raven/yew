using System;
using UnityEngine;
using UnityEngine.UIElements;
using YewLib.Util;

namespace YewLib
{
    
    public class Label : Primitive
    {
        public string Text { get; set; }
        public Color? Color { get; set; }
        public Action OnClick { get; set; }
        public Label(string text, string className = null, Color? color = null, Action onClick = null)
        {
            Text = text;
            AddClassName(className);
            Color = color;
            OnClick = onClick;
        }

        public override bool NeedsUpdate(View newView)
        {
            var newLabel = newView as Label;
            return Text != newLabel.Text || Color != newLabel.Color;
        }

        public Label(string text)
        {
            Text = text;
        }

        public override VisualElement ToVisualElement()
        {
            var label = new UnityEngine.UIElements.Label();
            label.text = Text;
            SetClassNamesOnVisualElement(label);
            if (OnClick != null)
                EventHelper<MouseUpEvent>.Bind(label, e => OnClick());
            if (Color.HasValue)
            {
                label.style.color = new StyleColor(Color.Value);
            }
            return label;
        }

        public override void UpdateVisualElement(VisualElement ve)
        {
            var label = ve as UnityEngine.UIElements.Label;
            label.text = Text;
            if (Color.HasValue)
            {
                label.style.color = new StyleColor(Color.Value);
            }
            EventHelper<MouseUpEvent>.Unbind(label);
            if (OnClick != null)
                EventHelper<MouseUpEvent>.Bind(label, e => OnClick());
        }
    }
}