using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] SceneTransitionStyleSO transitionStyle;
    public void Reload()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        SceneLoader.Load(currentScene, transitionStyle);
    }
}
