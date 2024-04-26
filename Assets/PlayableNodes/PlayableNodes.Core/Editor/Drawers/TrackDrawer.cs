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
            var deleted = DrawTrackHeader(trackProperty, player);
            if (deleted)
                return;

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

            ColoredToggleProperty(isActiveProperty, MaxWidth15);

            using (new DisableScope(isActiveProperty.boolValue && !TrackEditorPreview.IsPreviewing))
            {
                nameProperty.stringValue = EditorGUILayout.TextField(nameProperty.stringValue);
                using (new ColorScope(Color.magenta))
                {
                    if (GUILayout.Button("Preview"))
                    {
                        TrackEditorPreview.PreviewAnimation((ITracksPlayer)player, nameProperty.stringValue);
                    }
                }
            }

            var deleted = DeleteButton(track);
            EditorGUILayout.EndHorizontal();
            return deleted;
        }


        private static void DrawTrackNodes(SerializedProperty nodes, bool isActiveTrack)
        {
            using (new ColorScope(Color.green))
            {
                if (GUILayout.Button("+ Add New Track Node"))
                {
                    AddNewTrackNode(nodes);
                }
            }

            for (int i = 0; i < nodes.arraySize; i++)
            {
                var node = nodes.GetArrayElementAtIndex(i);
                DrawTrackNode(node, isActiveTrack);
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
                ColoredToggleProperty(activeProperty, MaxWidth30);
            }
            
            bool enabled = activeProperty.boolValue && isActiveTrack;
            using (new DisableScope(enabled))
            {
                var animations = node.FindPropertyRelative(TrackHelper.ANIMATIONS_PROPERTY);
                SelectObjectDrawer.PropertyField(contextProperty, animations, GUIContent.none);

                //EditorGUILayout.PropertyField(contextProperty, GUIContent.none);

                bool deleted = DeleteButton(node);
                EditorGUILayout.EndHorizontal();

                if (deleted)
                    return;

                if (!node.isExpanded)
                    return;

                EditorGUI.indentLevel += 1;

                //DrawAnimationControl(animations);
                SetTargetToAnimations(animations, contextProperty.objectReferenceValue);
                DrawAnimations(animations);

                EditorGUI.indentLevel -= 1;
            }
        }

        private static void DrawAnimations(SerializedProperty animations)
        {
            //чтобы адекватно работали листы, их нужно сохранять, поєтому тут такие костыли с сохранением в кеш
            if (!_reorderableCache.TryGetValue(animations.propertyPath, out var list))
            {
                list = CreateListView();
            }

            try
            {
                list.DoLayoutList();
            }
            catch
            {
                //Если пеерключать туда сюда, то проперти становится диспоузнотым, и я не нашел метода который бы проверил на это
                list = CreateListView();
                list.DoLayoutList();
            }

            float ElementHeightCallback(int index)
            {
                var animation = animations.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(animation);
            }

            void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
            {
                var animation = animations.GetArrayElementAtIndex(index);
                var boolProperty = animation.FindPropertyRelative(TrackHelper.ENABLE_PROPERTY);

                ColoredToggleLeft(
                    new Rect(rect.x + 20, rect.y, EditorGUIUtility.labelWidth - 20f, EditorGUIUtility.singleLineHeight),
                    boolProperty,
                    new GUIContent(animation.displayName));

                using (new DisableScope(boolProperty?.boolValue ?? true))
                {
                    rect.x += 10;
                    EditorGUI.PropertyField(rect, animation, GUIContent.none);
                }
                //DrawAnimation(animations.GetArrayElementAtIndex(index), enabled);
            }

            ReorderableList CreateListView()
            {
                list = new ReorderableList(animations.serializedObject, animations, true, false, true, true);
                list.drawElementCallback += DrawElementCallback;
                list.elementHeightCallback += ElementHeightCallback;
                _reorderableCache[animations.propertyPath] = list;
                return list;
            }
        }

        private static void SetTargetToAnimations(SerializedProperty animations, Object target)
        {
            for (int i = 0; i < animations.arraySize; i++)
            {
                var animation = animations.GetArrayElementAtIndex(i);
                if (target != null)
                    ((IAnimation)animation.managedReferenceValue)?.SetTarget(target);
                //DrawAnimation(animation, enabled);
            }
        }

        private static void DrawAnimation(SerializedProperty animation, bool enabled)
        {
            var boolProperty = animation.FindPropertyRelative(TrackHelper.ENABLE_PROPERTY);
            using (new DisableScope(enabled))
            {
                ColoredToggleLeft(GetAnimationEnableToggleRect(), boolProperty, new GUIContent(animation.displayName));
            }

            using (new DisableScope(enabled && (boolProperty?.boolValue ?? true)))
            {
                EditorGUILayout.PropertyField(animation, GUIContent.none);
            }
        }

        private static Rect GetAnimationEnableToggleRect()
        {
            var toggleRect = EditorGUILayout.GetControlRect(false, 0f);
            toggleRect.x += 5f;
            toggleRect.width = EditorGUIUtility.labelWidth - 10f;
            toggleRect.height = EditorGUIUtility.singleLineHeight;
            return toggleRect;
        }

        private static void DrawAnimationControl(SerializedProperty animations)
        {
            EditorGUILayout.BeginHorizontal();
            using (new ColorScope(Color.green))
            {
                if (GUILayout.Button("+ New Animation"))
                {
                    animations.InsertArrayElementAtIndex(animations.arraySize == 0 ? 0 : animations.arraySize - 1);
                    var newAnimation = animations.GetArrayElementAtIndex(animations.arraySize - 1);
                    newAnimation.managedReferenceValue = null;
                }
            }

            using (new ColorScope(Color.red))
            {
                if (GUILayout.Button("- Delete All Animations"))
                {
                    animations.arraySize = 0;
                }
            }

            EditorGUILayout.EndHorizontal();
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

        private static void ColoredToggleProperty(SerializedProperty boolProperty, params GUILayoutOption[] options)
        {
            if (boolProperty != null)
            {
                using (new ColorScope(boolProperty.boolValue ? Color.green : Color.red))
                {
                    boolProperty.boolValue = EditorGUILayout.Toggle(boolProperty.boolValue, options);
                }
            }
        }

        private static void ColoredToggleLeft(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property != null)
            {
                using (new ColorScope(property.boolValue ? Color.green : Color.red))
                {
                    property.boolValue =
                        EditorGUI.ToggleLeft(position, label,
                            property.boolValue);
                }
            }
        }
    }
}