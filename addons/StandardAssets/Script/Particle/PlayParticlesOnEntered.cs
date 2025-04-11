using Godot;

namespace Game
{
    public partial class PlayParticlesOnEntered : Area3D
    {
        [Export] public string particlesID = "poof";
        [Export] public bool Repeatable = true;
        [Export] public float RestDuration = 13.0f;

        [ExportCategory("Marker")]
        [Export] public bool UseMarker = false;
        [Export] public Marker3D Marker;

        private bool alreadyPlayed = false;
        private Particles particles;
        private Vector3 targetPosition;
        private Timer restTimer;

        public override void _Ready()
        {
            particles = GetNode<Particles>("/root/Particles");
            targetPosition = UseMarker ? Marker.GlobalPosition : GlobalPosition;
            AreaEntered += OnAreaEntered;

            if (Repeatable)
            {
                restTimer = new Timer
                {
                    WaitTime = RestDuration,
                    OneShot = true
                };
                AddChild(restTimer);
                restTimer.Timeout += () => alreadyPlayed = false;
            }
        }

        private void OnAreaEntered(Node3D other)
        {
            if (!alreadyPlayed)
            {
                particles.Play(particlesID, targetPosition);
                alreadyPlayed = true;

                if (Repeatable && restTimer != null)
                {
                    restTimer.Start();
                }
            }
        }

        public override void _ExitTree()
        {
            if (restTimer != null)
            {
                restTimer.Stop();
                restTimer.QueueFree();
            }
        }

    }

}

