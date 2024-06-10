using UnityEngine;

public abstract class StateController : PooledMonoBehaviour
{
    [Header("Core variables")]
    public State RemainState;
    public State StartState;

    private State currentState;
    private State previousState;

    private float stateTimeElapsed;
    private bool aiActive;

    public TowerAI TowerAiController { get; protected set; }
    public EnemyCharacter EnemyAiController { get; protected set; }

    protected void SetupAI(bool activate)
    {
        if (activate)
            aiActive = true;
        else aiActive = false;

        TransitionToState(StartState);
    }

    private void Update()
    {
        if (!aiActive)
            return;

        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != RemainState)
        {
            if (currentState != null)
                previousState = currentState;
            else previousState = nextState;

            previousState = currentState;
            currentState = nextState;

            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime * Time.timeScale;
        return stateTimeElapsed >= duration;
    }

    private void OnExitState()
    {
        ResetStateTimer();
    }

    public void ResetStateTimer()
    {
        stateTimeElapsed = 0;
    }

    protected void SetTowerStartingTimers()
    {
        stateTimeElapsed = 5;
    }

    protected void ResetStateMachine()
    {
        SetupAI(false);
    }
}