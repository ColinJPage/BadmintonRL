using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Springs
{
    [System.Serializable]
    public class FloatSpring : Spring<float>
    {
        [SerializeField] private Vector2 range = new Vector2(0f, 1f);

        public FloatSpring(SpringParameters springParameters, float startPos, float startVelocity = 0f) : base(springParameters, startPos, startVelocity)
        {

        }

        public override float Update(float delta, float targetPos, float targetVelocity)
        {
            float k2_stable = Mathf.Max(k2, 1.1f * (delta * delta / 4 + delta * k1 / 2));
            current += delta * velocity;
            velocity += delta * (targetPos + k3 * targetVelocity - current - k1 * velocity) / k2_stable;

            if (clampOffset) Clamp(targetPos);

            return current;
        }

        private void Clamp(float targetPos)
        {
            float offset = current - targetPos;
            if (offset < range.x)
            {
                current = targetPos + range.x;
                velocity = Mathf.Max(0f, velocity);
            }
            else if (offset > range.y)
            {
                current = targetPos + range.y;
                velocity = Mathf.Min(0f, velocity);
            }
        }

        protected override float ExtrapolateTargetVelocity(float delta, float targetPos)
        {
            return (targetPos - targetPrevious) / delta;
        }
        public override void NudgeVelocity(float nudge)
        {
            velocity += nudge;
        }
    }
}