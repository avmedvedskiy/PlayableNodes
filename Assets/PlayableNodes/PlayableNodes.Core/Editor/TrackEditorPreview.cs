using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.DOTweenEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableNodes
{
    public static class TrackEditorPreview
    {
        static TrackEditorPreview()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public static bool IsPreviewing { get; private set; }
        private static int PreviewingGroupId { get; set; }
        private static CancellationTokenSource TokenSource { get; set; }

        //Update SetDirty Every tick
        private static readonly List<Graphic> Graphics = new();

        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (IsPreviewing)
                StopPreview();
        }

        public static async void PreviewAnimation(Object player, string animationName, SerializedProperty track)
        {
            var trackDirector = (BaseTrackPlayer)player;
            TokenSource = new CancellationTokenSource();
            ForceEditorUpdate(TokenSource.Token);
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName($"Preview animation {animationName}");
            PreviewingGroupId = Undo.GetCurrentGroup();
            //Undo.RegisterFullObjectHierarchyUndo(player, player.name);
            var nodes = track.FindPropertyRelative(TrackHelper.TRACK_NODES_PROPERTY);
            for (int i = 0; i < nodes.arraySize; i++)
            {
                var context = nodes.GetArrayElementAtIndex(i).FindPropertyRelative(TrackHelper.CONTEXT_PROPERTY);
                if (context.objectReferenceValue != null)
                {
                    Undo.RegisterFullObjectHierarchyUndo(context.objectReferenceValue,
                        context.objectReferenceValue.name);
                    if (context.objectReferenceValue is Graphic graphic)
                        Graphics.Add(graphic);
                }
            }

            IsPreviewing = true;
            DOTweenEditorPreview.Start();
            await UniTask.WhenAny(
                trackDirector.PlayAsync(animationName),
                UniTask.WaitForSeconds(trackDirector.TotalDuration(animationName) + 1f)); //for any exceptions in editor
            StopPreview();
        }

        private static void StopPreview()
        {
            DOTweenEditorPreview.Stop();
            IsPreviewing = false;
            if (PreviewingGroupId != 0)
                Undo.RevertAllDownToGroup(PreviewingGroupId);
            //Repaint();
            PreviewingGroupId = 0;
            TokenSource?.Cancel();
            TokenSource?.Dispose();
            TokenSource = null;
            Graphics.Clear();
        }

        private static async void ForceEditorUpdate(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield();
                foreach (var graphic in Graphics)
                {
                    graphic.SetAllDirty();
                }

                EditorApplication.QueuePlayerLoopUpdate();
            }
        }
    }
}