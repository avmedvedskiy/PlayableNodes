using UnityEditor;
using UnityEngine;

namespace PlayableNodes
{
    public static class TrackListDrawer
    {
        public static void DrawHeaderAndTracks(SerializedProperty tracksProperty, Object player)
        {
            DrawTrackHeader(tracksProperty);
            DrawTracks(tracksProperty, player);
        }
        
        public static void DrawTrackHeader(SerializedProperty tracksProperty)
        {
            if (GUILayout.Button("+ Add New Track"))
            {
                AddNewTrack(tracksProperty);
            }
        }

        private static void AddNewTrack(SerializedProperty tracksProperty)
        {
            tracksProperty.InsertArrayElementAtIndex(tracksProperty.arraySize == 0 ? 0 : tracksProperty.arraySize - 1);
            var newTrack = tracksProperty.GetArrayElementAtIndex(tracksProperty.arraySize - 1);
            TrackDrawer.ClearTrack(newTrack);
            GUI.changed = true;
        }

        public static void DrawTracks(SerializedProperty tracksProperty, Object player)
        {
            for (int i = 0; i < tracksProperty.arraySize; i++)
            {
                var track = tracksProperty.GetArrayElementAtIndex(i);
                TrackDrawer.DrawTrack(track, player);
                EditorGUILayout.Separator();
            }
        }
        
        public static void DrawTrack(SerializedProperty trackProperty, Object player)
        {
            TrackDrawer.DrawTrack(trackProperty, player);
        }
    }
}