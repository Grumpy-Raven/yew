using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace YewLib
{
    public record View
    {
        public View(string className)
        {
            AddClassName(className);
        }

        public View()
        {
        }
        
        
        public string Key { get; set; }
        public HashSet<string> ClassNames { get; set; } = new(); 

        public void AddClassName(string className)
        {
            if (string.IsNullOrEmpty(className))
                return;
            ClassNames.Add(className);
        }

        public void RemoveClassName(string className)
        {
            ClassNames.Remove(className);
        }

        public void ToggleClassName(string className)
        {
            if (ClassNames.Contains(className))
            {
                ClassNames.Remove(className);
            }
            else
            {
                ClassNames.Add(className);
            }
        }

        public void SetClassNamesOnVisualElement(VisualElement ve)
        {
            foreach(var className in ClassNames)
                ve.AddToClassList(className);
        }

        public virtual Component ToComponent(Node node)
        {
            // TODO: if we're going to reflect, we'll want to cache.
            // DOUBLE TODO: this code is like.... really hacky.
            var type = GetType();
            var componentType = type.GetNestedType("Component", 
                BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Public);
            Component instance;
            try
            {
                instance = Activator.CreateInstance(componentType, new object[] {this}) as Component;
            }
            catch
            {
                instance = Activator.CreateInstance(componentType) as Component;
            }

            if (instance != null)
            {
                instance.Node = node;
                instance.Props = this;
                return instance;
            }
            Debug.LogError("error creating component from view");
            return null;
        }
        
        public static implicit operator View(string from)
        {
            return new Label(from);
        }

        public virtual bool NeedsUpdate(View newView) => false;

        public static Func<View, View, bool> ViewEquality => (a, b) =>
        {
            if (ReferenceEquals(null, b) || ReferenceEquals(null, a))
                return false;
            if (ReferenceEquals(a, b))
                return true;
            if (a.GetType() != b.GetType())
                return false;
            return a.Key == b.Key;
        };
    }
}