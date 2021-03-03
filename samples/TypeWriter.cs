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

            void Raf(State<int> len)
            {
                if (len >= Props.Text.Length) return;
                if (Random.value < 0.3)
                    len.Value++;
                else
                    RequestAnimationFrame(() => Raf(len));
            }
            
            public override View Render()
            {
                var len = UseState(0);
                RequestAnimationFrame(() => Raf(len));
                string text = Props.Text;
                if (len < text.Length)
                    text = $"{text.Substring(0, len)}<alpha=#00>{text.Substring(len)}";
                return Label(text, className: "typewriter");
            }
        }
    }
}