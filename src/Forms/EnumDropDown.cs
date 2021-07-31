using System;
using System.Collections.Generic;
using System.Linq;

namespace YewLib.Forms
{
    public record EnumDropdown<T> : View where T : struct
    {
        public string Label { get; set; }
        public bool Multiselect { get; set; }
        public T[] SelectedValues { get; set; }
        public T SelectedValue
        {
            set
            {
                SelectedValues = new[] {value};
            }
        }
        
        public Action<T[]> OnChanged { get; set; }
        
        static IEnumerable<T> ValuesAsEnum(string[] values) 
        {
            foreach (var v in values)
            {
                if (!Enum.TryParse<T>(values.First(), out var e))
                    continue;
                yield return e;
            }
        }
        public class Component : YewLib.Component
        {
            public override View Render()
            {
                var props = Props as EnumDropdown<T>;
                return new Dropdown()
                {
                    Multiselect = props.Multiselect,
                    Label = props.Label,
                    AvailableValues = Enum.GetNames(typeof(T)),
                    SelectedValues = props.SelectedValues.Select(e => e.ToString()).ToArray(),
                    OnChanged = values =>
                    {
                        props.OnChanged(ValuesAsEnum(values).ToArray());
                    }
                };
            }

        }
    }
}