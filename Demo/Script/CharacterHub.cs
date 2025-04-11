using Godot;

namespace Game
{
    public partial class CharacterHub : Node
    {
        private CameraBridge cameraBridge;
        private ProximityDetector proximityDetector;
        private CharacterBody3D player;

        public override void _Ready()
        {
            // Autoload
            cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");
            proximityDetector = GetNode<ProximityDetector>("Player/Proximity");
            player = GetNode<CharacterBody3D>("Player");

            proximityDetector.Initialize();

            GD.Print("[CharacterHub] Ready");
        }

        public void SetCharacterPosition(Vector3 position)
        {
            GD.Print("[CharacterHub] Setting character position");
            player.Position = position;
        }

        public void SetCharacterRotation(Vector3 rotation)
        {
            GD.Print("[CharacterHub] Setting character rotation");
            player.Rotation = rotation;
        }

    }

}