using UnityEngine;
using TMPro;

public class VariableDisplayUI<T> : MonoBehaviour
{
    [SerializeField] string prefix;
    [SerializeField] string postfix;
    [SerializeField] VariableReference<T> variable;
    TMP_Text text;
    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
    private void OnEnable()
    {
        variable.Subscribe(OnValueChange);
    }
    private void OnDisable()
    {
        variable.Unsubscribe(OnValueChange);
    }
    protected virtual T GetValue()
    {
        return variable.Value;
    }
    protected virtual string GetString()
    {
        return GetValue().ToString();
    }
    void OnValueChange(T value)
    {
        text?.SetText($"{prefix}{GetString()}{postfix}");
    }
}
