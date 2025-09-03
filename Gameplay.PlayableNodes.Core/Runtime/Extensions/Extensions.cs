using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace PlayableNodes.Extensions
{
    public static class Extensions
    {
        private static float TotalDuration(this IAnimation animation) =>
            animation == null
                ? 0f
                : animation.Duration + animation.Delay;

        public static float TotalDuration(this IAnimation[] animations)
        {
            float duration = 0;
            for (int i = 0; i < animations.Length; i++)
            {
                var anim = animations[i];
                var totalDuration = anim.TotalDuration();
                if (totalDuration > duration)
                    duration = totalDuration;
            }

            return duration;
        }


        public static async UniTask PlayAsync(this IAnimation[] animations, Object target,
            CancellationToken cancellationToken = default)
        {
            var taskList = ListPool<UniTask>.Get();
            foreach (var a in animations)
            {
                if (a is { Enable: true })
                {
                    a.SetTarget(target);
                    taskList.Add(a.PlayAsync(cancellationToken));
                }
            }

            await UniTask.WhenAll(taskList);
            ListPool<UniTask>.Release(taskList);
        }

        public static async UniTask PlayAsync(this TrackNode[] nodes, CancellationToken cancellationToken = default)
        {
            var list = ListPool<UniTask>.Get();
            foreach (var track in nodes)
            {
                if (track.IsActive)
                    list.Add(track.PlayAsync(cancellationToken));
            }

            await UniTask.WhenAll(list);
            ListPool<UniTask>.Release(list);
        }

        private static IEnumerable<IAnimation> AllAnimations(this ITracksPlayer tracksPlayer)
        {
            foreach (var track in tracksPlayer.Tracks)
            {
                if(track.Nodes == null)
                    continue;
                foreach (var node in track.Nodes)
                {
                    foreach (var a in node.Animations)
                    {
                        if (a is { Enable: true })
                            yield return a;
                    }
                }
            }
        }

        public static void DrawAnimationGizmos(this ITracksPlayer player)
        {
            foreach (var a in player.AllAnimations())
            {
                if (a is IDrawGizmosSelected drawGizmosSelected)
                    drawGizmosSelected.OnDrawGizmosSelected();
            }
        }

        public static void ChangeEndValueByPin<T>(this ITracksPlayer tracksPlayer, int pin, T value)
        {
            foreach (var a in tracksPlayer.AllAnimations())
            {
                if (a.Pin == pin && a is IChangeEndValue<T> changeEndValue)
                {
                    changeEndValue.ChangeEndValue(value);
                }
            }
        }
        
        public static void ChangeTargetByPin(this ITracksPlayer tracksPlayer, int pin, Object value)
        {
          foreach (var track in tracksPlayer.Tracks)
          {
            if(track.Nodes == null)
              continue;
            foreach (var node in track.Nodes)
            {
              foreach (var a in node.Animations)
              {
                if(a.Pin == pin)
                  node.SetContext(value);
              }
            }
          }
        }
    }
}