using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SceneTransition")]
public class SceneTransitionStyleSO : ScriptableObject
{
    [Tooltip("The stylyzed wipe/fade/powerpoint transition")]
    public GameObject transitionPrefab;
}
