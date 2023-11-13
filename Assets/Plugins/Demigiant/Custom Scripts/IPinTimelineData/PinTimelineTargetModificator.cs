using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Timeline;
using UnityEngine;
namespace Timeline.Extentions.Pin
{
    public class PinTimelineTargetModificator : IPinTimelineModificator
    {
        private int _id;
        private Object _target;

        public PinTimelineTargetModificator(int id, Object target)
        {
            _target = target;
            _id = id;
        }

        public void Modify(DOTweenClip clip)
        {
            clip.PinSetTarget(_id, _target);
        }
    }
}