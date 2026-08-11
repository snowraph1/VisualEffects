using UnityEngine;
using System;
using Snowraph.Utilities;

namespace Snowraph.VisualEffects
{
    public interface IVisualEffect
    {
        public Transform RootTransform { get; }

        public event Action<IVisualEffect> Stopped;

        public void Play();

        public void Stop();

        T GetModifiers<T>() where T : Component;
    }
}
