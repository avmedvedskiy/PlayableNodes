using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayableNodes
{
    [ExecuteAlways]
    public class MoveParticlesToTarget : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _system;

        [SerializeField] private Transform _target;

        [SerializeField] [Range(0f, 1f)] private float _elapsedMoveTime;
        [SerializeField] [Range(0f, 1f)] private float _elapsedEndTime;

        [SerializeField] private Easing _x = Easing.Default;
        [SerializeField] private Easing _y = Easing.Default;
        [SerializeField] private Easing _z = Easing.Default;

        private readonly List<uint> _ids = new();
        private readonly List<Tween> _tws = new();
        private readonly List<Vector3> _positions = new();

        private ParticleSystem.Particle[] _particles;
        private ITargetInteract Target { get; set; }

        public ParticleSystem System => _system;

        public void SetTarget(Transform target) => _target = target;


        private void OnEnable()
        {
            Target ??=_target != null
                ? _target.GetComponent<ITargetInteract>() 
                : null;
            if (_system != null && _particles == null || _particles.Length < _system.main.maxParticles)
                _particles = new ParticleSystem.Particle[_system.main.maxParticles];
        }

        private void OnDisable()
        {
            foreach (var tw in _tws)
            {
                tw.Kill();
            }

            CleanUp();
        }

        private void LateUpdate()
        {
            if (_system == null || _target == null)
                return;

            int numParticlesAlive = _system.GetParticles(_particles);

            if (numParticlesAlive == 0)
            {
                CleanUp();
                return;
            }

            Vector3 targetPos = GetTargetPosition();

            for (int i = 0; i < numParticlesAlive; i++)
            {
                var particle = _particles[i];
                var startLifetime = particle.startLifetime;
                var elapsedLifetime = startLifetime - particle.remainingLifetime;
                var normalizedElapsedLifetime = elapsedLifetime / startLifetime;
                if (normalizedElapsedLifetime < _elapsedMoveTime)
                {
                    continue;
                }

                var index = GetIndex(particle.randomSeed);
                if (index == -1)
                {
                    Vector3 offset = Random.onUnitSphere * 0.2f;
                    CreateSequence(particle.randomSeed,
                            particle.position,
                            targetPos + offset * 0.5f,
                            startLifetime - elapsedLifetime - _elapsedEndTime)
                        .PlayOrPreview();
                }
                else
                {
                    _particles[i].position = _positions[index];
                }
            }

            _system.SetParticles(_particles, numParticlesAlive);
        }

        private void CleanUp()
        {
            for (var i = 0; i < _tws.Count; i++)
                _tws[i].Kill();

            _ids.Clear();
            _tws.Clear();
            _positions.Clear();
        }

        private int GetIndex(uint id)
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                if (_ids[i] == id)
                    return i;
            }

            return -1;
        }

        private Tween CreateSequence(uint id,
            Vector3 startPosition,
            Vector3 endPosition,
            float duration)
        {
            var index = _tws.Count;
            var s = DOTween.Sequence()
                .Join(CreateMoveTween(index, endPosition, duration, _x)
                    .SetOptions(AxisConstraint.X))
                .Join(CreateMoveTween(index, endPosition, duration, _y)
                    .SetOptions(AxisConstraint.Y))
                .Join(CreateMoveTween(index, endPosition, duration, _z)
                    .SetOptions(AxisConstraint.Z))
                .OnComplete(OnInteract);

            _positions.Add(startPosition);
            _ids.Add(id);
            _tws.Add(s);
            return s;
        }

        private void OnInteract() => Target?.Interact();

        private TweenerCore<Vector3, Vector3, VectorOptions> CreateMoveTween(int index, Vector3 endPosition,
            float duration,
            Easing easing)
        {
            return DOTween.To(
                    () => _positions[index],
                    pos => _positions[index] = pos,
                    endPosition,
                    duration)
                .SetRecyclable(true)
                .SetEase(easing);
        }

        private Vector3 GetTargetPosition()
        {
            var position = _target.position;
            return _system.main.simulationSpace switch
            {
                ParticleSystemSimulationSpace.Local => _system.transform.InverseTransformPoint(position),
                ParticleSystemSimulationSpace.World => position,
                ParticleSystemSimulationSpace.Custom => _system.main.customSimulationSpace != null
                    ? _system.main.customSimulationSpace.InverseTransformPoint(position)
                    : _system.transform.InverseTransformPoint(position),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnValidate()
        {
            CleanUp();
        }
    }
}