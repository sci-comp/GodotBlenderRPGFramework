using Godot;
using System;

namespace Game
{
    public partial class SimpleThereAndBack : AnimatableBody3D, IActivatedPlatform
    {
        [Export] public float RestDuration = 2.0f;
        [Export] public float MoveDuration = 7.0f;
        [Export] public Vector3 MoveDistance = new(0, 1, 0);

        private bool canBeTriggered = true;
        private bool enabled = false;
        private Vector3 initialPos;
        private Tween tween;

        public event Action OnCanBeActivated;

        public bool CanBeActivated() { return canBeTriggered; }
        public bool Enabled() { return enabled; }

        public override void _Ready()
        {
            initialPos = Position;
            tween = CreateTween();
            tween.TweenProperty(this, "position", initialPos + MoveDistance, MoveDuration);
            tween.TweenInterval(RestDuration);
            tween.TweenProperty(this, "position", initialPos, MoveDuration);
            //tween.TweenInterval(RestDuration);
            tween.SetLoops();
            tween.LoopFinished += OnIdle;
            tween.Pause();
        }

        public void Disable()
        {
            tween.Play();
        }

        public void Enable()
        {
            tween.Pause();
        }

        public void Activate()
        {
            if (!canBeTriggered)
            {
                return;
            }
            canBeTriggered = false;
            tween.Play();
        }

        private void OnIdle(long _loopCount)
        {
            tween.Pause();
            canBeTriggered = true;
            OnCanBeActivated?.Invoke();
        }
    }
}
