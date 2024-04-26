using System;
using ManagedReference;
using ManagedReference.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    public static class SelectObjectDrawer
    {
        private static readonly GUIContent NullLabel = new("Null");

        public static void PropertyField(SerializedProperty property, SerializedProperty managedReferenceProperty,
            GUIContent guiContent)
        {
            EditorGUI.BeginChangeCheck();
            var lastTarget = property.objectReferenceValue;
            EditorGUILayout.PropertyField(property, guiContent);
            if (EditorGUI.EndChangeCheck())
            {
                if (CanReplaceTargets(property.objectReferenceValue, lastTarget, managedReferenceProperty))
                {
                    ReplaceTarget(property, lastTarget);
                }
                else
                {
                    var dropdown =
                        CreateComponentsDropdown(property.objectReferenceValue, lastTarget, property,
                            managedReferenceProperty);
                    dropdown.ShowAsContext();
                }
            }
        }

        private static void ReplaceTarget(
            SerializedProperty property,
            Object lastTarget)
        {
            var target = property.objectReferenceValue;
            if (target is GameObject go)
            {
                property.objectReferenceValue = go.GetComponent(lastTarget.GetType());
            }
        }

        private static bool CanReplaceTargets(
            Object target,
            Object lastTarget,
            SerializedProperty managedReferenceProperty)
        {
            return managedReferenceProperty.arraySize > 0 &&
                   lastTarget != null &&
                   (target.GetType() == lastTarget.GetType() ||
                    target is GameObject go && go.GetComponent(lastTarget.GetType()) != null);
        }

        private static GenericMenu CreateComponentsDropdown(
            Object target,
            Object lastTarget,
            SerializedProperty property,
            SerializedProperty managedReferenceProperty)
        {
            property.objectReferenceValue = lastTarget;
            GenericMenu nodesMenu = new GenericMenu();
            nodesMenu.AddItem(NullLabel, false, OnNUllSelect);
            AddFirstElementIfArray(managedReferenceProperty);
            var managedReferenceType = GetReferenceType(managedReferenceProperty);

            if (target is GameObject gameObject)
            {
                AddManagedValues(gameObject, managedReferenceProperty, nodesMenu, property, managedReferenceType);
                var allComponents = gameObject.GetComponents<Component>();
                foreach (var component in allComponents)
                {
                    AddManagedValues(component, managedReferenceProperty, nodesMenu, property, managedReferenceType);
                }
            }
            else if (target is Component component)
            {
                AddManagedValues(component, managedReferenceProperty, nodesMenu, property, managedReferenceType);
            }

            return nodesMenu;

            void OnNUllSelect()
            {
                property.objectReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private static void AddFirstElementIfArray(SerializedProperty managedReferenceProperty)
        {
            if (managedReferenceProperty.isArray && managedReferenceProperty.arraySize == 0)
            {
                managedReferenceProperty.arraySize = 1;
            }
        }

        static void AddManagedValues(
            Object target,
            SerializedProperty managedReferenceProperty,
            GenericMenu menu,
            SerializedProperty objectProperty,
            Type managedReferenceType)
        {
            var types = ManagedReferenceExtensions.GetTypes(managedReferenceType, target.GetType());
            if (types.Count == 0)
                return;
            menu.AddItem(new GUIContent($"{target.GetType().Name}/None"), false, OnSelect, null);

            foreach (var t in types)
            {
                string name = $"{target.GetType().Name}/{t.GetNameWithCategory()}";
                menu.AddItem(new GUIContent(name), false, OnSelect, t);
            }


            void OnSelect(object managedType)
            {
                if (managedType != null)
                    SetManagedReferenceProperty(managedReferenceProperty, (Type)managedType);
                objectProperty.objectReferenceValue = target;
                objectProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        private static void SetManagedReferenceProperty(SerializedProperty property, Type type)
        {
            if (property.isArray)
            {
                //property.InsertArrayElementAtIndex(property.arraySize == 0 ? 0 : property.arraySize - 1);
                var newItem = property.GetArrayElementAtIndex(property.arraySize - 1);
                newItem.SetManagedReferenceWithCopyValues(type);
                return;
            }

            property.SetManagedReferenceWithCopyValues(type);
        }

        private static Type GetReferenceType(SerializedProperty property) =>
            property.isArray
                ? property.GetArrayElementAtIndex(0).GetManagedReferenceFieldType()
                : property.GetManagedReferenceFieldType();
    }
}