using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PlayableNodes.Text
{
    public enum TextDurationType
    {
        PerCharacter,Total
    }
    
    public static class DOTweenTextExtensions
    {
        public static Tweener DOMaxVisibleCharacters(this TMP_Text target, int endValue, float duration,
            TextDurationType durationType = TextDurationType.PerCharacter)
        {
            return DOTween.To(
                    () => target.maxVisibleCharacters,
                    x => target.maxVisibleCharacters = x,
                    endValue,
                    durationType == TextDurationType.PerCharacter? duration * endValue : duration)
                .SetTarget(target)
                .OnComplete(() => target.maxVisibleCharacters = 99999);
        }
    }
}