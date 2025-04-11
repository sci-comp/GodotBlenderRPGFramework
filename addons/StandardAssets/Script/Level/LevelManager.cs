using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

namespace Game
{
    public partial class LevelManager : Node
    {
        [Export] public float FadeSpeedScale = 1f;
        [Export] public Color FadeToColor = new("#000000");
        [Export] public Dictionary<StringName, LevelInfo> LevelInfo = [];

        private string spawnpointAfterEpigraph = "";
        private readonly object levelChangeLock = new();
        private AnimationPlayer animationPlayer;
        private ColorRect colorRect;
        private Label levelNameLabel;
        private LevelInfo epigraphLevelInfo;

        public bool IsTransitioning { get; set; } = false;
        public string CurrentLevelName => CurrentLevelInfo.LevelName;
        public string CurrentLevelID => CurrentLevelInfo.LevelID;
        public string LevelIDAfterEpigraph { get; set; } = "";
        public LevelInfo CurrentLevelInfo { get; set; }
        public Node SceneTree { get; set; }
        public Node SceneTreeRoot { get; set; }
        public Node3D CurrentLevel { get; set; }
        public SaveManager SaveManager { get; set; }  // Populated by SaveManager itself later in the node order-- consider redesigning?

        public event Action LevelLoaded;
        public event Action<string, string> BeginUnloadingLevel;
        public event Action FadeInComplete;
        public event Action FadeOutComplete;

        public override void _Ready()
        {
            CurrentLevel = GetTree().CurrentScene as Node3D;

            if (LevelInfo.TryGetValue(CurrentLevel.Name, out LevelInfo _currentLevelInfo))
            {
                CurrentLevelInfo = _currentLevelInfo;
            }
            else
            {
                GD.PrintErr($"[LevelManager] Unable to find first scene {CurrentLevel.Name} in Level Info collection");
            }

            animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            colorRect = GetNode<ColorRect>("CanvasLayer/ColorRect");
            levelNameLabel = GetNode<Label>("LevelNameLabel");
            levelNameLabel.Modulate = new Color(1, 1, 1, 0);

            if (!LevelInfo.TryGetValue("Epigraph", out epigraphLevelInfo))
            {
                GD.PrintErr("[LevelManager] epigraphLevelInfo not found");
            }

            GD.Print($"[LevelManager] Loaded information on {LevelInfo.Count} levels");

        }

        public void ReturningFromEpigraph()
        {
            if (string.IsNullOrEmpty(LevelIDAfterEpigraph))
            {
                GD.PrintErr("[LevelManager] LevelIDAfterEpigraph IsNullOrEmpty");
                return;
            }

            SaveManager?.UpdateBooleanValue($"Epigraph_{LevelIDAfterEpigraph}", true);
            ChangeLevel(LevelIDAfterEpigraph, spawnpointAfterEpigraph);
        }

        public void ChangeLevel(string levelID) => ChangeLevel(levelID, "");

        public void ChangeLevel(string nextLevelID, string nextSpawnpoint)
        {
            if (IsTransitioning)
            {
                GD.PrintErr("[LevelManager] Level transition already in progress");
                return;
            }

            if (!LevelInfo.TryGetValue(nextLevelID, out LevelInfo nextLevelInfo))
            {
                GD.PrintErr($"[LevelManager] Requested level not found: {nextLevelID}");
                return;
            }

            if (nextLevelInfo.Level == null)
            {
                GD.PrintErr($"[LevelManager] Missing level in collection: {nextLevelID}");
                return;
            }

            if (nextLevelInfo.HasEpigraph && SaveManager != null && !SaveManager.GetBooleanValue($"Epigraph_{nextLevelID}"))
            {
                GD.Print("[LevelManager] Diverting to epigraph...");
                LevelIDAfterEpigraph = nextLevelID;
                spawnpointAfterEpigraph = nextSpawnpoint;
                nextLevelInfo = epigraphLevelInfo;
                nextSpawnpoint = "";
            }

            IsTransitioning = true;
            CallDeferred(nameof(ChangeLevelNow), nextLevelInfo, nextSpawnpoint);
        }

        private async void ChangeLevelNow(LevelInfo nextLevelInfo, string spawnpoint)
        {
            if (!IsTransitioning)
            {
                return;  // Safety check in case of multiple deferred calls
            }

            GD.Print($"[LevelManager] Changing to: {nextLevelInfo.LevelName}");

            BeginUnloadingLevel?.Invoke(nextLevelInfo.LevelID, spawnpoint);
            await FadeOut();

            var oldLevel = CurrentLevel;
            oldLevel?.QueueFree();
            await ToSignal(oldLevel, "tree_exited");

            CurrentLevelInfo = nextLevelInfo;
            CurrentLevel = nextLevelInfo.Level.Instantiate<Node3D>();
            GetTree().Root.AddChild(CurrentLevel);

            LevelLoaded?.Invoke();
            await FadeIn();

            DisplayLevelName();

            IsTransitioning = false;
        }



        public async Task FadeOut()
        {
            animationPlayer.SpeedScale = FadeSpeedScale;
            animationPlayer.Play("FadeToBlack");
            await ToSignal(animationPlayer, "animation_finished");
            FadeOutComplete?.Invoke();
        }

        public async Task FadeIn()
        {
            animationPlayer.SpeedScale = FadeSpeedScale;
            animationPlayer.PlayBackwards("FadeToBlack");
            await ToSignal(animationPlayer, "animation_finished");
            FadeInComplete?.Invoke();
        }

        private async void DisplayLevelName()
        {
            if (levelNameLabel == null)
            {
                return;
            }

            levelNameLabel.Text = CurrentLevelInfo.LevelName;
            levelNameLabel.Modulate = new Color(1, 1, 1, 0);
            levelNameLabel.Visible = true;
            Tween fadeInTween = CreateTween();
            fadeInTween.TweenProperty(levelNameLabel, "modulate:a", 1, .5);
            await ToSignal(fadeInTween, "finished");
            await ToSignal(GetTree().CreateTimer(2), "timeout");
            Tween fadeOutTween = CreateTween();
            fadeOutTween.TweenProperty(levelNameLabel, "modulate:a", 0, 1);
            await ToSignal(fadeOutTween, "finished");
            levelNameLabel.Visible = false;
        }

    }

}

