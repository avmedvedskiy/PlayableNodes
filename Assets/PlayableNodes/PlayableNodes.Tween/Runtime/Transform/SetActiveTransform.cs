using System;
using Cysharp.Threading.Tasks;
using PlayableNodes;
using UnityEngine;

[Serializable]
public class SetActiveTransform : TargetAnimation<Transform>
{
    [SerializeField] private bool _active;

    protected override UniTask Play()
    {
        Target.gameObject.SetActive(_active);
        return UniTask.CompletedTask;
    }
}
