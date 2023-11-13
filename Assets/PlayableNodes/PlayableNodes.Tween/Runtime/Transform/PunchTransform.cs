using System;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class PunchTransform : TweenAnimation<Transform>
    {
        [SerializeField] private Vector3 _to = Vector3.one;

        protected override Tweener GenerateTween() => 
            Target.DOPunchScale(_to, Duration);
    }
}