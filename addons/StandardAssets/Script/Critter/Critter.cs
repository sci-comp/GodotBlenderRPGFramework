using Godot;
using System;

namespace Game
{
    public partial class Critter : CharacterBody3D
    {
        [Export] public float walkSpeed = 2.0f;
        [Export] public float rotationSpeed = 5.0f;
        [Export] public float randomWalkDurationMin = 3.0f;
        [Export] public float randomWalkDurationMax = 8.0f;
        [Export] public float idleDurationMin = 2.0f;
        [Export] public float idleDurationMax = 4.0f;
        [Export] public float scratchDuration = 1.0f;

        private NextEventTimer eventTimer;
        private StateMachine<State> stateMachine;
        private Vector3 targetDirection;

        private enum State
        {
            Idle,
            Walking,
            Scratching
        }

        public override void _Ready()
        {
            eventTimer = new NextEventTimer();
            AddChild(eventTimer);

            stateMachine = new StateMachine<State>(State.Idle);
            stateMachine.OnStateChange += HandleStateChange;

            eventTimer.EventTriggered += OnEventTriggered;
            ResetEventTimerForState(State.Idle);
        }

        public override void _PhysicsProcess(double delta)
        {
            switch (stateMachine.Current)
            {
                case State.Walking:
                    Velocity = targetDirection * walkSpeed;
                    MoveAndSlide();
                    break;
                case State.Idle:
                    Velocity = Vector3.Zero;
                    break;
                case State.Scratching:
                    Velocity = Vector3.Zero;
                    break;
            }
        }

        private void OnEventTriggered()
        {
            switch (stateMachine.Current)
            {
                case State.Idle:
                    stateMachine.ChangeState(State.Walking);
                    break;
                case State.Walking:
                    stateMachine.ChangeState(State.Scratching);
                    break;
                case State.Scratching:
                    stateMachine.ChangeState(State.Idle);
                    break;
            }
        }

        private void HandleStateChange()
        {
            switch (stateMachine.Current)
            {
                case State.Idle:
                    targetDirection = Vector3.Zero;
                    ResetEventTimerForState(State.Idle);
                    break;
                case State.Walking:
                    targetDirection = GetRandomDirection();
                    ResetEventTimerForState(State.Walking);
                    break;
                case State.Scratching:
                    ResetEventTimerForState(State.Scratching);
                    break;
            }
        }

        private void ResetEventTimerForState(State state)
        {
            switch (state)
            {
                case State.Idle:
                    eventTimer.t_next_min = idleDurationMin;
                    eventTimer.t_next_max = idleDurationMax;
                    break;
                case State.Walking:
                    eventTimer.t_next_min = randomWalkDurationMin;
                    eventTimer.t_next_max = randomWalkDurationMax;
                    break;
                case State.Scratching:
                    eventTimer.t_next_min = scratchDuration;
                    eventTimer.t_next_max = scratchDuration;
                    break;
            }
            eventTimer.Reset();
        }

        private static Vector3 GetRandomDirection()
        {
            float randomAngle = (float)GD.RandRange(0.0f, Mathf.Tau);
            return new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).Normalized();
        }
    }
}
