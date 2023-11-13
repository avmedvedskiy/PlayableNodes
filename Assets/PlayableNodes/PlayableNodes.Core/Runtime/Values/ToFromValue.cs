using System;
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

        public static implicit operator T(ToFromValue<T> source)
        {
            return source._value;
        }
    }
}