using Cinemachine;
using UnityEngine.UIElements;

namespace YewLib
{
    
    public class Label : Primitive
    {
        public string Text { get; set; }
        public Label(string text, string className = null)
        {
            this.Text = text;
            this.ClassName = className;
        }

        public override bool NeedsUpdate(View newView)
        {
            return Text != (newView as Label).Text;
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
            return label;
        }

        public override void UpdateVisualElement(VisualElement ve)
        {
            var label = ve as UnityEngine.UIElements.Label;
            label.text = Text;
        }
    }
}