using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes;
using UnityEngine;

[Serializable]
[Description("Sets the active state of the Transform's GameObject")]
public class SetActiveTransform : TargetAnimation<Transform>
{
    [SerializeField] private bool _active;

    protected override UniTask Play(CancellationToken cancellationToken)
    {
        Target.gameObject.SetActive(_active);
        return UniTask.CompletedTask;
    }
}
