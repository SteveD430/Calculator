using System;

namespace Calculator.Models
{
    public class StateAction
    {
        public StateAction(Action<char> action, States transitionToState)
        {
            Action = action;
            TransitionToState = transitionToState;
        }
        public States TransitionToState { get; private set; }

        public Action<Char> Action { get; private set; }
    }
}
