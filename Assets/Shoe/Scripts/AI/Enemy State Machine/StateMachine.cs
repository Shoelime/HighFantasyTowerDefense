using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public object Owner { get; set; }
    public State CurrentState { get; private set; }
    public State RemainState { get; set; }
    public float StateTimeElapsed { get; private set; }

    private State previousState;

    public bool IsActive { get; private set; }

    public void Init(State startState)
    {
        IsActive = true;
        TransitionToState(startState);
    }

    public void Stop()
    {
        ResetStateTimer();
        IsActive = false;
    }

    public void Update()
    {
        if (!IsActive || CurrentState == null) 
            return;

        CurrentState.UpdateState(this);
    }

    public void SetTimer(float timer)
    {
        StateTimeElapsed = timer;
    }
    public void ResetStateTimer()
    {
        StateTimeElapsed = 0;
    }

    public void TransitionToState(State nextState)
    {
        if (nextState == RemainState) return;

        previousState = CurrentState;
        CurrentState = nextState;
        StateTimeElapsed = 0f;
    }
    public bool CheckIfCountDownElapsed(float duration)
    {
        StateTimeElapsed += Time.deltaTime * Time.timeScale;
        return StateTimeElapsed >= duration;
    }
}
