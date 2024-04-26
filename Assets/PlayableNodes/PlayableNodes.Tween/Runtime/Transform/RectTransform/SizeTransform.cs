using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class SizeTransform : TweenAnimation<RectTransform>
    {
        [SerializeField] private ToFromValue<Vector2> _from;
        [SerializeField] private ToFromValue<Vector2> _to;

        protected override Tweener GenerateTween() => 
            Target
                .DOSizeDelta(_to, Duration)
                .ChangeValuesVector(_to, _from);
        
    }
}