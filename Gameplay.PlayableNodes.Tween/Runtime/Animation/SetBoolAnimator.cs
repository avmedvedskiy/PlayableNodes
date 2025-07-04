using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [Serializable]
    [Description("Sets a bool parameter in an Animator")]
    public class SetBoolAnimator : TargetAnimation<Animator>
    {
        [SerializeField] private string _parameterName;
        [SerializeField] private bool _value;

        protected override async UniTask Play(CancellationToken cancellationToken)
        {
            Target.SetBool(_parameterName, _value);
            await UniTask.WaitForSeconds(Duration, cancellationToken: cancellationToken);
        }
    }
}
