using Godot;

namespace Game
{
    public abstract partial class Inspectable : StaticBody3D
    {
        public virtual string Title => "Default Title";
        public virtual string Details => "Default Details";

        public virtual void Inspect()
        {
            // Do nothing
        }

        public virtual void Select()
        {
            // Do nothing
        }

        public virtual void Deselect()
        {
            // Do nothing
        }

    }

}