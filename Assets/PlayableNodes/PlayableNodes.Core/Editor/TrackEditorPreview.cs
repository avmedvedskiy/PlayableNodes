using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.DOTweenEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    public static class TrackEditorPreview
    {
        static TrackEditorPreview()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public static bool IsPreviewing { get; private set; }
        public static string IsPreviewingName { get; private set; }
        private static int PreviewingGroupId { get; set; }

        //Update SetDirty Every tick
        private static readonly List<Graphic> Graphics = new();
        private static CancellationTokenSource _animationTokenSource;

        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (IsPreviewing)
            {
                DOTweenEditorPreview.Stop();
                StopPreview();
            }
        }

        public static async void PreviewAnimation(ITracksPlayer player, string animationName)
        {
            new CancellationTokenSource();
            _animationTokenSource = new CancellationTokenSource();
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName($"Preview animation {animationName}");
            PreviewingGroupId = Undo.GetCurrentGroup();
            IsPreviewing = true;
            IsPreviewingName = animationName;

            //cache all items
            RegisterContextForPreview(player, animationName);

            DOTweenEditorPreview.Start(SetAllGraphicsDirty);
            try
            {
                await player.PlayAsync(animationName, _animationTokenSource.Token);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            StopPreview();
        }

        public static void StopPreviewAnimation()
        {
            _animationTokenSource?.Cancel();
        }

        private static void RegisterContextForPreview(ITracksPlayer player, string animationName)
        {
            foreach (var track in player.Tracks)
            {
                if (track.Name == animationName)
                    foreach (var node in track.Nodes)
                    {
                        if (node.Context != null)
                            RegisterContextForPreview(node.Context,animationName);
                    }
            }
        }

        private static void RegisterContextForPreview(Object context, string animationName)
        {
            switch (context)
            {
                case ITracksPlayer tracksPlayer:
                    RegisterContextForPreview(tracksPlayer,animationName);
                    break;
                case Graphic graphic:
                    Graphics.Add(graphic);
                    break;
            }

            Undo.RegisterFullObjectHierarchyUndo(context, context.name);
        }

        private static void StopPreview()
        {
            DOTweenEditorPreview.Stop();
            if (PreviewingGroupId != 0)
                Undo.RevertAllDownToGroup(PreviewingGroupId);
            //Repaint();
            _animationTokenSource?.Dispose();
            _animationTokenSource = null;

            PreviewingGroupId = 0;
            Graphics.Clear();
            IsPreviewing = false;
            IsPreviewingName = string.Empty;
        }

        private static void SetAllGraphicsDirty()
        {
            foreach (var graphic in Graphics)
            {
                graphic.SetAllDirty();
                EditorApplication.QueuePlayerLoopUpdate();
            }
        }
    }
}