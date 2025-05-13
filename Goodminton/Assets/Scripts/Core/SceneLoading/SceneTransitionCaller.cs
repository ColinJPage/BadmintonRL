using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionCaller : MonoBehaviour
{
    [SerializeField] SceneTransition transition;

    public void CallTransition()
    {
        SceneLoader.Load(transition);
    }
}
