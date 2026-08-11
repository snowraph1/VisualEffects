using System.Collections;
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

        #endregion

        #region GameSystem Methods

        public void Initialize()
        {
            _visualEffectPools = new Dictionary<VisualEffectData, Pool<IVisualEffect>>();
            GameSystems.Instance.Register(this);
        }

        #endregion

        #region IVisualEffectSystem Methods

        public void PlayVisualEffect(Vector3 position, VisualEffectData visualEffectData, Action<IVisualEffect> OnVisualEffectStarted)
        {
            Pool<IVisualEffect> visualEffectPool;

            if (!_visualEffectPools.TryGetValue(visualEffectData, out visualEffectPool))
            {
                visualEffectPool = CreateVisualEffectPool(visualEffectData);
            }

            IVisualEffect visualEffect = StartVisualEffect(position, visualEffectPool);
            OnVisualEffectStarted?.Invoke(visualEffect);
        }

        #endregion

        #region Private Methods

        private IVisualEffect StartVisualEffect(Vector3 position, Pool<IVisualEffect> visualEffectPool)
        {
            IVisualEffect visualEffect = visualEffectPool.GetElement();
            visualEffect.RootTransform.position = position;
            visualEffect.Play();
            visualEffect.Initialize(visualEffectPool);

            return visualEffect;
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
