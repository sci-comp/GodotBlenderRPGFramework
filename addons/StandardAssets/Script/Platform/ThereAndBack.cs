using Godot;
using System;

namespace Game
{
    public partial class ThereAndBack : PathFollow3D, IActivatedPlatform
    {
        [Export] public float RestDuration = 1.0f;
        [Export] public float MoveDuration = 11.0f;

        private bool canBeTriggered = true;
        private bool enabled = false;
        private Tween tween;

        public event Action OnCanBeActivated;

        public bool CanBeActivated() { return canBeTriggered; }
        public bool Enabled() { return enabled; }

        public override void _Ready()
        {
            tween = CreateTween();
            tween.TweenProperty(this, "progress_ratio", 1, MoveDuration);
            tween.TweenInterval(RestDuration);
            tween.TweenProperty(this, "progress_ratio", 0, MoveDuration);
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

