using Godot;
using System;

namespace Game
{
    /// <summary>
    /// This is a placeholder script demonstrating interaction with StandardAssets
    /// </summary>
    public partial class CharacterHub : Node
    {
        private CameraBridge cameraBridge;
        private ProximityDetector proximityDetector;
        private CharacterBody3D player;

        public static event Action<CharacterHub> Spawned;
        public static event Action<CharacterHub> Destroyed;

        public override void _Ready()
        {
            // Autoload
            cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");
            proximityDetector = GetNode<ProximityDetector>("Player/Proximity");
            player = GetNode<CharacterBody3D>("Player");

            proximityDetector.Initialize();

            Spawned?.Invoke(this);

            GD.Print("[CharacterHub] Ready");
        }

        public override void _ExitTree()
        {
            Destroyed?.Invoke(this);
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