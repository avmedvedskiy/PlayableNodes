using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[Description("Changes the sorting order of a SortingGroup")]
public class ChangeSortingGroupOrder : TargetAnimation<SortingGroup>
{
    [SerializeField] private int _value;
    
    protected override async UniTask Play(CancellationToken cancellationToken)
    {
        Target.sortingOrder = _value;
        await UniTask.CompletedTask;
    }
}
