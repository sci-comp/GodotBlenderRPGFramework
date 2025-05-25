using Godot;
using System;

namespace Game
{
    /// <summary>
    /// This class is a stub, included here for demo purposes only.
    /// </summary>
    public partial class CharacterHub : Node
    {
        [Export] public CameraAngles CameraAngles;
        [Export] public Node3D LookAt;

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
            DialogueActor.DialogueStarted += OnDialogueStarted;

            Spawned?.Invoke(this);

            GD.Print("[CharacterHub] Ready");
        }

        public override void _ExitTree()
        {
            DialogueActor.DialogueStarted -= OnDialogueStarted;
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

        public void OnDialogueStarted(Vector3 position, float yaw)
        {
            SetCharacterPosition(position);
            SetCharacterRotation(new(0, yaw, 0));
            cameraBridge.Blink();
        }

    }

}