using UnityEngine;
using UnityEngine.UIElements;
using UIImage = UnityEngine.UIElements.Image;

namespace YewLib
{
    public class Image : Primitive
    {
        public string Src { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public Image(string src = null, int? width = null, int? height = null)
        {
            Src = src;
            Width = width;
            Height = height;
        }
        
        public override bool NeedsUpdate(View newView)
        {
            var newImage = newView as Image;
            return Src != newImage.Src || Height != newImage.Height || Width != newImage.Width;
        }

        public override VisualElement ToVisualElement()
        {
            var image = new UIImage();
            image.image = Resources.Load<Texture>(Src);
            if (Width.HasValue)
                image.style.width = Width.Value;
            if (Height.HasValue)
                image.style.height = Height.Value;
            return image;
        }

        public override void UpdateVisualElement(VisualElement ve)
        {
            var image = ve as UIImage;
            image.image = Resources.Load<Texture>(Src);
            if (Width.HasValue)
                image.style.width = Width.Value;
            if (Height.HasValue)
                image.style.height = Height.Value;
        }
    }
}