using UnityEngine;

public class IntVariableDisplayUI : VariableDisplayUI<int> {
    [SerializeField] int offset;
    protected override int GetValue()
    {
        return base.GetValue() + offset;
    }
}
