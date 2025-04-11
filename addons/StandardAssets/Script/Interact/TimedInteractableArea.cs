using Godot;
using System;

namespace Game
{
    public abstract partial class TimedInteractableArea : InteractableArea, ITimedInteractable
    {
        [Export] public bool HasInteractionDuration { get; set; } = true;
        [Export] public float InteractionDuration { get; set; } = 2.0f;

        public virtual void CompleteInteraction(string playerID)
        {
            bool success = OnInteractionComplete(playerID);
            Interact(playerID);
        }

        protected virtual bool OnInteractionComplete(string playerID)
        {
            return true;
        }
    }
}