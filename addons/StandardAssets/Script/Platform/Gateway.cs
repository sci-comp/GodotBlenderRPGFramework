using Godot;

namespace Game
{
    public partial class Gateway : Area3D
    {
        public Marker3D Spawnpoint;
        private Gateway otherGateway;

        public bool WaitingOnPlayerToExitPlatform = false;

        public void Initialize(Gateway _otherGateway)
        {
            Spawnpoint = GetNode<Marker3D>("Spawnpoint");
            otherGateway = _otherGateway;

            BodyExited += OnBodyExit;
        }

        public void ActivateGateway(CharacterBody3D character)
        {
            if (otherGateway != null && character != null)
            {
                otherGateway.WaitingOnPlayerToExitPlatform = true;
                character.GlobalPosition = otherGateway.Spawnpoint.GlobalPosition;
            }
        }

        private void OnBodyExit(Node3D body)
        {
            WaitingOnPlayerToExitPlatform = false;
        }

    }

}

