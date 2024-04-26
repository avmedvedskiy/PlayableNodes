using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace PlayableNodes.Extensions
{
    public static class Extensions
    {
        public static float TotalDuration(this IAnimation animation) =>
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
        
        public static float TotalDuration(this TrackNode[] trackNodes)
        {
            float duration = 0;
            for (int i = 0; i < trackNodes.Length; i++)
            {
                var anim = trackNodes[i];
                var totalDuration = anim.TotalDuration();
                if (totalDuration > duration)
                    duration = totalDuration;
            }

            return duration;
        }

        
        public static async UniTask PlayAsync(this IAnimation[] animations, Object target)
        {
            var taskList = ListPool<UniTask>.Get();
            foreach (var a in animations)
            {
                if (a is { Enable: true })
                {
                    a.SetTarget(target);
                    taskList.Add(a.PlayAsync());
                }
            }

            await UniTask.WhenAll(taskList);
            ListPool<UniTask>.Release(taskList);
        }
        
        public static async UniTask PlayAsync(this TrackNode[] nodes)
        {
            var list = ListPool<UniTask>.Get();
            foreach (var track in nodes)
            {
                if(track.IsActive)
                    list.Add(track.PlayAsync());
            }

            await UniTask.WhenAll(list);
            ListPool<UniTask>.Release(list);
        }
        
    }
}