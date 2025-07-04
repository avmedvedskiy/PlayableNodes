using System;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Moves a Transform to a target along a 3D Bezier path with optional look-at")]
    public class MoveToTarget3DBezierTransform : TweenAnimation<Transform>, IChangeEndValue<Transform>,
        IDrawGizmosSelected
    {
        [SerializeField] private MoveSpace _space = MoveSpace.Global;
        [SerializeField] private Transform _to;
        [Range(-5f, 5f)] [SerializeField] private float _centerPointA = 0.5f;
        [SerializeField] private float _offsetA = 1f;

        [Range(-5f, 5f)] [SerializeField] private float _centerPointB = 1f;
        [SerializeField] private float _offsetB = 1f;

        [SerializeField] private Vector3 _spaceDirection = Vector3.up;
        [SerializeField] private bool _withLookAt = false;
        [SerializeField] private bool _interactTarget = true;


        protected override Tween GenerateTween()
        {
            return Target
                .DOPath(CalculatePoints(), Duration, _space, PathType.CatmullRom)
                .SetLookAtPath(_withLookAt)
                .DOInteractWhenComplete(_to,_interactTarget);
        }

        public void ChangeEndValue(Transform value) => _to = value;

        private Vector3[] CalculatePoints()
        {
            Vector3 pointA, pointB;
            pointA = _space == MoveSpace.Global
                ? Target.position 
                : Target.localPosition;
            pointB = _space == MoveSpace.Global
                ? _to.position
                : Target.parent != null
                    ? Target.parent.InverseTransformPoint(_to.position)
                    : Target.localPosition;

            Vector3 vectorBetween = pointB - pointA;
            Vector3 midpointVectorA = pointA + vectorBetween * _centerPointA + _spaceDirection * _offsetA;
            Vector3 midpointVectorB = pointA + vectorBetween * _centerPointB + _spaceDirection * _offsetB;

            return DOCurve.CubicBezier.GetSegmentPointCloud(
                pointA,
                midpointVectorA,
                pointB,
                midpointVectorB,
                20);
        }

        void IDrawGizmosSelected.OnDrawGizmosSelected()
        {
            if (_to != null && Target != null)
                SimpleBezierPath.DebugDrawPath(CalculatePoints());
        }
    }
}