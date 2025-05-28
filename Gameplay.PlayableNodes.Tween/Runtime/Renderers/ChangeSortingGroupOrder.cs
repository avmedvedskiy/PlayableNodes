using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class ChangeSortingGroupOrder : TargetAnimation<SortingGroup>
{
    [SerializeField] private int _value;
    
    protected override async UniTask Play(CancellationToken cancellationToken)
    {
        Target.sortingOrder = _value;
        await UniTask.CompletedTask;
    }
}
