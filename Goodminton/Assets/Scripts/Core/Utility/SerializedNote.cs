using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedNote : MonoBehaviour
{
    [SerializeField]
    [TextArea(3,5)]
    private string note;
}
