using Godot;
using DialogueManagerRuntime;

namespace Game
{
    /// <summary>
    /// Autoload singleton accessible at /root/CameraBridge
    /// This class is a stub, included here for demo purposes only.
    /// </summary>
    public partial class CameraBridge : Node
    {
        public bool HasDefaultYaw => Mathf.IsEqualApprox(Yaw, 0.0f);
        public readonly float KeyboardTurnRate = 0.045f;
        public float Pitch = 0.0f;
        public float Yaw = 0.0f;
        public Camera3D MainCamera;

        // Autoload
        private LevelManager levelManager;
        private PlayerSpawner playerSpawner;
        private Preferences prefs;
        private SaveManager saveManager;

        // Spring length
        private float currentSpringLength = 2.5f;
        private float defaultSpringLength = 2.5f;
        private float targetSpringLength;
        private float maxSpringLength = 5f;
        private float zoomRate = 0.2f;
        private float gamepadZoomRate = 0.1f;

        private float blink_delay = 0.0f;
        private float blink_in = 0.0f;
        private float blink_out = 2.0f;
        private float pitchDefault = Mathf.DegToRad(-18.0f);
        private float pitchMin = Mathf.DegToRad(-45);
        private float pitchMax = Mathf.DegToRad(45);
        private CharacterHub characterHub;
        private ColorRect blackout;
        private Node3D nodePhantomCamera3D;
        private Node3D lookAtTarget;

        private CameraAngles currentActiveCameraAngles;
        private CameraAngle currentActiveAngle;

        public Vector3 CameraPosition => MainCamera.Position;

        public override void _Ready()
        {
            levelManager = GetNode<LevelManager>("/root/LevelManager");
            playerSpawner = GetNode<PlayerSpawner>("/root/PlayerSpawner");
            prefs = GetNode<Preferences>("/root/Preferences");
            saveManager = GetNode<SaveManager>("/root/SaveManager");

            MainCamera = GetNode<Camera3D>("Camera3D");
            nodePhantomCamera3D = GetNode<Node3D>("PhantomCamera3D");

            blackout = GetNode<ColorRect>("ColorRect");

            targetSpringLength = defaultSpringLength;
            nodePhantomCamera3D.Set("spring_length", defaultSpringLength);
            nodePhantomCamera3D.Call("set_fov", 65);

            Vector3 initialRotation = (Vector3)nodePhantomCamera3D.Call("get_third_person_rotation_degrees");
            Pitch = Mathf.DegToRad(initialRotation.X);
            Yaw = Mathf.DegToRad(initialRotation.Y);

            CharacterHub.Spawned += OnPlayerSpawned;
            CharacterHub.Destroyed += OnPlayerDestroyed;
            levelManager.BeginUnloadingLevel += OnBeginUnloadingLevel;
            DialogueManager.DialogueEnded += OnDialogueEnded;
            Mouse.SetCaptured("CharacterController");
            GD.PrintRich($"[CameraBridge] [color={ColorsHex.MediumSeaGreen}]Ready[/color]");
        }

        float xSensitivity = 0.06f;
        float ySensitivity = 0.04f;

        public override void _Process(double delta)
        {
            // Yaw
            Yaw -= mouseTwistInput * xSensitivity;
            Yaw = Mathf.Wrap(Yaw, 0, Mathf.Tau);

            // Pitch
            Pitch += mousePitchInput * ySensitivity;
            Pitch = Mathf.Clamp(Pitch, pitchMin, pitchMax);

            Vector3 newRotation = new(Mathf.RadToDeg(Pitch), Mathf.RadToDeg(-Yaw), 0);
            nodePhantomCamera3D.Call("set_third_person_rotation_degrees", newRotation);

            mouseTwistInput = 0.0f;
            mousePitchInput = 0.0f;
        }

        private float mouseTwistInput = 0.0f;
        private float mousePitchInput = 0.0f;

        public override void _UnhandledInput(InputEvent inputEvent)
        {
            if (inputEvent.IsActionPressed("zoom_in"))
            {
                GetViewport().SetInputAsHandled();
                targetSpringLength -= zoomRate;
                targetSpringLength = Mathf.Clamp(targetSpringLength, 1, maxSpringLength);
            }

            if (inputEvent.IsActionPressed("zoom_out"))
            {
                GetViewport().SetInputAsHandled();
                targetSpringLength += zoomRate;
                targetSpringLength = Mathf.Clamp(targetSpringLength, 1, maxSpringLength);
            }

            if (inputEvent is InputEventMouseMotion eventMouseMotion)
            {
                //if (Input.IsActionPressed("left_click") || Input.IsActionPressed("right_click"))
                //{
                mouseTwistInput = -eventMouseMotion.Relative.X * xSensitivity;
                mousePitchInput = -eventMouseMotion.Relative.Y * ySensitivity;
                //}
            }
        }

        public void Blink()
        {
            var tween = GetTree().CreateTween();
            tween.TweenProperty(blackout, "self_modulate:a", 1, blink_in).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
            tween.TweenProperty(blackout, "self_modulate:a", 0, blink_out).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetDelay(blink_delay);
        }

        public void SetActiveCamera(CameraAngles cameraAngles, CameraAngle angle)
        {
            if (currentActiveCameraAngles != null && currentActiveCameraAngles != cameraAngles)
            {
                currentActiveCameraAngles.SetCameraPriority(currentActiveAngle, 0);
            }

            cameraAngles.SetCameraPriority(angle);
            currentActiveCameraAngles = cameraAngles;
            currentActiveAngle = angle;
        }

        public void ResetActiveCamera()
        {
            if (currentActiveCameraAngles != null)
            {
                currentActiveCameraAngles.SetCameraPriority(currentActiveAngle, 0);
                currentActiveCameraAngles = null;
            }
        }

        protected void OnDialogueEnded(Resource dialogueResource)
        {
            ResetActiveCamera();
        }

        public void OnBeginUnloadingLevel(string nextLevel, string nextSpawnpoint)
        {
            nodePhantomCamera3D.Set("follow_target", default);
            nodePhantomCamera3D.Set("look_at_target", default);
        }

        public void OnPlayerSpawned(CharacterHub _characterHub)
        {
            characterHub = _characterHub;
            lookAtTarget = characterHub.LookAt;

            nodePhantomCamera3D.Set("follow_target", lookAtTarget);
            nodePhantomCamera3D.Set("look_at_target", lookAtTarget);

            GD.Print("[CameraBridge] Updated look targets");
        }

        public void OnPlayerDestroyed(CharacterHub _characterHub)
        {
            characterHub = null;
            lookAtTarget = null;

            GD.Print("[CameraBridge] Player destroyed, cleared references");
        }

        public Vector3 PhantomCameraPosition => nodePhantomCamera3D.GlobalPosition;
    }

}

