using Godot;

namespace Game
{
    /// <summary>
    /// Autoload singleton accessible at /root/CameraBridge
    /// 
    /// This is a placeholder script demonstrating interaction with StandardAssets
    /// 
    /// </summary>
    public partial class CameraBridge : Node
    {
        public Camera3D MainCamera;

        // Autoload
        private PlayerSpawner playerSpawner;
        private Preferences prefs;

        private CharacterHub characterHub;

        public Vector3 CameraPosition => MainCamera.Position;

        private float blink_delay = 0.0f;
        private float blink_in = 0.0f;
        private float blink_out = 2.0f;
        private ColorRect blackout;

        public override void _Ready()
        {
            playerSpawner = GetNode<PlayerSpawner>("/root/PlayerSpawner");
            prefs = GetNode<Preferences>("/root/Preferences");

            MainCamera = GetNode<Camera3D>("Camera3D");

            blackout = GetNode<ColorRect>("ColorRect");

            CharacterHub.Spawned += OnPlayerSpawned;
            CharacterHub.Destroyed += OnPlayerDestroyed;

            GD.PrintRich($"[CameraBridge] [color={ColorsHex.MediumSeaGreen}]Ready[/color]");
        }

        public void OnPlayerSpawned(CharacterHub _characterHub)
        {
            characterHub = _characterHub;
        }

        public void OnPlayerDestroyed(CharacterHub _characterHub)
        {
            characterHub = null;
        }

        public void Blink()
        {
            var tween = GetTree().CreateTween();
            tween.TweenProperty(blackout, "self_modulate:a", 1, blink_in).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
            tween.TweenProperty(blackout, "self_modulate:a", 0, blink_out).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetDelay(blink_delay);
        }


    }

}

