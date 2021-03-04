using System;
using System.Collections;
using FluffyUnderware.DevTools.Extensions;
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
                var buttonOpacity = UseState(0f);
                var color = UseState(Color.green);
                Action changeColor = () =>
                {
                    color.Value = Color.Lerp(Color.red, Color.green, Mathf.PingPong(Time.time, 1));
                };
                IEnumerator anim()
                {
                    while (len < Props.Text.Length) {
                        len.Value++;
                        changeColor();
                        yield return new WaitForSeconds(0.1f);
                    }
                    while (buttonOpacity < 1) {
                        buttonOpacity.Value += 0.03f;
                        changeColor();
                        yield return new WaitForSeconds(0.025f);
                    }

                    while (true)
                    {
                        changeColor();
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                UseCoroutine(anim);
                string text = Props.Text;
                string colorPrefix = $"<color={color.Value.ToHtml()}>";
                if (len < text.Length)
                    text = $"{text.Substring(0, len)}<alpha=#00>{text.Substring(len)}";
                return new StackLayout()
                {
                    Label(colorPrefix + text, className: "typewriter"),
                    new Button("Examine body", () => { }, opacity: buttonOpacity)
                };
            }
        }
    }
}