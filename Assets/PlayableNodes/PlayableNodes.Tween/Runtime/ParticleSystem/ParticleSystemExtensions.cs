using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace PlayableNodes.Particle
{
    public static class ParticleSystemExtensions
    {
#if UNITY_EDITOR
        private static async UniTask PlayPreviewAsync(this ParticleSystem system,float duration, bool withChildren)
        {
            if (system == null)
            {
                Debug.LogWarning($"Particle system is null");
                return;
            }

            system.useAutoRandomSeed = false;
            system.Play(withChildren);
            var time = 0f;
            var initialTime = (float) UnityEditor.EditorApplication.timeSinceStartup;
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                system.Simulate(time, withChildren);
                system.time = time;
                time = (float) UnityEditor.EditorApplication.timeSinceStartup- initialTime;
                if (time >= duration)
                    break;
            }
            system.Stop(withChildren, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
#endif

        private static async UniTask PlayRuntimeAsync(this ParticleSystem system,float duration, bool withChildren)
        {
            system.Play(withChildren);
            await UniTask.WaitForSeconds(duration);
            system.Stop();
        }


        public static UniTask PlayAsync(this ParticleSystem system, float duration, bool withChildren = true)
        {
#if UNITY_EDITOR
            return Application.isPlaying
                    ? system.PlayRuntimeAsync(duration,withChildren)
                    : system.PlayPreviewAsync(duration,withChildren);
#else
            return system.PlayRuntimeAsync(duration,withChildren);
#endif
        }
    }
}