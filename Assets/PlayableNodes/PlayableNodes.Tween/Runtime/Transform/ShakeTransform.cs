using System;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class ShakeTransform : TweenAnimation<Transform>
    {
        [SerializeField] private float _strength = 1f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private bool _fadeOut = true;


        protected override Tweener GenerateTween() => 
            Target.DOShakeScale(Duration, _strength, _vibrato, _randomness, _fadeOut);
    }
}