using Godot;
using System.Collections.Generic;

namespace Game
{
    public partial class SFX2D : Node
    {
        public Dictionary<string, AudioStreamPlayer> SoundGroups = new();

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is AudioStreamPlayer)
                {
                    SoundGroups[child.Name] = child as AudioStreamPlayer;
                }
            }

            GD.Print(string.Format("[SFX2D] Ready with {0} sound groups", SoundGroups.Count));
        }

        public void PlaySound(string soundGroupName)
        {
            if (SoundGroups.TryGetValue(soundGroupName, out AudioStreamPlayer player))
            {
                if (player != null)
                {
                    GD.Print("[SFX2D] Playing: " + soundGroupName);
                    player.Play();
                }
            }
            else
            {
                GD.Print("[SFX2D] Requested a sound group that does not exist: " + soundGroupName);
            }
        }

    }

}

