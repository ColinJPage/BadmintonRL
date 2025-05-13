using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Springs
{
    [System.Serializable]
    public abstract class Spring<T> : ISerializationCallbackReceiver
    {
        protected T targetPrevious;
        protected T current, velocity;
        protected float k1, k2, k3;

        [SerializeField] private SpringParameters parameters;
        [SerializeField] protected bool clampOffset = false;

        private void OnValidate()
        {
            SetParameters(parameters);
        }

        public Spring(SpringParameters springParameters, T startPos, T startVelocity)
        {
            SetParameters(springParameters);
            SetState(startPos, startVelocity);
        }
        public void SetState(T position, T velocity)
        {
            SetParameters(parameters);
            targetPrevious = position;
            current = position;
            this.velocity = velocity;
        }

        public void SetParameters(SpringParameters param)
        {
            parameters = param;
            SetParameters(param.frequency, param.damping, param.response);
        }

        private void SetParameters(float frequency, float zeta, float r)
        {
            var pi = Mathf.PI;
            frequency = Mathf.Max(SpringParameters.minFrequency, frequency);
            k1 = zeta / (pi * frequency);
            k2 = 1 / ((2 * pi * frequency) * (2 * pi * frequency));
            k3 = r * zeta / (2 * pi * frequency);
        }

        public T Update(float delta, T targetPos)
        {
            if (delta <= 0f) return current;
            var targetVelocity = ExtrapolateTargetVelocity(delta, targetPos);
            targetPrevious = targetPos;

            return Update(delta, targetPos, targetVelocity);
        }
        protected abstract T ExtrapolateTargetVelocity(float delta, T targetPos);

        public abstract T Update(float delta, T targetPos, T targetVelocity);

        public abstract void NudgeVelocity(T nudge);
        public void SetClampOffset(bool clamp)
        {
            clampOffset = clamp;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() => this.OnValidate();
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }
    }
}