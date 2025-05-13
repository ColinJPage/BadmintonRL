using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Springs
{
    [System.Serializable]
    public class Vector3Spring : Spring<Vector3>
    {
        [SerializeField] float maxOffset;

        public Vector3Spring(SpringParameters springParameters, Vector3 startPos, Vector3 startVelocity, float maxOffset = Mathf.Infinity) : base(springParameters, startPos, startVelocity)
        {
            this.maxOffset = maxOffset;
        }

        public override Vector3 Update(float delta, Vector3 targetPos, Vector3 targetVelocity)
        {
            //float k2_stable = Mathf.Max(k2, 1.1f * (delta * delta / 4f + delta * k1 / 2f));
            float k2_stable = Mathf.Max(k2, delta*delta/2f + delta*k1/2f, delta*k1);
            current += delta * velocity;
            velocity += delta * (targetPos + k3 * targetVelocity - current - k1 * velocity) / k2_stable;
            if (clampOffset) ClampOffset(targetPos);
            return current;
        }

        public void SetClampOffset(float maxOffset)
        {
            SetClampOffset(true);
            this.maxOffset = maxOffset;
        }

        private void ClampOffset(Vector3 targetPos)
        {
            var offset = current - targetPos;
            if (offset.magnitude < maxOffset) return;

            var offsetDirection = offset.normalized;

            current = targetPos + offsetDirection * maxOffset;
            var awayVelocity = Vector3.Project(velocity, offsetDirection);
            if (Vector3.Dot(awayVelocity, offsetDirection) > 0f)
            {
                velocity -= awayVelocity;
            }
        }

        protected override Vector3 ExtrapolateTargetVelocity(float delta, Vector3 targetPos)
        {
            return (targetPos - targetPrevious) / delta;
        }
        public override void NudgeVelocity(Vector3 nudge)
        {
            velocity += nudge;
        }
    }
}