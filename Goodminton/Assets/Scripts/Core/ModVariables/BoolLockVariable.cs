using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolLock : Modifier<bool, bool>
{
    public bool locked = true;
    public void SetLocked(bool locked)
    {
        this.locked = locked;
        JustChanged();
    }
    public override bool Modify(ref bool value, in bool mode)
    {
        if (locked)
        {
            return true;
        }
        else
        {
            return value;
        }
    }
}
public class BoolLockVariable : ModVariable<bool, BoolLock>
{
    public bool IsLocked()
    {
        return GetValue();
    }
    protected override bool BaseValue()
    {
        return false;
    }
}
