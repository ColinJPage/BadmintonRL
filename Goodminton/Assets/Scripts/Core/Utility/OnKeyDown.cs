using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnKeyDown : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] UnityEvent keyDownEvent;

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            keyDownEvent?.Invoke();
        }
    }
}
