using UnityEngine.UIElements;

namespace YewLib
{
    public class Flex : ContainerView
    {
        public FlexDirection FlexDirection { get; set; }
        public Align AlignItems { get; set; }
        
        public Flex(
            string key = null, 
            string className = null,
            string style = null,
            FlexDirection flexDirection = FlexDirection.Row,
            Align alignItems = Align.Stretch
        )
        {
            FlexDirection = flexDirection;
            Key = key;
            StyleSrc = style;
            ClassName = className;
            AlignItems = alignItems;
        }
        
        public override VisualElement ToVisualElement()
        {
            var ve = new VisualElement();
            ve.style.flexDirection = FlexDirection;
            ve.style.alignItems = AlignItems;
            if (ClassName != null)
                ve.AddToClassList(ClassName);
            return ve;
        }
    }
}