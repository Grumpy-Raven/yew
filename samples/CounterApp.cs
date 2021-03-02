using UnityEngine.UIElements;

namespace YewLib
{
    public class CounterApp : View
    {
        public class Component : YewLib.Component
        {
            public override View Render()
            {
                var state = UseState(0);
                return new Flex(alignItems: Align.Center)
                {
                    Label(state.Value.ToString()),
                    Button("Increment", () => state.Value++)
                };
            }
        }
    }
}