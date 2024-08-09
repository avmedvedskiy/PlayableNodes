using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PlayableNodes.Values
{
    public enum ToFromType
    {
        Direct,Dynamic
    }
    
    [Serializable]
    public struct ToFromValue<T>
    {
        [SerializeField] private T _value;
        [SerializeField] private ToFromType _type;
        public ToFromType Type => _type;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(ToFromValue<T> source)
        {
            return source._value;
        }

        public ToFromValue(T value, ToFromType type = ToFromType.Direct)
        {
            _value = value;
            _type = type;
        }

        public static ToFromValue<T> Dynamic => new(default, ToFromType.Dynamic);
        
    }
}