using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine<SM, S> 
    where SM : IStateMachine<SM, S>
    where S : IState<SM, S>
{
    public S State { get; set; }

    public void SetState(S state);
    void SetStateInternal(S newState)
    {
        if (State != null && State.Equals(newState)) return; //Prevent transitions to the current state

        State?.Exit();
        State = newState;
        State?.Enter();
    }
}
