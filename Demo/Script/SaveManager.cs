using Game;
using Godot;

/// <summary>
/// Autoload singleton accessible at /root/SaveManager
/// This class is a stub, included here for demo purposes only.
/// </summary>
public partial class SaveManager : Node
{
    public string SpawnpointName = "SP_Island";
    public string CurrentLevel = "Island";

    private LevelManager levelManager;

    public override void _Ready()
    {
        levelManager = GetNode<LevelManager>("/root/LevelManager");
        levelManager.BeginUnloadingLevel += OnBeginUnloadingLevel;
    }
    private void OnBeginUnloadingLevel(string levelName, string spawnpoint)
    {
        CurrentLevel = levelName;
        SpawnpointName = spawnpoint;
    }

    public Marker3D FindSpawnpoint()
    {
        if (levelManager.CurrentLevel == null)
        {
            GD.PrintErr("[SaveManager] Current level not found");
            return null;
        }

        SceneTree tree = levelManager.CurrentLevel.GetTree();
        if (tree == null)
        {
            GD.PrintErr("[SaveManager] Could not find SceneTree");
            return null;
        }

        foreach (Node node in tree.GetNodesInGroup("Spawnpoint"))
        {
            if (node.Name == SpawnpointName)
            {
                if (node is not Marker3D marker)
                {
                    GD.PrintErr("[SaveManager] Spawnpoint is not of type Marker3D");
                    return null;
                }

                GD.Print("[SaveManager] Requested spawnpoint found: " + SpawnpointName);
                return marker;
            }
            else
            {
                GD.Print($"[SaveManager] Spawnpoint {node.Name} does not match the desired spawnpoint: {SpawnpointName}");
            }
        }

        GD.PrintErr("[SaveManager] No spawnpoint found in level: " + levelManager.CurrentLevelID);
        GD.PrintErr("[SaveManager] tree.GetNodesInGroup(\"Spawnpoint\").Count", tree.GetNodesInGroup("Spawnpoint").Count);
        return null;
    }

    public void UpdateBooleanValue(string _id, bool _)
    {
        // Placeholder
    }

    public bool GetBooleanValue(string _id)
    {
        // Placeholder
        return false;
    }

    public float GetTime()
    {
        // Todo
        return 0.0f;
    }

    public void SetTime(float time)
    {
        // Todo
    }
}

