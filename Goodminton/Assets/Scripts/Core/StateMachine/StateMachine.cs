using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<SM, S> : IStateMachine<SM, S>
    where SM : IStateMachine<SM, S>
    where S : IState<SM, S>
{
    protected S state;
    public S State
    {
        get => state;
        set => state = value;
    }
    public StateMachine(S startingState)
    {
        State = startingState;
    }

    public void SetState(S state)
    {
        //base(state);
        ((IStateMachine<SM, S>)this).SetStateInternal(state);
        //base(IStateMachine<SM, S>).SetState(state);
    }
}
