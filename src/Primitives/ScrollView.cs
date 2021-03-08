using UnityEngine.UIElements;
using UIScrollView = UnityEngine.UIElements.ScrollView;

namespace YewLib
{
    public class ScrollView : ContainerView
    {
        public ScrollView()
        {
            
        }
        
        public ScrollView(string className) : base(className)
        {
            
        }
        
        public override VisualElement ToVisualElement()
        {
            var scroller = new UIScrollView();
            if (ClassName != null)
                scroller.AddToClassList(ClassName);
            return scroller;
        }
    }
}