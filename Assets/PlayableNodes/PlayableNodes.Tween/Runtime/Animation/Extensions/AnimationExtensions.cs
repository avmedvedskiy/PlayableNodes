using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace PlayableNodes.Animations
{
    public static class AnimationExtensions
    {
        private static async UniTask PlayPreviewAsync(this Animation animation, string animationName)
        {
            var clip = animation.GetClip(animationName);
            if (clip == null)
            {
                Debug.LogWarning($"Can't find animation {animationName} to preview", animation);
                return;
            }

            var duration = animation.GetClip(animationName).averageDuration;
            var time = 0f;
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                animation[animationName].enabled = true;
                animation[animationName].time = time;
                animation[animationName].weight = 1f;
                animation.Sample();
                time += Time.deltaTime;

                if (time >= duration)
                    break;
            }
        }

        private static UniTask PlayRuntimeAsync(this Animation animation, string animationName)
        {
            animation.Play(animationName);
            return UniTask.WaitWhile(() => animation.isPlaying);
        }

        public static UniTask PlayAsync(this Animation animation, string animationName) =>
            Application.isPlaying
                ? animation.PlayRuntimeAsync(animationName)
                : animation.PlayPreviewAsync(animationName);
    }
}