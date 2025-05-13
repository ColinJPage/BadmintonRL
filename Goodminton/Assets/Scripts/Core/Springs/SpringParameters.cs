using UnityEngine;

namespace Springs
{
    [System.Serializable]
    public struct SpringParameters
    {
        public const float minFrequency = 0.01f;

        [Min(minFrequency)]
        public float frequency;
        public float damping, response;
        public SpringParameters(float frequency, float damping, float response)
        {
            this.frequency = Mathf.Max(frequency, minFrequency);
            this.damping = damping;
            this.response = response;
        }
    }
}