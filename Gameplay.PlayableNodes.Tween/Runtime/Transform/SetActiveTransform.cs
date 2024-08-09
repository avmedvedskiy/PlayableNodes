using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes;
using UnityEngine;

[Serializable]
public class SetActiveTransform : TargetAnimation<Transform>
{
    [SerializeField] private bool _active;

    protected override UniTask Play(CancellationToken cancellationToken)
    {
        Target.gameObject.SetActive(_active);
        return UniTask.CompletedTask;
    }
}
