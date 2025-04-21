#if UNITY_EDITOR
using DG.DOTweenEditor;
#endif
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    public static class DOTweenExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PlayOrPreview<T>(this T tween) where T : Tween
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DOTweenEditorPreview.PrepareTweenForPreview(tween, false);
                DOTweenEditorPreview.Start();
            }
#endif

            return tween.Play();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RestartOrPreview<T>(this T tween) where T : Tween
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DOTweenEditorPreview.PrepareTweenForPreview(tween, false);
                DOTweenEditorPreview.Start();
            }
#endif
            tween.Restart();
            return tween;
        }

        public static Tweener DOMove(this Transform transform, MoveSpace space, ToFromValue<Vector3> to, float duration)
        {
            return space == MoveSpace.Global
                ? transform.DOMove(to.ConvertValue(transform.position), duration)
                : transform.DOLocalMove(to.ConvertValue(transform.localPosition), duration);
        }

        public static Tweener DORotate(this Transform transform, MoveSpace space, ToFromValue<Vector3> to,
            float duration,
            RotateMode mode = RotateMode.Fast)
        {
            return space == MoveSpace.Global
                ? transform.DORotate(to.ConvertValue(transform.position), duration, mode)
                : transform.DOLocalRotate(to.ConvertValue(transform.localPosition), duration, mode);
        }

        public static Tweener DORotateQuaternion(this Transform transform, MoveSpace space, Transform to,
            float duration)
        {
            return space == MoveSpace.Global
                ? transform.DORotateQuaternion(to.rotation, duration)
                : transform.DOLocalRotateQuaternion(to.localRotation, duration);
        }

        public static Sequence DOMoveConstraint(this Transform transform,
            ToFromValue<Vector3> from,
            ToFromValue<Vector3> to,
            Easing x,
            Easing y,
            Easing z,
            float duration,
            bool recyclable = true)
        {
            var currentPosition = transform.position;
            var toPosition = to.ConvertValue(currentPosition);
            var fromPosition = from.ConvertValue(currentPosition);
            return DOMoveConstraint(transform, fromPosition, toPosition, x, y, z, duration, recyclable);
        }

        public static Sequence DOMoveConstraint(this Transform transform,
            Vector3 from,
            Vector3 to,
            Easing x,
            Easing y,
            Easing z,
            float duration,
            bool recyclable = true)
        {
            return DOTween.Sequence()
                .Join(transform
                    .DOMoveX(to.x, duration)
                    .SetEase(x)
                    .ChangeStartValue(from)
                    .SetRecyclable(recyclable))
                .Join(transform
                    .DOMoveY(to.y, duration)
                    .SetEase(y)
                    .ChangeStartValue(from)
                    .SetRecyclable(recyclable))
                .Join(transform
                    .DOMoveZ(to.z, duration)
                    .SetEase(z)
                    .ChangeStartValue(from)
                    .SetRecyclable(recyclable))
                .SetRecyclable(recyclable);
        }

        public static Tween DOInteractWhenComplete(this Tween tweener, Transform target, bool active)
        {
            if (active && target.TryGetComponent<ITargetInteract>(out var interact))
            {
                tweener.OnComplete(interact.Interact);
            }

            return tweener;
        }


        public static Tweener DOFollowTarget(this Transform transform, Transform target, float duration)
        {
            var endPosition = target.position;
            var t = transform.DOMove(endPosition, duration);
            return t.OnUpdate(OnUpdate);

            void OnUpdate()
            {
                var position = target.position;
                if (position == endPosition)
                    return;
                endPosition = position;

                var newDuration = t.Duration() - Time.deltaTime;
                t.ChangeValuesVector(position, transform.position, newDuration);
                if (newDuration < 0f)
                {
                    t.OnUpdate(null);
                    t.Complete(true);
                }
            }
        }

        public static TweenerCore<Vector3, Path, PathOptions> SetLookAtPath(
            this TweenerCore<Vector3, Path, PathOptions> t, bool value)
        {
            if (value)
                t.SetLookAt(0.1f);
            return t;
        }

        public static TweenerCore<Vector3, Path, PathOptions> DOPath(
            this Transform target,
            Vector3[] path,
            float duration,
            MoveSpace space = MoveSpace.Global,
            PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D,
            int resolution = 10,
            Color? gizmoColor = null)
        {
            return space == MoveSpace.Global
                ? target.DOPath(path, duration, pathType, pathMode, resolution, gizmoColor)
                : target.DOLocalPath(path, duration, pathType, pathMode, resolution, gizmoColor);
        }

        public static TweenerCore<float, float, FloatOptions> DOEvaluateScaleByCurve(
            this Transform transform, 
            AnimationCurve curve, 
            float duration)
        {
            float timer = 0f;
            var initialValue = transform.localScale;
            return DOTween.To(() => timer, x => timer = x, 1f, duration)
                .OnStart(()=> initialValue = transform.localScale)
                .OnUpdate(()=>transform.localScale = initialValue * curve.Evaluate(timer));;
        }
        
        public static TweenerCore<float, float, FloatOptions> DOEvaluateScaleByCurve(
            this Transform transform, 
            AnimationCurve x, 
            AnimationCurve y, 
            AnimationCurve z, 
            float duration)
        {
            float timer = 0f;
            var initialValue = transform.localScale;
            return DOTween.To(() => timer, x => timer = x, 1f, duration)
                .OnStart(()=> initialValue = transform.localScale)
                .OnUpdate(()=>transform.localScale = new Vector3(
                    initialValue.x * x.Evaluate(timer), 
                    initialValue.y * y.Evaluate(timer), 
                    initialValue.z * z.Evaluate(timer)));;
        }
    }
}