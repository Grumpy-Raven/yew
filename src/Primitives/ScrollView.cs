using UnityEngine.UIElements;
using UIScrollView = UnityEngine.UIElements.ScrollView;

namespace YewLib
{
    public class ScrollView : ContainerView
    {
        public ScrollView()
        {
            
        }
        
        public ScrollView(string classNames) : base(classNames)
        {
            
        }
        
        public override VisualElement ToVisualElement()
        {
            var scroller = new UIScrollView();
            SetClassNamesOnVisualElement(scroller);
            return scroller;
        }
    }
}