using Godot;
using System.Collections.Generic;

namespace Game
{
    public partial class PlatformLeverHub : Node
    {
        private readonly List<Lever> levers = new();
        private readonly List<IActivatedPlatform> platforms = new();

        public override void _Ready()
        {
            Toolbox.Toolbox.FindAndPopulate(this, levers);
            Toolbox.Toolbox.FindAndPopulate(this, platforms);

            foreach (Lever lever in levers)
            {
                lever.Interacted += OnInteract;
            }

            foreach (IActivatedPlatform platform in platforms)
            {
                platform.OnCanBeActivated += OnCanBeActivated;
            }
        }

        private void OnInteract(string playerID)
        {
            foreach (IActivatedPlatform platform in platforms)
            {
                platform.Activate();
            }
        }

        private void OnCanBeActivated() 
        { 
            foreach (Lever lever in levers)
            {
                lever.ResetLever();
            }
        }

    }

}

