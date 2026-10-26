using System.Collections.Generic;
using Snowraph.GameSystem;
using UnityEngine;
using Snowraph.Utilities;
using System;

namespace Snowraph.VisualEffects
{
    public class VisualEffectSystem : MonoBehaviour, IGameSystem, IVisualEffectSystem
    {
        #region Private Fields

        private Dictionary<VisualEffectData, Pool<IVisualEffect>> _visualEffectPools;
        private Dictionary<IVisualEffect, Pool<IVisualEffect>> _visualEffectHandles;

        #endregion

        #region GameSystem Methods

        public void Initialize()
        {
            _visualEffectPools = new Dictionary<VisualEffectData, Pool<IVisualEffect>>();
            _visualEffectHandles = new Dictionary<IVisualEffect, Pool<IVisualEffect>>();
        }

        #endregion

        #region IVisualEffectSystem Methods

        public void PlayVisualEffect(Vector3 position, VisualEffectData visualEffectData, Action<IVisualEffect> OnVisualEffectStarted = null)
        {
            Pool<IVisualEffect> visualEffectPool;

            if (!_visualEffectPools.TryGetValue(visualEffectData, out visualEffectPool))
            {
                visualEffectPool = CreateVisualEffectPool(visualEffectData);
            }

            IVisualEffect visualEffect = StartVisualEffect(position, visualEffectPool);
            if (visualEffect == null)
            {
                return;
            }

            OnVisualEffectStarted?.Invoke(visualEffect);
        }

        #endregion

        #region Private Methods

        private IVisualEffect StartVisualEffect(Vector3 position, Pool<IVisualEffect> visualEffectPool)
        {
            IVisualEffect visualEffect = visualEffectPool.GetElement();
            if (visualEffect == null)
            {
                return null;
            }

            visualEffect.RootTransform.position = position;
            visualEffect.Play();

            _visualEffectHandles.Add(visualEffect, visualEffectPool);
            visualEffect.Stopped -= OnVisualEffectStopped;
            visualEffect.Stopped += OnVisualEffectStopped;

            return visualEffect;
        }

        private void OnVisualEffectStopped(IVisualEffect visualEffect)
        {
            if (_visualEffectHandles.TryGetValue(visualEffect, out Pool<IVisualEffect> visualEffectPool))
            {
                visualEffectPool.ReturnElement(visualEffect);
                _visualEffectHandles.Remove(visualEffect);
            }

            visualEffect.Stopped -= OnVisualEffectStopped;
        }

        private Pool<IVisualEffect> CreateVisualEffectPool(VisualEffectData visualEffectData)
        {
            Pool<IVisualEffect> pool = new Pool<IVisualEffect>(visualEffectData.BasePoolCount, visualEffectData.CreateVisualEffect);
            _visualEffectPools.Add(visualEffectData, pool);
            return pool;
        }

        #endregion
    }
}
