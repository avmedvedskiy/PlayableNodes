using System;
using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Experimental
{
    [CustomEditor(typeof(RetargetTrackPlayerCollection))]
    public class RetargetTrackPlayerCollectionEditor : Editor
    {
        private void OnEnable()
        {
            AutoSetTargets();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(TrackHelper.CLIP_PROPERTY));
            DrawTargets();
            DrawStorageTracks();
            if (EditorGUI.EndChangeCheck())
            {
                AutoSetTargets();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawStorageTracks()
        {
            EditorGUI.BeginChangeCheck();
            var storageProperty = serializedObject.FindProperty(TrackHelper.CLIP_PROPERTY);
            var storage = storageProperty.objectReferenceValue;
            if(storage == null)
                return;
            var o = new SerializedObject(storage);
            var tracksProperty = o.FindProperty(TrackHelper.TRACKS_PROPERTY); 
            TrackListDrawer.DrawTracks(tracksProperty, serializedObject.targetObject);
            if (EditorGUI.EndChangeCheck())
            {
                o.ApplyModifiedProperties();
                EditorUtility.SetDirty(storage);
            }
        }

        private void AutoSetTargets()
        {
            var bindings = serializedObject.FindProperty(TrackHelper.BINDINGS);
            var storage = (TrackClip)serializedObject.FindProperty(TrackHelper.CLIP_PROPERTY).objectReferenceValue;
            var rootObject = ((Component)serializedObject.targetObject).transform;
            if(storage == null)
                return;

            bindings.arraySize = storage.Bindings.Count;
            for (int i = 0; i < storage.Bindings.Count; i++)
            {
                var element = bindings.GetArrayElementAtIndex(i);
                var bindReference = storage.Bindings[i];
                if(element.objectReferenceValue  == null)
                    element.objectReferenceValue = rootObject.Find(bindReference.Path).GetComponent(Type.GetType(bindReference.TypeName));
            }
            serializedObject.ApplyModifiedProperties();

        }
        
        private void DrawTargets()
        {
            EditorGUI.indentLevel++;
            var bindings = serializedObject.FindProperty(TrackHelper.BINDINGS);
            var storage = (TrackClip)serializedObject.FindProperty(TrackHelper.CLIP_PROPERTY).objectReferenceValue;

            bindings.isExpanded = EditorGUILayout.Foldout(bindings.isExpanded,bindings.displayName);

            if (bindings.isExpanded)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < bindings.arraySize; i++)
                {
                    var targetElement = storage.Bindings[i];
                    var element = bindings.GetArrayElementAtIndex(i);
                    var type = Type.GetType(targetElement.TypeName);
                    using (new ColorScope(element.objectReferenceValue == null ? Color.red : Color.white))
                    {
                        EditorGUILayout.ObjectField(element, type, new GUIContent(targetElement.Path));
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;

        }
    }
}