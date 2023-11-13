using UnityEngine;
using System.Collections;
using DG.Tweening.Timeline;

public static class DOExtensionMethods
{
    public static void PinsSetToVector(this DOTweenClip visualSequence, int pinIndex, Vector4 targetVector)
    {
        var list = visualSequence.FindClipElementsByPinNoAlloc(pinIndex);
        foreach(var e in list)
        {
            e.toVector2Val = targetVector;
            e.toVector3Val = targetVector;
            e.toVector4Val = targetVector;
        }
    }
    
    public static void PinsSetFromVector(this DOTweenClip visualSequence, int pinIndex, Vector4 targetVector)
    {
        var list = visualSequence.FindClipElementsByPinNoAlloc(pinIndex);
        foreach (var e in list)
        {
            e.fromVector2Val = targetVector;
            e.fromVector3Val = targetVector;
            e.fromVector4Val = targetVector;
        }
    }

    public static void PinSetToFloatVal(this DOTweenClip visualSequence, int pinIndex, float targetValue)
    {
        var list = visualSequence.FindClipElementsByPinNoAlloc(pinIndex);
        foreach (var e in list)
        {
            e.toFloatVal = targetValue;
        }
    }

    public static void PinSetTarget(this DOTweenClip visualSequence, int pinIndex, Object targetValue)
    {
        var list = visualSequence.FindClipElementsByPinNoAlloc(pinIndex);
        foreach (var e in list)
        {
            e.target = targetValue;
        }
    }
}