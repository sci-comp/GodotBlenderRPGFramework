using Godot;

namespace Game
{
    public partial class CharacterController : CharacterBody3D
    {
        [ExportCategory("Character")]
        [Export] public float BaseSpeed = 5.0f;
        [Export] public float SprintSpeed = 8.0f;
        [Export] public float JumpVelocity = 4.5f;

        [ExportGroup("Controls")]
        [Export] public string Jump = "jump";
        [Export] public string Left = "move_left";
        [Export] public string Right = "move_right";
        [Export] public string Forward = "move_forward";
        [Export] public string Backward = "move_backward";
        [Export] public string Pause = "start";
        [Export] public string Sprint = "sprint";

        private float gravity;
        private float speed;
        private CameraBridge cameraBridge;

        public override void _Ready()
        {
            gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
            speed = BaseSpeed;
            cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!IsOnFloor())
            {
                Velocity = new Vector3(Velocity.X, Velocity.Y - gravity * (float)delta, Velocity.Z);
            }

            if (Input.IsActionJustPressed(Jump) && IsOnFloor())
            {
                Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);
            }

            speed = Input.IsActionPressed(Sprint) ? SprintSpeed : BaseSpeed;

            Vector2 inputDir = Input.GetVector(Left, Right, Forward, Backward);

            Vector3 cameraForward = cameraBridge.MainCamera.GlobalTransform.Basis.Z;
            Vector3 cameraRight = cameraBridge.MainCamera.GlobalTransform.Basis.X;

            cameraForward.Y = 0;
            cameraRight.Y = 0;
            cameraForward = cameraForward.Normalized();
            cameraRight = cameraRight.Normalized();

            Vector3 direction = (cameraForward * inputDir.Y + cameraRight * inputDir.X).Normalized();

            if (direction != Vector3.Zero)
            {
                Velocity = new Vector3(direction.X * speed, Velocity.Y, direction.Z * speed);

                float targetAngle = Mathf.Atan2(direction.X, direction.Z);
                Rotation = new Vector3(Rotation.X, targetAngle, -Rotation.Z);
            }
            else
            {
                Velocity = new Vector3(
                    Mathf.MoveToward(Velocity.X, 0, speed),
                    Velocity.Y,
                    Mathf.MoveToward(Velocity.Z, 0, speed)
                );
            }

            MoveAndSlide();
        }

        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed(Pause))
            {
                ToggleMouseCapture();
            }
        }

        private void ToggleMouseCapture()
        {
            if (Mouse.IsCursorCaptured())
            {
                Mouse.SetVisible("CharacterController");
            }
            else
            {
                Mouse.SetCaptured("CharacterController");
            }
        }

    }

}

