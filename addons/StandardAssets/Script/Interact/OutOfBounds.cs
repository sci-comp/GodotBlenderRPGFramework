using Godot;

namespace Game
{
    public partial class OutOfBounds : Area3D
    {
        [Export] public string SfxToPlay = "Splash";
        
        private CameraBridge cameraBridge;
        private LevelManager levelManager;
        private SaveManager saveManager;
        private SFX sfx;

        public override void _Ready()
        {
            saveManager = GetNode<SaveManager>("/root/SaveManager");
            sfx = GetNode<SFX>("/root/SFX");
            cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");
            levelManager = GetNode<LevelManager>("/root/LevelManager");
            BodyEntered += OnBodyEntered;
        }

        private void OnBodyEntered(Node body)
        {
            if (body is CharacterBody3D characterBody)
            {
                Marker3D sp = saveManager.FindSpawnpoint();

                if (sp == null)
                {
                    GD.PrintErr("[OutOfBounds] Spawnpoint not found");
                    return;
                }

                cameraBridge.Blink();
                sfx.PlaySound(SfxToPlay, characterBody.GlobalPosition);
                characterBody.GlobalTransform = new Transform3D(characterBody.GlobalTransform.Basis, sp.GlobalTransform.Origin);
            }
        }

    }

}

