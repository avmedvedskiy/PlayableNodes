using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    
    [Serializable]
    public class FadeCanvasGroup : TweenAnimation<CanvasGroup>
    {
        [SerializeField] private ToFromValue<float> _from;
        [SerializeField] private ToFromValue<float> _to;
        protected override Tweener GenerateTween() => Target
            .DOFade(_to, Duration)
            .ChangeValuesFloat(_to, _from);
    }
}