using UnityEngine.UIElements;

namespace YewLib
{
    public record Flex : ContainerView
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
            AddClassName(className);
            AlignItems = alignItems;
        }
        
        public override VisualElement ToVisualElement()
        {
            var ve = new VisualElement();
            ve.style.flexDirection = FlexDirection;
            ve.style.alignItems = AlignItems;
            SetClassNamesOnVisualElement(ve);
            return ve;
        }
    }
}