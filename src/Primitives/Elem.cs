using UnityEngine.UIElements;

namespace YewLib
{
    public class Elem : ContainerView
    {
        public Elem(string key = null, string className = null, string style = null)
        {
            Key = key;
            ClassName = className;
            StyleSrc = style;
        }
        
        public override VisualElement ToVisualElement()
        {
            var ve = new VisualElement();
            if (ClassName != null)
                ve.AddToClassList(ClassName);
            return ve;
        }
    }
}