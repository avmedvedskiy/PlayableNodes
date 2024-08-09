using UnityEditor;
using UnityEngine;

namespace PlayableNodes
{
    [CustomEditor(typeof(TrackPlayer))]
    public class TrackPlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = !TrackEditorPreview.IsPreviewing;
            EditorGUI.BeginChangeCheck();
            //DrawTracksHeader();
            var track = serializedObject.FindProperty(TrackHelper.TRACK_PROPERTY);
            TrackListDrawer.DrawTrack(track, serializedObject.targetObject);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            GUI.enabled = true;
        }
        
    }
}