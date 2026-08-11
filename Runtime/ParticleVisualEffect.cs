using System;
using System.Collections.Generic;
using System.Linq;
using Snowraph.Utilities;
using UnityEngine;

namespace Snowraph.VisualEffects
{
    public class ParticleVisualEffect : MonoBehaviour, IVisualEffect
    {
        #region Private Fields
        [SerializeField] private ParticleSystem _particleSystem;

        private List<IVisualEffectModifier> _visualEffectModifiers;
        private Pool<IVisualEffect> _visualEffectPool;

        #endregion

        #region IVisualEffectModifier Properties

        public Transform RootTransform
        {
            get { return transform; }
        }

        #endregion

        #region IVisualEffect Methods

        public void Initialize(Pool<IVisualEffect> pool)
        {
            _visualEffectPool = pool;
        }

        public void Play()
        {
            _particleSystem.Play();
        }

        public void Stop()
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
            Debug.Log("stopped");
            Stop();
            _visualEffectPool.ReturnElement(this);
        }

        #endregion

        #region Internal Methods

        public void CacheVisualEffectModifiers()
        {
            IVisualEffectModifier[] modifiers = GetComponentsInChildren<IVisualEffectModifier>();
            _visualEffectModifiers = modifiers.ToList();
        }

        #endregion
    }
}
