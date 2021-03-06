﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace YewLib
{
    public class View : IEquatable<View>
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

        public virtual bool Equals(View other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((View) obj);
        }

        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }

        public static bool operator ==(View left, View right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(View left, View right)
        {
            return !Equals(left, right);
        }
    }
}