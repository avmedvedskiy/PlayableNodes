using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PlayableNodes
{

    [Serializable]
    [Description("Set RectTransform anchored position")]
    public class SetAnchoredPositionTransform : TargetAnimation<RectTransform>
    {
        [SerializeField] private Vector2 _anchoredPosition = Vector2.zero;
        protected override UniTask Play(CancellationToken cancellationToken)
        {
            Target.anchoredPosition = _anchoredPosition;
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    [Description("Tweens the RectTransform's anchored position between two values")]
    public class AnchoredPositionTransform : TweenAnimation<RectTransform>
    {
        [SerializeField] private ToFromValue<Vector2> _from;
        [SerializeField] private ToFromValue<Vector2> _to;

        protected override Tween GenerateTween() => 
            Target
                .DOAnchorPos(_to, Duration)
                .ChangeValuesVector(_to, _from);
    }
}