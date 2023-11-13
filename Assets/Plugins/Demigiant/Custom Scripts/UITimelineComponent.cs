using DG.Tweening;
using DG.Tweening.Timeline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Timeline.Extentions
{
    using Pin;

    public class UITimelineComponent : MonoBehaviour
    {
        public event TweenCallback OnComplete;

        [SerializeField] private DOTweenClipCollection _clipCollection;
        [SerializeField] private string _currentAnimation;
        private string _nextAnimation;
        private List<DOTweenClip> _tweenClips = new List<DOTweenClip>();
        private Coroutine _coroutine;

        private IPinTimelineModificator _pinData;

        public void PlayAnimation(string animation)
        {
            _nextAnimation = animation;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(WaitEndOfFrameAndPlayAnimation());
        }

        public void PlayAnimation(string animation, IPinTimelineModificator pinData)
        {
            _pinData = pinData;

            _nextAnimation = animation;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(WaitEndOfFrameAndPlayAnimation());
        }

        /// <summary>
        /// PlayAnimation without any current and next animations
        /// </summary>
        /// <param name="animation"></param>
        public void PlayAnimationImmediately(string animation)
        {
            PlayCustomAnimation(animation);
        }

        /// <summary>
        /// PlayAnimation without any current and next animations
        /// </summary>
        /// <param name="animation"></param>
        public void PlayAnimationImmediately(string animation, IPinTimelineModificator pinData)
        {
            _pinData = pinData;
            PlayCustomAnimation(animation);
        }

        //Не очень гибкое решение для правки анимаций. 
        //В один кадр происходит несколько изменений состояния, поэтому ждем окончания кадра и проигрываем одно состояние
        private IEnumerator WaitEndOfFrameAndPlayAnimation()
        {
            yield return new WaitForEndOfFrame();

            if (_nextAnimation != _currentAnimation)
            {
                PlayCustomAnimation(_nextAnimation);
                _currentAnimation = _nextAnimation;
            }

            _coroutine = null;
        }

        private void PlayCustomAnimation(string id)
        {
            if (_clipCollection == null)
                return;

            foreach (var tw in _tweenClips)
            {
                if (tw.IsTweenPlaying())
                    tw.KillTween();
            }

            _tweenClips.Clear();
            _clipCollection.GetClipsByName(id, ref _tweenClips);
            foreach (var x in _tweenClips)
            {
                if (_pinData != null)
                    _pinData.Modify(x);

                x.Play();
            }

            if (_tweenClips.Count > 0)
                _tweenClips[0].tween.onComplete += OnComplete;
        }
    }
}