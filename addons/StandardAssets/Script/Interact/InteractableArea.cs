using System;

public abstract partial class InteractableArea : InspectableArea
{
    public event Action<string> Interacted;

    public virtual void Interact(string playerID = "")
    {
        Interacted?.Invoke(playerID);
    }

}

