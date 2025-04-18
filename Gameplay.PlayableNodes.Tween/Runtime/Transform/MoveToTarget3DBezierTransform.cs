using System;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class MoveToTarget3DBezierTransform : TweenAnimation<Transform>, IChangeEndValue<Transform>,
        IDrawGizmosSelected
    {
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
                .DOPath(CalculatePoints(), Duration, PathType.CatmullRom)
                .SetLookAtPath(_withLookAt)
                .DOInteractWhenComplete(_to,_interactTarget);
        }

        public void ChangeEndValue(Transform value) => _to = value;

        private Vector3[] CalculatePoints()
        {
            Vector3 pointA = Target.position;
            Vector3 pointB = _to.position;

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