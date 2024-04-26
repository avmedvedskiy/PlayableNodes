using UnityEngine;

namespace PlayableNodes
{
    public class DisableScope : EditorGUIScope
    {
        private readonly bool _previousState;

        public DisableScope(bool enabled)
        {
            _previousState = GUI.enabled;
            GUI.enabled = _previousState && enabled;
        }
        
        protected override void CloseScope()
        {
            GUI.enabled = _previousState;
        }
    }
}