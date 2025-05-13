using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModVariable<Data, TModifier> : ModVariable<Data, TModifier, bool> where TModifier : Modifier<Data, bool> { } // Mode defaults to bool

[System.Serializable]
public class ModVariable<Data, TModifier, Mode> : EventBroadcaster<Data>
    where TModifier : Modifier<Data, Mode>
{
    //class Modifier : NewModifier<T, Mode, Data> { }
    //using Modifier = NewModifier<T, Mode, Data>;

    //Modifier mod;
    List<TModifier> modifiers = new List<TModifier>();
    //List<NewModifier<T, Mode, Data>> modifiers;
    //List<int> intList;

    public Event<Data> OnChangeEvent = new Event<Data>();

    public void AddModifier(TModifier modifier)
    {
        modifiers.Add(modifier);
        modifier.ChangedEvent.Subscribe(OnChange);
        OnChange();
    }
    public void RemoveModifier(TModifier modifier)
    {
        modifiers.Remove(modifier);
        modifier.ChangedEvent.Unsubscribe(OnChange);
        OnChange();
    }
    void OnChange()
    {
        OnChangeEvent.Trigger(GetValue());
    }

    public void RemoveAllModifiers()
    {
        modifiers.Clear();
    }

    public Data GetValue()
    {
        return GetValue(DefaultMode());
    }

    public Data GetValue(Mode mode)
    {
        return CalculateValue(mode);
    }

    protected Data CalculateValue(Mode mode)
    {
        Data value = BaseValue();
        for (int i = 0; i < modifiers.Count; i++)
        {
            value = modifiers[i].Modify(ref value, in mode);
        }
        PruneModifiers();
        return value;
    }

    private void PruneModifiers()
    {
        for (int i = modifiers.Count - 1; i >= 0; --i)
        {
            if (modifiers[i].RemoveMe)
                modifiers.RemoveAt(i);
        }
    }

    protected virtual Data BaseValue()
    {
        return default(Data);
    }

    protected Mode DefaultMode()
    {
        return default(Mode);
    }
}
