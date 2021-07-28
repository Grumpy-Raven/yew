using UnityEngine.UIElements;

namespace YewLib
{
    public record StackLayout : ContainerView
    {
        public StackLayout(string key = null, string className = null, string style = null)
        {
            Key = key;
            AddClassName(className);
            StyleSrc = style;
        }

        public override VisualElement ToVisualElement()
        {
            var ve = new VisualElement();
            SetClassNamesOnVisualElement(ve);
            return ve;
        }
    }
}