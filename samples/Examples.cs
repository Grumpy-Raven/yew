using System;
using UnityEngine;

namespace YewLib
{
    public class Examples : View
    {
        public enum Choice
        {
            None,
            Counter,
            HelloWorld,
            Todo
        }
        
        public class Component : YewLib.Component
        {
            public override View Render()
            {
                var state = UseState(Choice.None);
                return new StackLayout(className: "root", style: "UI/styles.uss")
                {
                    Label("Yew Samples", "h1"),
                    new Flex(className: "sample-menu")
                    {
                        new Checkbox(state.Value == Choice.Counter,
                            v => state.Value = Choice.Counter,
                            "Counter"),
                        new Checkbox(state.Value == Choice.Todo,
                            v => state.Value = Choice.Todo,
                            "Todo"),
                        new Checkbox(state.Value == Choice.HelloWorld,
                            v => state.Value = Choice.HelloWorld,
                            "Sandbox App"),
                    },
                    state.Value switch
                    {
                        Choice.Counter => new CounterApp(),
                        Choice.Todo => new TodoApp(),
                        Choice.HelloWorld => new HelloWorld(),
                        _ => new Label("Choose a sample to learn more about yew")
                    }
                };
            }
        }
    }
}