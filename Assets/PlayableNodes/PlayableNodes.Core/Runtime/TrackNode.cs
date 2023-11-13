using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ManagedReference;
using PlayableNodes.Extensions;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    public class TrackNode
    {
        [SerializeField] private bool _isActive;
        [SerializeField, SelectSceneObject] private Object _context;

        [SerializeReference, DynamicReference(propertyName = nameof(_context))]
        private IAnimation[] _animations;
        public bool IsActive => _isActive;

        public async UniTask PlayAsync()
        {
            if (_context == null)
                return;

            var taskList = ListPool<UniTask>.Get();
            foreach (var a in _animations)
            {
                if(a == null)
                    continue;
                
                a.SetTarget(_context);
                var task = a.Delay > 0
                    ? UniTask.WaitForSeconds(a.Delay).ContinueWith(a.PlayAsync)
                    : a.PlayAsync();
                taskList.Add(task);
            }

            await UniTask.WhenAll(taskList);
            ListPool<UniTask>.Release(taskList);
        }

        public void SetContext(Object context) => _context = context;

        public float TotalDuration()
        {
            float duration = 0;
            for (int i = 0; i < _animations.Length; i++)
            {
                var anim = _animations[i];
                var totalDuration = anim.TotalDuration();
                if (totalDuration > duration)
                    duration = totalDuration;
            }

            return duration;
        }
    }
}