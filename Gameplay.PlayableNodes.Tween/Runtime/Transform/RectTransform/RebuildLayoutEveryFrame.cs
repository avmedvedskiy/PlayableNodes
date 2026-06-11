using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableNodes
{
    [Serializable]
    [Description("Rebuilds the RectTransform layout every frame")]
    public class RebuildLayoutEveryFrame : TargetAnimation<RectTransform>
    {
        protected override async UniTask Play(CancellationToken cancellationToken)
        {
            var time = 0f;
            while (!cancellationToken.IsCancellationRequested && time < Duration)
            {
                LayoutRebuilder.MarkLayoutForRebuild(Target);
                await UniTask.Yield(PlayerLoopTiming.Update);
                time += Time.deltaTime;
            }
        }
    }
}
