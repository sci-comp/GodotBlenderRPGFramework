using System;

namespace Game
{
    public interface IActivatedPlatform
    {
        public event Action OnCanBeActivated;
        public bool CanBeActivated();
        public bool Enabled();
        public void Enable();
        public void Disable();
        public void Activate();
    }

}

