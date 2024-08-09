using UnityEngine;

namespace PlayableNodes
{
    
    /// <summary>
    /// Disable GUI.Enabled based with previous state
    /// </summary>
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