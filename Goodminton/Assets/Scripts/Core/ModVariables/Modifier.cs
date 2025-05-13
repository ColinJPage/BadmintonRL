using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier<Data, Mode>
{
    public Event ChangedEvent = new Event();
    public abstract Data Modify(ref Data data, in Mode mode);

    protected void JustChanged()
    {
        ChangedEvent.Trigger();
    }
    public virtual bool RemoveMe => false; // when true, this modifier will be removed from mod variables
}

public abstract class Modifier<Data> : Modifier<Data, bool> { }