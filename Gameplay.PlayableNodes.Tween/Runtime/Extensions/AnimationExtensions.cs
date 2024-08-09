using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace PlayableNodes.Animations
{
    public static class AnimationExtensions
    {
#if UNITY_EDITOR
        private static async UniTask PlayPreviewAsync(this Animation animation, string animationName,
            CancellationToken cancellationToken = default)
        {
            var clip = animation.GetClip(animationName);
            if (clip == null)
            {
                Debug.LogWarning($"Can't find animation {animationName} to preview", animation);
                return;
            }

            var duration = animation.GetClip(animationName).averageDuration;
            var time = 0f;

            var initialTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                animation[animationName].enabled = true;
                animation[animationName].time = time;
                animation[animationName].weight = 1f;
                animation.Sample();
                time = (float)UnityEditor.EditorApplication.timeSinceStartup - initialTime;

                //lastFrameCount = Time.frameCount;
                if (time >= duration || cancellationToken.IsCancellationRequested)
                {
                    animation.Stop();
                    break;
                }
            }
        }
#endif

        private static async UniTask PlayRuntimeAsync(this Animation animation, string animationName,
            CancellationToken cancellationToken = default)
        {
            animation.Play(animationName);
            var isCanceled = await UniTask
                .WaitWhile(() => animation.isPlaying, cancellationToken: cancellationToken)
                .SuppressCancellationThrow();
            if (isCanceled)
            {
                animation[animationName].time = 1f;
                animation.Stop(animationName);
            }
        }

        public static UniTask PlayAsync(this Animation animation, string animationName,
            CancellationToken cancellationToken = default)
        {
#if UNITY_EDITOR
            return Application.isPlaying
                ? animation.PlayRuntimeAsync(animationName, cancellationToken)
                : animation.PlayPreviewAsync(animationName, cancellationToken);
#else
            return animation.PlayRuntimeAsync(animationName, cancellationToken);
#endif
        }
    }
}