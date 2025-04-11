using System;

namespace Game
{

    public abstract partial class Interactable : Inspectable
    {
        public event Action<string> Interacted;

        public virtual void Interact(string playerID = "")
        {
            Interacted?.Invoke(playerID);
        }

    }

}

