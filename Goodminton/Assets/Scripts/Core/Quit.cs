using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Application.Quit();

        }
    }
}
