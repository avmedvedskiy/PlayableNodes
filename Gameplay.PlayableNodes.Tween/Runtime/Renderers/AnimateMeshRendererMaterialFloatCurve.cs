using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    [Description("Tweens a float material property on a MeshRenderer using an AnimationCurve and optionally restores the original material on completion")]
    public class AnimateMeshRendererMaterialFloatCurve : TweenAnimation<MeshRenderer>
    {
        [SerializeField] private Material _material;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private string _fieldName;
        [SerializeField] private bool _resetMaterialOnComplete = true;

        private Material _lastMaterial;

        protected override Tween GenerateTween()
        {
            float t = 0f;
            return DOTween
                .To(() => t, x => Set(_curve.Evaluate(x)), 1f, Duration)
                .OnStart(OnStart)
                .OnComplete(OnComplete);
        }

        private void OnStart()
        {
          if (_material != null)
          {
            _lastMaterial = Target.sharedMaterial;
            Target.material = Object.Instantiate(_material);
          }
          Set(_curve.Evaluate(0f));
        }

        private void OnComplete()
        {
            if (Target && _resetMaterialOnComplete && _material != null)
            {
                Target.material.DestroyOrPreview();
                Target.material = _lastMaterial;
            }
        }

        private void Set(float value)
        {
            Target.material.SetFloat(_fieldName, value);
        }
    }
}
