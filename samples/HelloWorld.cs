
using System.Linq;

namespace YewLib
{
    public class HelloWorld : View
    {
        public int Num { get; set; } = 1;

        public class Component : YewLib.Component
        {
            private HelloWorld props;
            public Component(HelloWorld props)
            {
                this.props = props;
            }

            public override void ReceiveProps(View view)
            {
                props = view as HelloWorld;
            }

            int MakeRandom() => UnityEngine.Random.Range(1, 100);
            
            View Random()
            {
                var state = UseState(0);
                var todoItems = UseAtomValue<TodoState>(TodoApp.TodoAppKey)?.TodoItems
                    .Where(i => i is {Completed: true});
                return new StackLayout()
                {
                    Label("Random Num: " + state.Value),
                    Button("click me random", () => state.Value = MakeRandom()),
                    Label("Completed Todos:"),
                    todoItems?.Select(i => Label(i.Text))
                };
            }

            public override View Render()
            {
                View f = null;
                if (MakeRandom() % 2 == 1)
                {
                    f = new Label("label even");
                }
                else
                {
                    f = new Label("label odd");
                }
                var y = new StackLayout("stack")
                {
                    f,
                    new Label("Num: " + props.Num),
                    Button("click me one", () =>
                    {
                        props.Num++;
                        Update();
                    }),
                    Random(),
                };
                return y;
            }
        }
    }
}