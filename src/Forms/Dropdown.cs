using System;
using System.Collections.Generic;

namespace YewLib.Forms
{
    public class Dropdown : View
    {
        public string Label { get; set; }
        public bool Multiselect { get; set; }
        public List<string> AvailableValues { get; set; }
        public List<string> SelectedValues { get; set; }

        public string DropdownClassName { get; set; } = "dd";
        public string DropdownItemsClassName { get; set; } = "dd-items";
        
        public Action<List<string>> OnChanged { get; set; }
        
        public class Component : YewLib.Component
        {
            private Dropdown view;
            public Component(Dropdown view)
            {
                this.view = view;
            }

            public override void ReceiveProps(View view)
            {
                this.view = view as Dropdown;
            }

            public override View Render()
            {
                
                var open = UseState(false);
                var label = $"{(open ? "⌃" : "⌄")} {string.Join(", ", view.SelectedValues)}";

                return new StackLayout(className: view.DropdownClassName)
                {
                    label,
                };
            }
        }
    }
}