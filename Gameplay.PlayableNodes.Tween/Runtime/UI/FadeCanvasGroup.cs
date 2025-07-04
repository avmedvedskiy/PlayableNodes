using System;
using System.ComponentModel;
using System.Linq;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    [Description("Fades a CanvasGroup between alpha values")]
    public class FadeCanvasGroup : TweenAnimation<CanvasGroup>
    {
        [SerializeField] private ToFromValue<float> _from;
        [SerializeField] private ToFromValue<float> _to;
        protected override Tween GenerateTween() => Target
            .DOFade(_to, Duration)
            .ChangeValuesFloat(_to, _from);
    }
}