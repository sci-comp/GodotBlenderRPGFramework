using Godot;

namespace Game
{
    public partial class LevelPortal : Area3D
    {
        [Export] public string LevelToLoad = "Sandbox";
        [Export] public string Spawnpoint = "SP_Sandbox";
        [Export] public string PortalEnteredSFX = "portal";

        private LevelManager levelManager;
        private SaveManager saveManager;

        public override void _Ready()
        {
            levelManager = GetNode<LevelManager>("/root/LevelManager");
            saveManager = GetNode<SaveManager>("/root/SaveManager");
            BodyEntered += OnBodyEnter;
        }

        public void OnBodyEnter(Node body)
        {
            if (!levelManager.IsTransitioning)
            {
                GD.Print($"[LevelPortal] Changing level to {LevelToLoad} with spawnpoint {Spawnpoint}");
                levelManager.ChangeLevel(LevelToLoad, Spawnpoint);
            }
        }

    }

}

