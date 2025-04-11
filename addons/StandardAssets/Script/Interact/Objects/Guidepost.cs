using Godot;

namespace Game
{
    public partial class Guidepost : InspectableArea
    {
        [Export] public string _Title = "Signpost";
        [Export] public string _Details = "";

        public override string Title => _Title;
        public override string Details => _Details;
    }

}

