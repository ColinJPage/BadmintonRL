using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<SM, S> : IState<SM, S>
    where SM : IStateMachine<SM, S>
    where S : IState<SM, S>
{
    protected SM stateMachine;

    public SM StateMachine { 
        get => stateMachine; 
        set => stateMachine = value; 
    }

    public State(SM machine)
    {
        StateMachine = machine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }

    public bool Equals(S other)
    {
        return false;
    }
}
