using DG.Tweening.Timeline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timeline.Extentions.Pin
{
    public interface IPinTimelineModificator
    {
        void Modify(DOTweenClip clip);
    }
}
