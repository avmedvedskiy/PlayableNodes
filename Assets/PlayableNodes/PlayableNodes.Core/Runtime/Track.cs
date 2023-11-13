using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace PlayableNodes
{
    [Serializable]
    public class Track
    {
        [SerializeField] private bool _isActive = true;
        [SerializeField] private string _name;
        [SerializeField] private List<TrackNode> _trackNodes;
        public bool IsActive => _isActive;
        public string Name => _name;

        public IReadOnlyList<TrackNode> Nodes => _trackNodes;

        public async UniTask PlayAsync()
        {
            List<UniTask> list = ListPool<UniTask>.Get();
            foreach (var track in _trackNodes)
            {
                if(track.IsActive)
                    list.Add(track.PlayAsync());
            }

            await UniTask.WhenAll(list);
            ListPool<UniTask>.Release(list);
        }
        
        public float TotalDuration()
        {
            float duration = 0;
            for (int i = 0; i < _trackNodes.Count; i++)
            {
                var anim = _trackNodes[i];
                var totalDuration = anim.TotalDuration();
                if (totalDuration > duration)
                    duration = totalDuration;
            }

            return duration;
        }
    }
}