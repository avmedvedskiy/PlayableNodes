using UnityEngine;

namespace PlayableNodes
{
    public class EnabledScope : EditorGUIScope
    {
        private readonly bool _previousState;

        public EnabledScope()
        {
            _previousState = GUI.enabled;
            GUI.enabled = true;
        }
        
        protected override void CloseScope()
        {
            GUI.enabled = _previousState;
        }
    }
}