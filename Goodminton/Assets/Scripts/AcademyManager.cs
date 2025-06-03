using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AcademyManager : MonoBehaviour
{
    private void OnDestroy()
    {
        Academy.Instance.Dispose();
    }
}
