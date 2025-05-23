﻿using System;
using System.Collections.Generic;
using System.Reflection;
using ManagedReference.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    
    public static class AnimationDrawer
    {
        private static readonly Dictionary<string, ReorderableList> _cache = new();

        private static readonly int[] _pinArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private static readonly string[] _pinContent = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        private static readonly Color[] _pinColors =
        {
            Color.white,
            Color.cyan,
            Color.magenta,
            Color.yellow,
            new(1.0f, 0.5f, 0.0f), // Оранжевый
            new(1.0f, 0.0f, 0.5f),  // Розовый
            new(0.5f, 0.0f, 0.5f), // Фиолетовый
            new(0.5f, 0.5f, 0.0f), // Оливковый
            new(0.5f, 0.25f, 0.0f), // Коричневый
            Color.blue,
        };

        private static MethodInfo IsValidProperty { get; }

        static AnimationDrawer()
        {
            IsValidProperty = typeof(SerializedProperty)
                .GetProperty("isValid", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetMethod;
        }

        public static void ClearCache() => _cache.Clear();

        public static void SetTargetToAnimations(SerializedProperty animations, Object target)
        {
            try
            {
                for (int i = 0; i < animations.arraySize; i++)
                {
                    var animation = animations.GetArrayElementAtIndex(i);
                    if (target != null)
                        ((IAnimation)animation.managedReferenceValue)?.SetTarget(target);
                    //DrawAnimation(animation, enabled);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void DrawAnimations(SerializedProperty animations)
        {
            //чтобы адекватно работали листы, их нужно сохранять, поєтому тут такие костыли с сохранением в кеш
            if ((_cache.TryGetValue(animations.propertyPath, out var list) && IsValid(list.serializedProperty)) ==
                false)
            {
                list = CreateListView();
            }

            try
            {
                list.DoLayoutList();
            }
            catch (ExitGUIException)
            {
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _cache.Remove(animations.propertyPath);
                //Если пеерключать туда сюда, то проперти становится диспоузнотым, и я не нашел метода который бы проверил на это
                list = CreateListView();
                list.DoLayoutList();
            }

            float ElementHeightCallback(int index) =>
                EditorGUI.GetPropertyHeight(animations.GetArrayElementAtIndex(index), true);

            void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
            {
                var animation = animations.GetArrayElementAtIndex(index);
                var boolProperty = animation.FindPropertyRelative(TrackHelper.ENABLE_PROPERTY);
                var pinProperty = animation.FindPropertyRelative(TrackHelper.PIN_PROPERTY);

                DrawerTools.ColoredToggleLeft(
                    new Rect(rect.x + 20, rect.y, EditorGUIUtility.labelWidth - 40f, EditorGUIUtility.singleLineHeight),
                    boolProperty,
                    new GUIContent(animation.displayName, animation.tooltip));

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
                _cache[animations.propertyPath] = list;
                return list;
            }
        }
        
        private static bool IsValid(SerializedProperty property) =>
            IsValidProperty != null && (bool)IsValidProperty.Invoke(property, null);

        private static void PinProperty(Rect rect, SerializedProperty pinProperty)
        {
            if (pinProperty == null)
                return;

            using (new ColorScope(_pinColors[pinProperty.intValue]))
            {
                if (GUI.Button(new Rect(rect.width / 2f - 20f, rect.y, 30f, EditorGUIUtility.singleLineHeight),
                        _pinContent[pinProperty.intValue], EditorStyles.popup))
                {
                    var menu = new GenericMenu();
                    for (int i = 0; i < _pinContent.Length; i++)
                    {
                        int index = i;
                        menu.AddItem(new GUIContent(_pinContent[i]), pinProperty.intValue == i, () =>
                        {
                            pinProperty.intValue = index;
                            pinProperty.serializedObject.ApplyModifiedProperties();
                        });
                    }

                    menu.ShowAsContext();
                }
            }
        }


        private static void DrawAnimation(SerializedProperty animation)
        {
            var boolProperty = animation.FindPropertyRelative(TrackHelper.ENABLE_PROPERTY);

            using (new DisableScope(boolProperty?.boolValue ?? true))
            {
                EditorGUILayout.PropertyField(animation, GUIContent.none);
            }
        }
    }
}