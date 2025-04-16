using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AIstate")]
public class State : ScriptableObject
{
    [SerializeField] private StateAction[] actions;
    [SerializeField] private Transition[] transitions;

    public void UpdateState(StateMachine controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateMachine controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(StateMachine controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if (decisionSucceeded)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}