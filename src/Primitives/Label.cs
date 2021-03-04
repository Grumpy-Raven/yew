using UnityEngine;
using UnityEngine.UIElements;

namespace YewLib
{
    
    public class Label : Primitive
    {
        public string Text { get; set; }
        public Color? Color { get; set; }
        public Label(string text, string className = null, Color? color = null)
        {
            Text = text;
            ClassName = className;
            Color = color;
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
            if (!string.IsNullOrEmpty(ClassName))
                label.AddToClassList(ClassName);
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
        }
    }
}