using System;
using UnityEngine.UIElements;
using YewLib.Util;

namespace YewLib
{
    public class TextField : Primitive
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public Action<string> OnChanged { get; set; }
        
        public Align LabelAlign { get; set; }
        public TextField(string label, string value, Action<string> onChanged, string className = null,
            Align labelAlign = Align.Center)
        {
            Label = label;
            Value = value;
            OnChanged = onChanged;
            AddClassName(className);
            LabelAlign = labelAlign;
        }

        public override bool NeedsUpdate(View newView)
        {
            var newTextField = newView as TextField;
            return newTextField.Value != Value || newTextField.Label != Label;
        }

        public override VisualElement ToVisualElement()
        {
            var textField = new UnityEngine.UIElements.TextField(Label);
            SetClassNamesOnVisualElement(textField);
            textField.value = Value;
            textField.labelElement.style.alignSelf = LabelAlign;
            EventHelper<ChangeEvent<string>>.Bind(textField, Updated);
            return textField;
        }

        public override void UpdateVisualElement(VisualElement ve)
        {
            var textfield = ve as UnityEngine.UIElements.TextField;
            EventHelper<ChangeEvent<string>>.Unbind(ve);
            textfield.SetValueWithoutNotify(Value);
            EventHelper<ChangeEvent<string>>.Bind(ve, Updated);
        }

        private void Updated(ChangeEvent<string> e)
        {
            Value = e.newValue;
            OnChanged(e.newValue);
        }
    }
}