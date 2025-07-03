using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableNodes
{
    [Serializable]
    [Description("Fades a Graphic component between alpha values")]
    public class FadeGraphic : TweenAnimation<Graphic>
    {
        [SerializeField] private ToFromValue<float> _from;
        [SerializeField] private ToFromValue<float> _to;

        protected override Tween GenerateTween() => Target
            .DOFade(_to, Duration)
            .ChangeValuesAlpha(_to, _from);
    }
}