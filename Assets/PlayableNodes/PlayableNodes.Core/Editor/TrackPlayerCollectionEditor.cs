using System;
using UnityEditor;
using UnityEngine;

namespace PlayableNodes
{
    [CustomEditor(typeof(TrackPlayerCollection))]
    public class TrackPlayerCollectionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = !TrackEditorPreview.IsPreviewing;
            EditorGUI.BeginChangeCheck();
            DrawTracksHeader();
            var tracksProperty = serializedObject.FindProperty(TrackHelper.TRACKS_PROPERTY);
            TrackListDrawer.DrawTracks(tracksProperty, serializedObject.targetObject);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            GUI.enabled = true;
        }

        private void DrawTracksHeader()
        {
            EditorGUILayout.BeginHorizontal();
            TrackListDrawer.DrawTrackHeader(serializedObject.FindProperty(TrackHelper.TRACKS_PROPERTY));
            DrawSaveLoadButton();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSaveLoadButton()
        {
            using (new ColorScope(Color.yellow))
            {
                if (GUILayout.Button("Save Track"))
                {
                    var path = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
                    if(string.IsNullOrEmpty( path))
                        return;
                
                    var storage = CreateAsset<TrackClip>(GetRelativePath(path), $"{serializedObject.targetObject.name}Storage");
                    if(storage)
                        SaveToStorage(storage);
                }
            }
            
            using (new ColorScope(Color.green))
            {
                if (GUILayout.Button("Load Track"))
                {
                    var path = EditorUtility.OpenFilePanel("Select storage", Application.dataPath, "asset");
                    if(string.IsNullOrEmpty( path))
                        return;
                    
                    TrackClip clip = AssetDatabase.LoadAssetAtPath<TrackClip>(GetRelativePath(path));
                    if(clip)
                        LoadFromStorage(clip);
                }
            }
        }

        private static string GetRelativePath(string path) => path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));

        private void LoadFromStorage(TrackClip clip)
        {
            EditorUtility.CopySerializedManagedFieldsOnly(clip,serializedObject.targetObject);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            AutoSetTargets(clip);
            serializedObject.ApplyModifiedProperties();

        }

        private void AutoSetTargets(TrackClip trackClip)
        {
            var property = serializedObject.FindProperty(TrackHelper.TRACKS_PROPERTY);
            var rootObject = ((Component)serializedObject.targetObject).transform;
            int i = 0;
            while (property.NextVisible(true))
            {
                if (property.name == TrackHelper.CONTEXT_PROPERTY)
                {
                    var bindReference = trackClip.Bindings[i];
                    var referenceObject = rootObject.Find(bindReference.Path)
                        .GetComponent(Type.GetType(bindReference.TypeName));
                    property.objectReferenceValue = referenceObject;
                    i++;
                }
            }
        }

        private void SaveToStorage(TrackClip clip)
        {
            EditorUtility.CopySerializedManagedFieldsOnly(serializedObject.targetObject, clip);
            clip.Bindings.Clear();
            var tracks = serializedObject.FindProperty(TrackHelper.TRACKS_PROPERTY);
            while (tracks.NextVisible(true))
            {
                if (tracks.name == TrackHelper.CONTEXT_PROPERTY)
                {
                    var targetObject = tracks.objectReferenceValue;
                    var type = targetObject.GetType();
                    var typeName = type.AssemblyQualifiedName;
                    var targetName = targetObject.name;
                    if (targetObject is Component component)
                    {
                        var parentTransform = ((Component)serializedObject.targetObject).transform;
                        targetName = AnimationUtility.CalculateTransformPath(component.transform, parentTransform);
                    }

                    clip.Bindings.Add(new TrackClip.BindingReference(targetName, typeName));
                }
            }
            EditorUtility.SetDirty(clip);
            Repaint();
            AssetDatabase.Refresh();
        }

        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");
            if (asset != null)
            {
                var result = EditorUtility.DisplayDialog($"Override {assetName}?", $"File {assetName} found, do you want to override it?", "Yes", "No");
                return result
                    ? asset
                    : null;
            }

            asset = CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.Refresh();
            return asset;
        }
    }
}