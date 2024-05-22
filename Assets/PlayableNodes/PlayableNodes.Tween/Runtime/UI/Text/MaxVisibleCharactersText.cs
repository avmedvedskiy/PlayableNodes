using System;
using DG.Tweening;
using TMPro;

namespace PlayableNodes.Text
{
    [Serializable]
    public class MaxVisibleCharactersText: TweenAnimation<TMP_Text>
    {
        protected override Tweener GenerateTween()
        {
            Target.maxVisibleCharacters = 0;
            return Target.DOMaxVisibleCharacters(Target.text.Length, Duration);
        }
    }
}