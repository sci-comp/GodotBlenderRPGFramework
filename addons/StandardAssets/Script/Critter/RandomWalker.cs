using Game;
using Godot;
using System;

public partial class RandomWalker : CharacterBody3D
{
    [Export] public float Speed = 3.0f;
    [Export] public float wanderDistance = 3.0f;

    private NextEventTimer eggTimer;
    private Timer nextEventTimer;
    private NavigationAgent3D navAgent;
    private readonly Random random = new();

    public override void _Ready()
    {
        eggTimer = GetNode<NextEventTimer>("EggTimer");
        eggTimer.EventTriggered += UpdateLocation;
        navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        navAgent.VelocityComputed += VelocityComputed;
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;
        Vector3 newVelocity = Vector3.Zero;

        if ((GlobalPosition - navAgent.TargetPosition).Length() < 0.25f)
        {
            // Resting
        }
        else
        {
            newVelocity = (navAgent.GetNextPathPosition() - GlobalPosition).Normalized() * Speed;
            newVelocity.Y = 0;
        }

        if (newVelocity.Length() > 0.1f)
        {
            Vector3 dir = newVelocity.Normalized();
            Vector3 targetRotation = new(0, Mathf.Atan2(dir.X, dir.Z), 0);
            Rotation = Rotation.Lerp(targetRotation, .1f);
        }

        newVelocity.Y = Velocity.Y - 9.81f * dt;

        navAgent.Velocity = newVelocity;
        
    }

    private void UpdateLocation()
    {
        Vector3 newPosition = GenerateRandomPosition();
        navAgent.TargetPosition = newPosition;
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector2 randomCircle = Vector2.Right.Rotated(random.NextSingle() * Mathf.Pi * 2) * random.NextSingle() * wanderDistance;
        Vector3 randomOffset = new(randomCircle.X, 0, randomCircle.Y);
        Vector3 targetPosition = GlobalTransform.Origin + randomOffset;

        navAgent.TargetPosition = targetPosition;

        return navAgent.GetNextPathPosition();
    }

    private void VelocityComputed(Vector3 _velocity) 
    {
        Velocity = Velocity.MoveToward(_velocity, 0.25f);
        MoveAndSlide();
    }

}

