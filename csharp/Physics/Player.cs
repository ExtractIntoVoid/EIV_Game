using Godot;

namespace ExtractIntoVoid.Physics;

public partial class Player : BasePlayer
{
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    // const stuffs
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;
    public const float RunSpeed = 8.5f;
    
    // Export stuffs
    [Export]
    float MouseSensitivity = 3f;

    public sealed override void _PhysicsProcess(double delta)
    {
        if (GetMultiplayerAuthority() != NetId)
            return;
        Vector3 velocity = Velocity;
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (GetMultiplayerAuthority() != NetId)
            return;

        if (@event is InputEventMouseButton && Input.MouseMode == Input.MouseModeEnum.Visible)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        if (@event is InputEventKey inputEventKey && inputEventKey.Pressed && inputEventKey.Keycode == Key.Escape && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            var _actualRotation = Vector3.Zero;
            // Horizontal mouse look.
            _actualRotation.Y -= mouseMotion.Relative.X / 1000 * MouseSensitivity;

            // Vertical mouse look.
            _actualRotation.X = Mathf.Clamp(-mouseMotion.Relative.Y / 1000 * MouseSensitivity, -1.45f, 1.45f);
            RotateY(_actualRotation.Y);
            Camera.RotateX(_actualRotation.X);
            if (Camera.Rotation.X < -0.45f || Camera.Rotation.X > 1.20f) //make sure we cannot loop over our head/feet
            {
                Camera.RotateX(-_actualRotation.X);
            }
        }
    }
}
