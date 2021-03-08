using System;
using System.Collections.Generic;
using System.Linq;

namespace YewLib
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }
    }
    
    public class TodoState
    {
        public List<TodoItem> TodoItems = new List<TodoItem>();
        public string CurrentItemText { get; set; }
        public int IdGen { get; set; }
    }

    public class TodoApp : View
    {
        public static string TodoAppKey = "toyew";
        
        public class Component : YewLib.Component
        {
            public View TodoItemView(Action update, TodoItem item) =>
                new Flex("item-" + item.Id, "todo-item")
                {
                    new Checkbox(item.Completed, v =>
                    {
                        item.Completed = v;
                        update();
                    }),
                    new Label(item.Text)
                };
            
            public override View Render()
            {
                var state = UseAtom(TodoAppKey, () => new TodoState());
                return new StackLayout()
                {
                    new ScrollView
                    {
                        state.Value.TodoItems
                            .Where(x => !x.Completed)
                            .Select(item => TodoItemView(state.Update, item)),
                    },
                    new Flex(className: "new-todo") {
                        new TextField(
                            label: "What do you need to do?",
                            value: state.Value.CurrentItemText,
                            className: "textfield",
                            onChanged:(v) => state.Value.CurrentItemText = v
                        ),
                        new Button("Add!", () =>
                        {
                            state.Value.TodoItems.Add(new TodoItem
                            {
                                Id = state.Value.IdGen++,
                                Text = state.Value.CurrentItemText
                            });
                            state.Value.CurrentItemText = string.Empty;
                            state.Update();
                        })
                    },
                    state.Value.TodoItems.Any(x => x.Completed) ? new ScrollView(className: "completed")
                    {
                        Label("Completed Items:"),
                        state.Value.TodoItems
                            .Where(x => x.Completed)
                            .Select(item => TodoItemView(state.Update, item)),
                    } : null
                };
            }
        }
    }
}