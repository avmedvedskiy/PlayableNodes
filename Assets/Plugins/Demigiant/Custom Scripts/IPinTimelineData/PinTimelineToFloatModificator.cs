using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Timeline;
using UnityEngine;
namespace Timeline.Extentions.Pin
{
    public class PinTimelineToFloatModificator : IPinTimelineModificator
    {
        private int _id;
        private float _target;
        public PinTimelineToFloatModificator(int id, float target)
        {
            _target = target;
            _id = id;
        }

        public void Modify(DOTweenClip clip)
        {
            clip.PinSetToFloatVal(_id, _target);
        }
    }
}