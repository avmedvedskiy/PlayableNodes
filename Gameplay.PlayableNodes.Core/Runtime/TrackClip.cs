using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayableNodes
{
    public class TrackClip : ScriptableObject
    {
        [System.Serializable]
        public struct BindingReference
        {
            [SerializeField]
            private string _path;
            [SerializeField]
            private string _typeName;

            public string Path => _path;
            public string TypeName => _typeName;

            public BindingReference(string path, string typeName)
            {
                _path = path;
                _typeName = typeName;
            }
        }
        
        [SerializeField] private List<Track> _tracks;
        [SerializeField] private List<BindingReference> _bindingReferences;

        public IReadOnlyList<Track> Tracks => _tracks;
        public List<BindingReference> Bindings => _bindingReferences;

        internal Track FindTrack(string trackName) => _tracks.Find(x => x.Name == trackName);

    }
}