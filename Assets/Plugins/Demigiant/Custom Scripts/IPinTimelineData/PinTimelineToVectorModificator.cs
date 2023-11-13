using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Timeline;
using UnityEngine;
namespace Timeline.Extentions.Pin
{
    public class PinTimelineToVectorModificator : IPinTimelineModificator
    {
        private int _id;
        private Vector3 _target;

        public PinTimelineToVectorModificator(int id, Vector3 target)
        {
            _target = target;
            _id = id;
        }

        public void Modify(DOTweenClip clip)
        {
            clip.PinsSetToVector(_id, _target);
        }
    }
}
