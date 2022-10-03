﻿using UnityEngine;

namespace Game.Common.ScriptableData
{
    public abstract class ScriptableObjectData<T> : ScriptableObject
    {
        public const string SCRIPTABLE_OBJECT_DATA_MENU_NAME = "Singular Data/";

        public delegate void OnChangeEventHandler(T data);
        public event OnChangeEventHandler OnChangeEvent;

        public T value
        {
            get => _value;
            set
            {
                _value = value;
                OnChangeEvent?.Invoke(value);
            }
        }

        private T _value;
    }
}