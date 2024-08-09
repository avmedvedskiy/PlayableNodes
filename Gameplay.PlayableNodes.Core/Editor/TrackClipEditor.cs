using System;
using UnityEditor;
using UnityEngine;

namespace PlayableNodes
{
    [CustomEditor(typeof(TrackClip))]
    public class TrackClipEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = !TrackEditorPreview.IsPreviewing;
            EditorGUI.BeginChangeCheck();
            DrawTargets();
            EditorGUILayout.LabelField("Animation");
            var tracks = serializedObject.FindProperty(TrackHelper.TRACKS_PROPERTY);
            TrackListDrawer.DrawHeaderAndTracks(tracks, serializedObject.targetObject);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            GUI.enabled = true;
        }

        private void DrawTargets()
        {
            EditorGUILayout.LabelField("Target References");
            var references = serializedObject.FindProperty(TrackHelper.BINDING_REFERENCES);
            for (int i = 0; i < references.arraySize; i++)
            {
                var reference = references.GetArrayElementAtIndex(i);
                var typeProperty = reference.FindPropertyRelative(TrackHelper.REFERENCE_TYPE);
                var pathProperty = reference.FindPropertyRelative(TrackHelper.REFERENCE_PATH);
                EditorGUILayout.SelectableLabel($"{pathProperty.stringValue}({typeProperty.stringValue})", EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
        }
    }
}