using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snowraph.VisualEffects
{
    public class ParticleVisualEffect : MonoBehaviour, IVisualEffect
    {
        #region Private Fields
        [SerializeField] private ParticleSystem _particleSystem;

        private List<IVisualEffectModifier> _visualEffectModifiers;

        public event Action<IVisualEffect> Stopped;

        #endregion

        #region IVisualEffectModifier Properties

        public Transform RootTransform
        {
            get { return transform; }
        }

        #endregion

        #region IVisualEffect Methods

        public void Play()
        {
            if (_visualEffectModifiers == null)
            {
                IVisualEffectModifier[] modifiers = GetComponentsInChildren<IVisualEffectModifier>();
                _visualEffectModifiers = modifiers.ToList();
            }

            _particleSystem.Play();
        }

        public void Stop()
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Stopped?.Invoke(this);
        }

        public T GetModifiers<T>() where T : Component
        {
            for (int i = 0; i < _visualEffectModifiers.Count; i++)
            {
                if (_visualEffectModifiers[i].GetType() == typeof(T))
                {
                    return (T)_visualEffectModifiers[i];
                }
            }

            return default(T);
        }

        #endregion

        #region Particle Methods

        // This triggers automatically when the particle system stops and all particles die
        private void OnParticleSystemStopped()
        {
            Stop();
        }

        #endregion
    }
}
