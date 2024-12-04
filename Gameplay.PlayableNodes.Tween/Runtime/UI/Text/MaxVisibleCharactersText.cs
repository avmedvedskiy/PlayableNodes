using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PlayableNodes.Text
{
    [Serializable]
    public class MaxVisibleCharactersText: TweenAnimation<TMP_Text>
    {

        [SerializeField] private TextDurationType _durationType = TextDurationType.PerCharacter;
        
        protected override Tween GenerateTween()
        {
            Target.maxVisibleCharacters = 0;
            return Target.DOMaxVisibleCharacters(Target.text.Length, Duration, _durationType);
        }
    }
}