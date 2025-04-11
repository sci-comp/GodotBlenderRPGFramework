using Godot;
using System;

namespace Game
{
    /// <summary>
    /// A simple next event timer. 
    /// 
    /// Events are spaced apart using an exponential distribution, defined by t_next_min, t_next_max,
    /// and t_next_average. This timer can either loop or run once. Events can be triggered based on 
    /// the elapsed time, and the class provides a reset function to set the timer back to the initial 
    /// state.
    /// </summary>
    public partial class NextEventTimer : Node
    {
        [Export] public bool alreadyTriggeredEvent = false;
        [Export] public bool loop = false;
        [Export] public float t_next_min = 3.0f;
        [Export] public float t_next_max = 12.0f;
        [Export] public float t_next_average = 5.0f;

        private float u_min = 0.0f;
        private float u_max = 0.0f;
        private float t_current = 0.0f;
        private float t_next = 0.0f;

        public event Action EventTriggered;

        public float Remaining => t_next - t_current;

        public override void _Ready()
        {
            u_min = Mathf.Exp(-t_next_min / t_next_average);
            u_max = Mathf.Exp(-t_next_max / t_next_average);

            t_current = 0.0f;
            t_next = -t_next_average * Mathf.Log(GD.Randf() * (u_max - u_min) + u_min);
            t_next = Mathf.Clamp(t_next, t_next_min, t_next_max);
        }

        public override void _Process(double _delta)
        {
            float delta = (float)_delta;

            if (loop || !alreadyTriggeredEvent)
            {
                if (t_current > t_next)
                {
                    t_current = 0.0f;
                    alreadyTriggeredEvent = true;
                    t_next = -t_next_average * Mathf.Log(GD.Randf() * (u_max - u_min) + u_min);
                    t_next = Mathf.Clamp(t_next, t_next_min, t_next_max);
                    EventTriggered?.Invoke();
                }
                else
                {
                    t_current += delta;
                }
            }
        }

        public void Reset()
        {
            alreadyTriggeredEvent = false;

            t_current = 0.0f;
            t_next = -t_next_average * Mathf.Log(GD.Randf() * (u_max - u_min) + u_min);
            t_next = Mathf.Clamp(t_next, t_next_min, t_next_max);
        }

        public void Halt()
        {
            alreadyTriggeredEvent = true;
            loop = false;
        }

        public void Pause()
        {
            SetProcess(false);
        }

        public void Resume()
        {
            SetProcess(true);
        }

    }

}

