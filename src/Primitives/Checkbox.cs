using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YewLib.Util;

namespace YewLib
{
    public class Checkbox : Primitive
    {
        public string Label { get; set; }
        public bool Value { get; set; }
        
        public Action<bool> OnChanged { get; set; }

        public Checkbox(bool value, Action<bool> changed, string label = null)
        {
            Value = value;
            OnChanged = changed;
            Label = label;
        }
        
        public override VisualElement ToVisualElement()
        {
            var checkbox = new UnityEngine.UIElements.Toggle();
            checkbox.value = Value;
            checkbox.text = Label;
            EventHelper<ChangeEvent<bool>>.Bind(checkbox, Changed);
            return checkbox;
        }

        private void Changed(ChangeEvent<bool> c)
        {
            OnChanged(c.newValue);
        }

        public override bool NeedsUpdate(View newView)
        {
            var other = newView as Checkbox;
            return other.Label != Label || other.Value != Value;
        }

        public override void UpdateVisualElement(VisualElement ve)
        {
            var toggle = ve as Toggle;
            EventHelper<ChangeEvent<bool>>.Unbind(ve);
            toggle.text = Label;
            toggle.SetValueWithoutNotify(Value);
            EventHelper<ChangeEvent<bool>>.Bind(ve, Changed);
        }

        public override string ToString() => $"Checkbox: value: {Value} label: {Label}";
    }
}