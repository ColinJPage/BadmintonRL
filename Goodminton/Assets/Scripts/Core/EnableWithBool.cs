using UnityEngine;

public class EnableWithBool : MonoBehaviour
{
    [SerializeField] BoolReference boolReference;
    [SerializeField] GameObject toggledObject;
    [SerializeField] GameObject[] toggledObjects;
    [SerializeField] bool invert;
    private void OnEnable()
    {
        boolReference?.Subscribe(OnChange);
        OnChange(boolReference.Value);
    }
    private void OnDisable()
    {
        boolReference?.Unsubscribe(OnChange);
    }
    void OnChange(bool b)
    {
        bool active = invert ? !b : b;
        if(toggledObject != null) toggledObject.SetActive(active);
        foreach(var o in toggledObjects)
        {
            o?.SetActive(active);
        }
    }
}
