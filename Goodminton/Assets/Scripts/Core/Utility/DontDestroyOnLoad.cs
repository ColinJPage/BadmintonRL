using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] bool unparentOnAwake = false;
    private void Awake()
    {
        if (unparentOnAwake)
        {
            transform.SetParent(null);
        }
        DontDestroyOnLoad(gameObject);
    }
}
