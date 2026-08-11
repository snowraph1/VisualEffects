using System;
using Snowraph.GameSystem;
using UnityEngine;

namespace Snowraph.VisualEffects
{
    public interface IVisualEffectSystem : IGameSystem
    {
        public void PlayVisualEffect(Vector3 position, VisualEffectData visualEffectData, Action<IVisualEffect> played);
    }
}
