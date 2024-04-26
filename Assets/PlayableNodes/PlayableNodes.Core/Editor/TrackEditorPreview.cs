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
            {
                DOTweenEditorPreview.Stop();
                StopPreview();
            }
        }

        public static async void PreviewAnimation(ITracksPlayer player, string animationName)
        {
            TokenSource = new CancellationTokenSource();
            ForceEditorUpdate(TokenSource.Token);
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName($"Preview animation {animationName}");
            PreviewingGroupId = Undo.GetCurrentGroup();
            IsPreviewing = true;
            
            //cache all items
            RegisterContextForPreview(player);
            
            DOTweenEditorPreview.Start();
            await UniTask.WhenAny(
                player.PlayAsync(animationName),
                UniTask.WaitForSeconds(player.TotalDuration(animationName) + 1f)); //for any exceptions in editor
            StopPreview();
            IsPreviewing = false;
        }
        
        private static void RegisterContextForPreview(ITracksPlayer player)
        {
            foreach (var track in player.Tracks)
            {
                foreach (var node in track.Nodes)
                {
                    if(node.Context != null)
                        RegisterContextForPreview(node.Context);
                }
            }
        }
        
        private static void RegisterContextForPreview(Object context)
        {
            switch (context)
            {
                case ITracksPlayer tracksPlayer:
                    RegisterContextForPreview(tracksPlayer);
                    break;
                case Graphic graphic:
                    Graphics.Add(graphic);
                    break;
            }
            Undo.RegisterFullObjectHierarchyUndo(context, context.name);
        }

        private static void StopPreview()
        {
            //DOTweenEditorPreview.Stop();
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