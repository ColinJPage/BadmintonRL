using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolOnStart : MonoBehaviour
{
    [SerializeField] BoolReference boolRef;
    [SerializeField] bool targetValue = true;

    private void Start()
    {
        boolRef.Value = targetValue;
    }
}
