using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableNodes
{
    [Serializable]
    public class FadeGraphic : TweenAnimation<Graphic>
    {
        [SerializeField] private ToFromValue<float> _from;
        [SerializeField] private ToFromValue<float> _to;

        protected override Tweener GenerateTween() => Target
            .DOFade(_to, Duration)
            .ChangeValuesAlpha(_to, _from);
    }
}