using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Autoload singleton accessible at /root/PC
    /// This class is a stub, included here for demo purposes only.
    /// </summary>
    public partial class PC : Node
    {
        private CharacterHub charHub;
        private NPC npc;
        private RandomNumberGenerator rng = new();
        private CameraBridge cameraBridge;

        public override void _Ready()
        {
            cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");
            npc = GetNode<NPC>("/root/NPC");

            CharacterHub.Spawned += OnPlayerSpawned;
            CharacterHub.Destroyed += OnPlayerDestroyed;

            GD.PrintRich($"[PC] [color={ColorsHex.MediumSeaGreen}]Ready[/color]");
        }


        public void Camera(string angleName)
        {
            if (Enum.TryParse(angleName, out CameraAngle angle))
            {
                cameraBridge.SetActiveCamera(charHub.CameraAngles, angle);
            }
        }

        private void OnPlayerSpawned(CharacterHub _charHub)
        {
            charHub = _charHub;
        }

        private void OnPlayerDestroyed(CharacterHub _charHub)
        {
            charHub = null;
        }

    }

}

