using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [Serializable]
    [Description("Sets a trigger parameter in an Animator")]
    public class SetTriggerAnimator : TargetAnimation<Animator>
    {
        [SerializeField] private string _triggerName;

        protected override async UniTask Play(CancellationToken cancellationToken)
        {
            Target.SetTrigger(_triggerName);
            await UniTask.WaitForSeconds(Duration, cancellationToken: cancellationToken);
        }
    }
}