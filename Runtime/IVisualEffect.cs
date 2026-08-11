using UnityEngine;
using System;
using Snowraph.Utilities;

namespace Snowraph.VisualEffects
{
    public interface IVisualEffect
    {
        public Transform RootTransform { get; }

        public void Initialize(Pool<IVisualEffect> pool);

        public void Play();

        public void Stop();

        public void CacheVisualEffectModifiers();

        T GetModifiers<T>() where T : Component;
    }
}
