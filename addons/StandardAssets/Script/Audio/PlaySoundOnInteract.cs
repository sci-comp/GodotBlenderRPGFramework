using Godot;

namespace Game
{
    public partial class PlaySoundOnInteract : InteractableArea
    {
        [Export] public string SoundName = "DoorOpen";
        [Export] public string _Title = "Object";
        [Export] public string _Details = "Unidentified";

        private SFX player;

        public override string Title => _Title;
        public override string Details => _Details;

        public override void _Ready()
        {
            player = GetNode<SFX>("/root/SFX");
        }

        public override void Interact(string playerID)
        {
            player.PlaySound(SoundName, Position);
            base.Interact();

        }

    }

}

