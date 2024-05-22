using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace PlayableNodes
{
    public static class AnimationDrawer
    {
        private static Dictionary<string, ReorderableList> _reorderableCache = new();

        private static int[] _pinArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private static string[] _pinContent = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        public static void SetTargetToAnimations(SerializedProperty animations, Object target)
        {
            for (int i = 0; i < animations.arraySize; i++)
            {
                var animation = animations.GetArrayElementAtIndex(i);
                if (target != null)
                    ((IAnimation)animation.managedReferenceValue)?.SetTarget(target);
                //DrawAnimation(animation, enabled);
            }
        }

        public static void DrawAnimations(SerializedProperty animations)
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
                var pinProperty = animation.FindPropertyRelative(TrackHelper.PIN_PROPERTY);

                DrawerTools.ColoredToggleLeft(
                    new Rect(rect.x + 20, rect.y, EditorGUIUtility.labelWidth - 40f, EditorGUIUtility.singleLineHeight),
                    boolProperty,
                    new GUIContent(animation.displayName));

                using (new DisableScope(boolProperty?.boolValue ?? true))
                {
                    PinProperty(rect, pinProperty);
                    EditorGUI.PropertyField(new Rect(rect.x + 10, rect.y, rect.width, rect.height),
                        animation,
                        GUIContent.none);
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

        private static void PinProperty(Rect rect, SerializedProperty pinProperty)
        {
            if(pinProperty == null)
                return;
            
            using (new ColorScope(pinProperty.intValue > 0 ? Color.cyan : Color.white))
            {
                pinProperty.intValue = EditorGUI.IntPopup(
                    new Rect(rect.width / 2f - 20f, rect.y, 30f, EditorGUIUtility.singleLineHeight),
                    string.Empty,
                    pinProperty.intValue,
                    _pinContent,
                    _pinArray);
            }
        }


        /*
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
        */
    }
}