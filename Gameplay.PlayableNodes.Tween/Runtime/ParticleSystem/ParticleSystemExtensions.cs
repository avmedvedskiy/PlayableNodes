using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace PlayableNodes.Particle
{
    public static class ParticleSystemExtensions
    {
#if UNITY_EDITOR
        private static async UniTask PlayPreviewAsync(this ParticleSystem system,
            float duration,
            bool withChildren,
            CancellationToken cancellationToken = default)
        {
            if (system == null)
            {
                Debug.LogWarning($"Particle system is null");
                return;
            }
            
            bool disableWhenComplete = system.gameObject.activeSelf == false;
            if (disableWhenComplete)
            {
                system.gameObject.SetActive(true);
                await UniTask.Yield();
            }

            var childSystems = system.GetComponentsInChildren<ParticleSystem>().ToList();
            childSystems.ForEach(x => x.useAutoRandomSeed = false);

            //system.Play(withChildren);
            var time = 0f;
            var initialTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                system.Simulate(time, false);
                if (withChildren)
                    childSystems.ForEach(x => x.Simulate(time, false));

                //system.Pause(withChildren);
                //system.time = time;
                time = (float)UnityEditor.EditorApplication.timeSinceStartup - initialTime;

                if (time >= duration || cancellationToken.IsCancellationRequested)
                    break;
            }

            if (!system.main.loop)
            {
                system.Stop(withChildren, ParticleSystemStopBehavior.StopEmittingAndClear);
                if(disableWhenComplete)
                    system.gameObject.SetActive(false);
            }
        }
#endif

        private static async UniTask PlayRuntimeAsync(this ParticleSystem system,
            float duration,
            bool withChildren,
            CancellationToken cancellationToken = default)
        {
            bool disableWhenComplete = system.gameObject.activeSelf == false;
            if (disableWhenComplete)
            {
                system.gameObject.SetActive(true);
                await UniTask.Yield();
            }

            system.Play(withChildren);
            await UniTask
                .WaitForSeconds(duration, cancellationToken: cancellationToken)
                .SuppressCancellationThrow();
            if (system && !system.main.loop)
            {
                system.Stop(withChildren, ParticleSystemStopBehavior.StopEmittingAndClear);
                if(disableWhenComplete)
                    system.gameObject.SetActive(false);
            }
        }

        public static async UniTask StopAsync(this ParticleSystem system,
            float duration,
            bool withChildren = true,
            CancellationToken cancellationToken = default)
        {
            system.Stop(withChildren, ParticleSystemStopBehavior.StopEmitting);
            await UniTask
                .WaitForSeconds(duration, cancellationToken: cancellationToken)
                .SuppressCancellationThrow();
            //system.Stop(withChildren, ParticleSystemStopBehavior.StopEmittingAndClear);
        }


        public static UniTask PlayAsync(this ParticleSystem system,
            float duration,
            bool withChildren = true,
            CancellationToken cancellationToken = default)
        {
#if UNITY_EDITOR
            return Application.isPlaying
                ? system.PlayRuntimeAsync(duration, withChildren, cancellationToken)
                : system.PlayPreviewAsync(duration, withChildren, cancellationToken);
#else
            return system.PlayRuntimeAsync(duration,withChildren,cancellationToken);
#endif
        }
    }
}