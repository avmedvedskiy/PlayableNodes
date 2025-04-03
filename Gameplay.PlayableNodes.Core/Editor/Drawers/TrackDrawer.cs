using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    public static class TrackDrawer
    {
        private static Dictionary<string, ReorderableList> _reorderableCache = new();
        private static GUILayoutOption MaxWidth15 => GUILayout.MaxWidth(15f);
        private static GUILayoutOption MaxWidth30 => GUILayout.MaxWidth(30f);
        private static GUIStyle FoldoutStyle => new(EditorStyles.foldout) { fixedWidth = 5f };

        public static void DrawTrack(SerializedProperty trackProperty, Object player)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            var deleted = DrawTrackHeader(trackProperty, player);
            if (deleted)
            {
                return;
            }

            var isActiveProperty = trackProperty.FindPropertyRelative(TrackHelper.IS_ACTIVE_PROPERTY);
            using (new DisableScope(isActiveProperty.boolValue))
            {
                var nodes = trackProperty.FindPropertyRelative(TrackHelper.TRACK_NODES_PROPERTY);
                if (trackProperty.isExpanded)
                {
                    EditorGUI.indentLevel += 1;
                    DrawTrackNodes(nodes, isActiveProperty.boolValue);
                    EditorGUI.indentLevel -= 1;
                }
            }
            EditorGUILayout.EndVertical();
        }

        public static void ClearTrack(SerializedProperty track)
        {
            track.FindPropertyRelative(TrackHelper.IS_ACTIVE_PROPERTY).boolValue = true;
            track.FindPropertyRelative(TrackHelper.NAME_PROPERTY).stringValue = default;
            track.FindPropertyRelative(TrackHelper.TRACK_NODES_PROPERTY).arraySize = 0;
        }

        private static bool DrawTrackHeader(SerializedProperty track, Object player)
        {
            var isActiveProperty = track.FindPropertyRelative(TrackHelper.IS_ACTIVE_PROPERTY);
            var nameProperty = track.FindPropertyRelative(TrackHelper.NAME_PROPERTY);
            EditorGUILayout.BeginHorizontal();
            track.isExpanded = EditorGUILayout.Toggle(track.isExpanded, FoldoutStyle, MaxWidth15);

            DrawerTools.ColoredToggleProperty(isActiveProperty, MaxWidth15);

            using (new DisableScope(isActiveProperty.boolValue))
            {
                nameProperty.stringValue = EditorGUILayout.TextField(nameProperty.stringValue);
                DrawPreviewButton(player, nameProperty.stringValue);
            }


            var deleted = DeleteButton(track);
            EditorGUILayout.EndHorizontal();
            return deleted;
        }

        private static void DrawPreviewButton(Object player, string nameProperty)
        {
            if (TrackEditorPreview.IsPreviewing && nameProperty == TrackEditorPreview.IsPreviewingName)
            {
                using (new EnabledScope())
                using (new ColorScope(Color.red))
                    if (GUILayout.Button("    Stop   "))
                    {
                        TrackEditorPreview.StopPreviewAnimation();
                    }
            }
            else
            {
                using (new ColorScope(Color.magenta))
                    if (GUILayout.Button("Preview"))
                    {
                        TrackEditorPreview.PreviewAnimation((ITracksPlayer)player, nameProperty);
                    }
            }
        }


        private static void DrawTrackNodes(SerializedProperty nodes, bool isActiveTrack)
        {
            for (int i = 0; i < nodes.arraySize; i++)
            {
                var node = nodes.GetArrayElementAtIndex(i);
                DrawTrackNode(node, isActiveTrack);
            }
            
            using (new ColorScope(Color.green))
            {
                if (GUILayout.Button("+ Add New Track Node"))
                {
                    AddNewTrackNode(nodes);
                }
            }
        }

        private static void AddNewTrackNode(SerializedProperty nodes)
        {
            nodes.InsertArrayElementAtIndex(nodes.arraySize == 0 ? 0 : nodes.arraySize - 1);
            var newNode = nodes.GetArrayElementAtIndex(nodes.arraySize - 1);
            newNode.FindPropertyRelative(TrackHelper.IS_ACTIVE_PROPERTY).boolValue = true;
            newNode.FindPropertyRelative(TrackHelper.CONTEXT_PROPERTY).objectReferenceValue = default;
            newNode.FindPropertyRelative(TrackHelper.ANIMATIONS_PROPERTY).arraySize = 0;
        }

        private static void DrawTrackNode(SerializedProperty node, bool isActiveTrack)
        {
            EditorGUILayout.BeginHorizontal();

            var contextProperty = node.FindPropertyRelative(TrackHelper.CONTEXT_PROPERTY);
            var activeProperty = node.FindPropertyRelative(TrackHelper.IS_ACTIVE_PROPERTY);
            node.isExpanded = EditorGUILayout.Toggle(node.isExpanded, FoldoutStyle, MaxWidth15);
            using (new DisableScope(!TrackEditorPreview.IsPreviewing))
            {
                DrawerTools.ColoredToggleProperty(activeProperty, MaxWidth30);
            }

            bool enabled = activeProperty.boolValue && isActiveTrack;
            using (new DisableScope(enabled))
            {
                var animations = node.FindPropertyRelative(TrackHelper.ANIMATIONS_PROPERTY);
                SelectObjectDrawer.PropertyField(contextProperty, 
                    animations,
                    new GUIContent(string.Empty, contextProperty.tooltip));

                //EditorGUILayout.PropertyField(contextProperty, GUIContent.none);

                bool deleted = DeleteButton(node);
                EditorGUILayout.EndHorizontal();

                if (deleted)
                {
                    AnimationDrawer.ClearCache();
                    return;
                }

                if (!node.isExpanded)
                    return;

                EditorGUI.indentLevel += 1;

                //DrawAnimationControl(animations);
                AnimationDrawer.SetTargetToAnimations(animations, contextProperty.objectReferenceValue);
                AnimationDrawer.DrawAnimations(animations);

                EditorGUI.indentLevel -= 1;
            }
        }

        private static bool DeleteButton(SerializedProperty property)
        {
            using (new ColorScope(Color.red))
            {
                if (GUILayout.Button(TrackHelper.DELETE_TEXT, MaxWidth15))
                {
                    property.DeleteCommand();
                    return true;
                }
            }

            return false;
        }
    }
}