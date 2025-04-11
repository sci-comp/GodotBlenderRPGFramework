using Godot;
using System;


namespace Game
{
    public partial class PointOfInterest : Node3D
    {
        [Export] public Texture2D IconTexture;
        //[Export] public bool DestroyWhenFound = false;

        public TextureRect IconRepresentation { get; set; }

        public static event Action<PointOfInterest> POISpawned;
        public static event Action<PointOfInterest> POIDestroyed;

        public override void _Ready()
        {
            AddToGroup("POI");
            POISpawned?.Invoke(this);
            //BodyEntered += OnBodyEntered;
        }

        /*private void OnBodyEntered(Node3D other)
        {
            if (DestroyWhenFound)
            {
                POIDestroyed?.Invoke(this);
                QueueFree();
            }
        }*/

    }

}

