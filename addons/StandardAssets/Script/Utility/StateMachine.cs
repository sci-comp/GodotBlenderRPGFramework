using System;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// A basic state machine that holds and manages states of type T. 
    /// 
    /// Invokes an event whenever a state change occurs and provides properties for 
    /// accessing the current and previous states.
    /// 
    /// </summary>
    public class StateMachine<T>
    {
        public T Current { get; private set; }
        public T Previous { get; private set; }

        public event Action OnStateChange;

        public StateMachine(T initialState)
        {
            Current = initialState;
            Previous = initialState;
        }

        public void ChangeState(T newState)
        {
            if (EqualityComparer<T>.Default.Equals(newState, Current))
            {
                return;
            }

            Previous = Current;
            Current = newState;
            OnStateChange?.Invoke();
        }

    }

}

