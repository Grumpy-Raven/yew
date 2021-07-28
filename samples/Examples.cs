using System;
using System.Linq;
using UnityEngine.UIElements;

namespace YewLib
{
    public record Examples : View
    {
        public enum Choice
        {
            None,
            Counter,
            HelloWorld,
            TypeWriter,
            Todo
        }
        
        public class Component : YewLib.Component
        {
            public override View Render()
            {
                var state = UseState(Choice.None);
                return new StackLayout(className: "root", style: "UI/styles")
                {
                    new Flex(alignItems: Align.FlexEnd)
                    {
                        new Image("Textures/yew-logo-small", width: 100, height: 100),
                        Label("Yew Samples", "h1")
                    },
                    new Flex(className: "sample-menu")
                    {
                        Enum.GetValues(typeof(Choice)).Cast<Choice>().Select((Choice c) => new Checkbox(state.Value == c,
                            v => state.Value = c, c.ToString()))
                    },
                    state.Value switch
                    {
                        Choice.Counter => new CounterApp(),
                        Choice.Todo => new TodoApp(),
                        Choice.HelloWorld => new HelloWorld(),
                        Choice.TypeWriter => new TypeWriter() { Text = "You step into a dark room. Inside, you see a body."},
                        _ => new Label("Choose a sample to learn more about yew")
                    }
                };
            }
        }
    }
}