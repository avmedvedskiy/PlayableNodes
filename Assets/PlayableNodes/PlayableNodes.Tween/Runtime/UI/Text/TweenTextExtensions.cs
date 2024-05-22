using System;
using DG.Tweening;
using TMPro;

namespace PlayableNodes.Text
{
    public static class DOTweenTextExtensions
    {
        public static Tweener DOMaxVisibleCharacters(this TMP_Text target, int endValue, float duration)
        {
            return DOTween.To(() => target.maxVisibleCharacters, x => target.maxVisibleCharacters = x, endValue, duration)
                .SetTarget(target)
                .OnComplete(()=> target.maxVisibleCharacters = int.MaxValue);
        }
    }
}