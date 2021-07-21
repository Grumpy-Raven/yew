using System;
using System.Collections.Generic;
using System.Linq;

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
        public string DropdownEntryClassName { get; set; } = "dd-entry";
        public string DropdownSelectedClassName { get; set; } = "dd-selected";
        
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
                    new StackLayout(className: view.DropdownItemsClassName)
                    {
                        view.AvailableValues.Select(v =>
                        {
                            var l = new Label(v, view.DropdownEntryClassName);
                            if (view.SelectedValues.Contains(v))
                                l.ClassName += " " + view.DropdownSelectedClassName;
                            l.OnClick = () =>
                            {
                                if (view.SelectedValues.Contains(v))
                                {
                                    view.SelectedValues.Remove(v);
                                }
                                else
                                {
                                    view.SelectedValues.Add(v);
                                }
                            };
                            return l;
                        })
                    }
                };
            }
        }
    }
}