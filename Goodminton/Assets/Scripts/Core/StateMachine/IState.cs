using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<SM, S> 
    where SM : IStateMachine<SM, S>
    where S : IState<SM, S>
{
    public SM StateMachine { get; set; }

    public virtual void Enter() { }
    public virtual void Exit() { }


    public void SetStateInternal(S state)
    {
        StateMachine.SetStateInternal(state);
    }

    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }

    bool Equals(S other);
}
