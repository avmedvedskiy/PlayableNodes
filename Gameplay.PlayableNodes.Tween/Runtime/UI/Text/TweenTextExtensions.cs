using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PlayableNodes.Text
{
    public static class DOTweenTextExtensions
    {
        public static Tweener DOMaxVisibleCharacters(this TMP_Text target, int endValue, float duration)
        {
            return DOTween.To(
                    () => target.maxVisibleCharacters,
                    x => target.maxVisibleCharacters = x,
                    endValue,
                    duration * endValue)
                .SetTarget(target)
                .OnComplete(() => target.maxVisibleCharacters = 99999);
        }
    }
}