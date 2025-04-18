using System;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class MoveToTarget2DBezierTransform : TweenAnimation<Transform>, IChangeEndValue<Transform>
    {
        [SerializeField] private Transform _to;
        [SerializeField] private float _centerPoint = 0.5f;
        [SerializeField] private float _offset = 1f;
        [SerializeField] private bool _interactTarget = true;


        protected override Tween GenerateTween()
        {
            return Target
                .DOPath(CalculatePoints(), Duration, PathType.CatmullRom)
                .DOInteractWhenComplete(_to,_interactTarget);
        }

        public void ChangeEndValue(Transform value) => _to = value;

        private Vector3[] CalculatePoints()
        {
            Vector3 pointA = Target.position;
            Vector3 pointB =  _to.position;
            Vector3 vectorBetween = pointB - pointA;
            Vector3 midpointVector = pointA + vectorBetween * _centerPoint;
            Vector3 forwardVector =pointB.x > pointA.x ? Vector3.forward : Vector3.back;
            Vector3 rightVector = Vector3.Cross(vectorBetween, forwardVector).normalized;
		
            Vector3 shiftedPoint = midpointVector + rightVector * _offset;
            
            Vector3[] points = new Vector3[20];
            SimpleBezierPath.GetPath(Target.position, shiftedPoint, _to.position, ref points);
            return points;
        }
    }
}