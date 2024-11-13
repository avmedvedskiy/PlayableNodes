using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableNodes
{
    [Serializable]
    public class RebuildLayoutEveryFrame : TargetAnimation<RectTransform>
    {
        private float _time;

        protected override async UniTask Play(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _time < Duration)
            {
                LayoutRebuilder.MarkLayoutForRebuild(Target);
                await UniTask.Yield(PlayerLoopTiming.Update);
                _time += Time.deltaTime;
            }
        }
    }
}