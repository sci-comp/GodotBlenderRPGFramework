using Godot;

namespace Game
{
    public partial class PeriodicPlatform : AnimatableBody3D
    {
        [Export] float restDuration = 3.0f;
        [Export] float moveDuration = 7.0f;
        [Export] Vector3 moveDistance = new(0, 1, 0);

        private Vector3 initialPos;
        private Vector3 dest;
        private Tween tween;

        public override void _Ready()
        {
            initialPos = Position;
            dest = initialPos + moveDistance;

            tween = CreateTween();

            tween.TweenProperty(this, "position", dest, moveDuration);

            if (!Mathf.IsZeroApprox(restDuration))
            {
                tween.TweenInterval(restDuration);
            }

            tween.TweenProperty(this, "position", initialPos, moveDuration);

            if (!Mathf.IsZeroApprox(restDuration))
            {
                tween.TweenInterval(restDuration);
            }

            tween.SetLoops(0);
            tween.Play();
        }

    }

}

