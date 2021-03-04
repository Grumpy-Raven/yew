using UnityEngine;

namespace YewLib
{
    public class TypeWriter : View
    {
        public string Text { get; set; }

        class Component : YewLib.Component
        {
            private TypeWriter Props { get; set; }
            public Component(TypeWriter props) => Props = props;

            public override View Render()
            {
                var len = UseState(0);
                UseRaf(() =>
                {
                    if (len >= Props.Text.Length) return false;
                    if (Random.value > 0.3) return true;
                    len.Value++;
                    return false; // no need to call another raf because render will do it for us.
                });
                string text = Props.Text;
                if (len < text.Length)
                    text = $"{text.Substring(0, len)}<alpha=#00>{text.Substring(len)}";
                return Label(text, className: "typewriter");
            }
        }
    }
}