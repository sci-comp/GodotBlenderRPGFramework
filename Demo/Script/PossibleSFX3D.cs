using Godot;
using System.Collections.Generic;

namespace Game
{
    public partial class PossibleSFX3D : Node
    {
        public void Initialize(SFX player)
        {

        }

        public Dictionary<string, SoundGroup3D> GetSoundGroups()
        {
            return new();
        }

    }

}

