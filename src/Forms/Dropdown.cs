using System;
using System.Collections.Generic;
using System.Linq;

namespace YewLib.Forms
{
    public record Dropdown : View
    {
        public string Label { get; set; }
        public bool Multiselect { get; set; }
        public string[] AvailableValues { get; set; }
        public string[] SelectedValues { get; set; }

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
                    new Label(label)
                    {
                        OnClick = () => open.Value = !open.Value
                    },
                    new StackLayout(className: view.DropdownItemsClassName)
                    {
                        open.Value ? view.AvailableValues.Select(v =>
                        {
                            var l = new Label(v, view.DropdownEntryClassName);
                            if (view.SelectedValues.Contains(v))
                            {
                                l.AddClassName(view.DropdownSelectedClassName);
                            }
                            l.OnClick = () =>
                            {
                                if (!view.Multiselect)
                                {
                                    view.OnChanged(new List<string>() {v});
                                    return;
                                }
                                if (view.SelectedValues.Contains(v))
                                {
                                    view.OnChanged(view.SelectedValues.Except(new [] {v}).ToList());
                                }
                                else
                                {
                                    view.OnChanged(view.SelectedValues.Concat(new [] {v}).ToList());
                                }
                            };
                            return l;
                        }) : null
                    }
                };
            }
        }
    }
}