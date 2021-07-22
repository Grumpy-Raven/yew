using System.Collections.Generic;
using UnityEngine.UIElements;

namespace YewLib
{
    public class Yew
    {
        private static Dictionary<VisualElement, Node> nodes = new();
        
        public static void Render(View e, VisualElement ve)
        {
            ve.Clear();
            var rootNode = new Node
            {
                VisualElement = ve,
                Children = new List<Node>()
            };
            var node = Node.PrepareNode(rootNode, e);
            rootNode.Children.Add(node);
            nodes[ve] = rootNode;
        }
    }
}