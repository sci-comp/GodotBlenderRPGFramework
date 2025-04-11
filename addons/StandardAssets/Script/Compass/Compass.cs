using Godot;
using System.Collections.Generic;

namespace Game
{
    public partial class Compass : Control
    {
        private TextureRect north;
        private TextureRect east;
        private TextureRect south;
        private TextureRect west;

        private bool isEnabled = true;
        private Camera3D mainCamera;
        private const int MaxPOIs = 32;
        private readonly List<PointOfInterest> listOfPointsOfInterest = [];

        private CameraBridge cameraBridge;
        private LevelManager levelManager;
        private PlayerSpawner playerSpawner;

        public override void _Ready()
        {
            cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");
            levelManager = GetNode<LevelManager>("/root/LevelManager");
            playerSpawner = GetNode<PlayerSpawner>("/root/PlayerSpawner");

            if (cameraBridge == null)
            {
                GD.Print("[Compass] Camera bridge is null");
            }
            else
            {
                mainCamera = cameraBridge.MainCamera;

                if (mainCamera == null)
                {
                    GD.PrintErr("[Compass] Camera is null");
                }
            }

            north = GetNode<TextureRect>("North");
            east = GetNode<TextureRect>("East");
            south = GetNode<TextureRect>("South");
            west = GetNode<TextureRect>("West");

            PointOfInterest.POISpawned += OnPOISpawned;
            PointOfInterest.POIDestroyed += OnPOIDestroyed;
            levelManager.BeginUnloadingLevel += OnBeginUnloadingLevel;

            Visible = false;
            GD.Print("[Compass] Ready");
        }

        public override void _Process(double delta)
        {
            UpdateCompass();
        }

        public override void _ExitTree()
        {
            PointOfInterest.POISpawned -= OnPOISpawned;
            PointOfInterest.POIDestroyed -= OnPOIDestroyed;
            levelManager.BeginUnloadingLevel -= OnBeginUnloadingLevel;
        }

        public void SetVisibility(bool visible)
        {
            Visible = visible;
        }

        private void OnBeginUnloadingLevel(string nextLevel, string nextSpawnpoint)
        {
            GD.Print("[Compass] On begin unloading level, setting visible to true");
            Visible = false;
        }
        
        private void OnPOIDestroyed(PointOfInterest poi)
        {
            if (listOfPointsOfInterest.Contains(poi))
            {
                listOfPointsOfInterest.Remove(poi);
                poi.IconRepresentation?.QueueFree();
            }
        }

        private void OnPOISpawned(PointOfInterest poi)
        {
            var poiIcon = new TextureRect
            {
                Texture = poi.IconTexture,
                Name = poi.Name + "_Icon",
                Visible = false,
                StretchMode = TextureRect.StretchModeEnum.KeepAspect
            };
            AddChild(poiIcon);
            poi.IconRepresentation = poiIcon;
            listOfPointsOfInterest.Add(poi);
        }

        private void UpdateCompass()
        {
            if (mainCamera == null)
            {
                GD.PrintErr("[Compass] Cannot find camera");
                return;
            }

            var playerOrientation = mainCamera.GlobalTransform.Basis.GetEuler().Y;
            UpdateIndicatorPosition(north, playerOrientation, 0);
            UpdateIndicatorPosition(west, playerOrientation, Mathf.Pi / 2);
            UpdateIndicatorPosition(south, playerOrientation, Mathf.Pi);
            UpdateIndicatorPosition(east, playerOrientation, 3 * Mathf.Pi / 2);

            int count = 0;
            foreach (PointOfInterest poi in listOfPointsOfInterest)
            {
                if (count < MaxPOIs)
                {
                    Vector3 directionToPOI = (poi.GlobalTransform.Origin - mainCamera.GlobalTransform.Origin).Normalized();
                    float poiDirectionAngle = Mathf.Atan2(directionToPOI.X, directionToPOI.Z) + Mathf.Pi;
                    poiDirectionAngle = Mathf.PosMod(poiDirectionAngle, Mathf.Pi * 2);
                    UpdateIndicatorPosition(poi.IconRepresentation, playerOrientation, poiDirectionAngle);
                    count++;
                }
            }
        }

        private void UpdateIndicatorPosition(TextureRect indicator, float playerOrientation, float directionAngle)
        {
            playerOrientation = Mathf.PosMod(playerOrientation, Mathf.Pi * 2);
            float angleDifference = Mathf.Wrap(directionAngle - playerOrientation, -Mathf.Pi, Mathf.Pi);

            bool isInFrontOfPlayer = Mathf.Abs(angleDifference) <= Mathf.Pi / 2;

            if (isInFrontOfPlayer)
            {
                float normalizedPosition = -1.0f * angleDifference / (Mathf.Pi / 2);

                float scaledPosition = normalizedPosition * Size.X / 2;
                float offset = (Size.X / 2) - (indicator.Size.X / 2);
                indicator.Position = new Vector2(scaledPosition + offset, indicator.Position.Y);
                indicator.Visible = true;
            }
            else
            {
                indicator.Visible = false;
            }
        }

    }

}

