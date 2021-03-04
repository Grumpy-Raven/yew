using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace YewLib
{
    public abstract class ContainerView : Primitive, IEnumerable<View>
    {
        public List<View> Children { get; } = new List<View>();
        
        public string StyleSrc { get; set; }

        public IEnumerator<View> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(View e)
        {
            if (e == null) return;
            Children.Add(e);
        }
        
        public void Add(ContainerView e)
        {
            if (e == null) return;
            Children.Add(e);
        }
        
        public void Add(IEnumerable<View> e)
        {
            Children.AddRange(e.Where(x => x != null));
        }

        public void MaybeSyncStyle(VisualElement ve)
        {
            if (string.IsNullOrEmpty(StyleSrc)) return;
            ve.styleSheets.Clear();
            var stylesheet = Resources.Load<StyleSheet>(StyleSrc);
            ve.styleSheets.Add(stylesheet);
        }

        public override bool NeedsUpdate(View newView)
        {
            var other = newView as ContainerView;
            if (other.Children.Count != Children.Count) return true;
            for(int i = 0; i < Children.Count; i++)
                if (!Equals(other.Children[i], Children[i]) ||
                    Children[i].NeedsUpdate(other.Children[i]))
                    return true;
            return false;
        }

        public override void UpdateVisualElement(VisualElement ve)
        {
            // no need to throw an exception for these...
        }
    }

    public class StackLayout : ContainerView
    {
        public StackLayout(string key = null, string className = null, string style = null)
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