using ExtractIntoVoid.Managers;
using Godot;

namespace ExtractIntoVoid.Physics;

public partial class Player : CharacterBody3D
{
    public int NetId;
    public Node HandlePoint;

    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;
    public const float RunSpeed = 8.5f;

    Camera3D Camera;

    [Export]
    float MouseSensitivity = 3f;

    [Export]
    private Vector3 syncPos = new Vector3(0, 0, 0);
    [Export]
    private Vector3 syncRot = new Vector3(0, 0, 0);

    public override void _EnterTree()
    {
        NetId = Name.ToString().ToInt();
        SetMultiplayerAuthority(NetId);
        GetNode<MultiplayerSynchronizer>("MSync").SetMultiplayerAuthority(NetId);

    }

    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("Camera");
        HandlePoint = Camera.FindChild("HandlePoint");
        if (IsMultiplayerAuthority())
        {
            Camera.Current = IsMultiplayerAuthority();
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition = GlobalPosition.Lerp(syncPos, .5f);
        if (IsMultiplayerAuthority() && GetMultiplayerAuthority() == NetId)
        {
            bool IsJumping = false;

            Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
            Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
            Rpc("ServerMove", NetId, direction, (float)delta, IsJumping);
        }
    }


    [Rpc(mode: MultiplayerApi.RpcMode.Authority)]
    public void ServerMove(int id, Vector3 direction, float delta, bool IsJumping)
    {
        //  TODO: disable all things here. with #if SERVER
        //  TODO: and also add some interpolation or some shit.
        if (GameManager.Instance.BuildType != BuildType.Server)
            return;
        var node = this.GetParent().GetNode(id.ToString());
        var player = node as Player;
        Vector3 velocity = player.Velocity;
        GD.Print($"Client {id} wants to move to D: {direction}, with V: {velocity}, in Delta: {delta} ");
        if (!IsOnFloor())
            velocity.Y -= gravity * delta;
        if (IsOnFloor() && IsJumping)
            velocity.Y = JumpVelocity;
        if (IsOnFloor() && direction == Vector3.Zero)
            return;
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(player.Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, Speed);
        }
        player.Velocity = velocity;
        GD.Print($"Client {id} will move to D: {direction}, with V: {velocity}, in Delta: {delta} ");
        player.MoveAndSlide();
        GD.Print($"Client {id} new pos: {player.GlobalPosition} vel: {player.Velocity}");
        Rpc("ClientMove", id, player.GlobalPosition, player.Velocity);
    }


    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void ClientMove(int id, Vector3 Pos, Vector3 Vel)
    {
        var node = this.GetParent().GetNode(id.ToString());
        var player = node as Player;
        GD.Print($"Pos: {Pos}, Vel: {Vel}");
        player.GlobalPosition = player.GlobalPosition.Lerp(Pos, .5f);
        player.Velocity = player.Velocity.Lerp(Vel, .5f);
        player.syncPos = Pos;
        player.MoveAndSlide();
        GD.Print($"ClientMove: Client {id}[{player.NetId}] new pos: {player.GlobalPosition} vel: {player.Velocity}");
    }

    public override void _Input(InputEvent @event)
    {
        if (IsMultiplayerAuthority() && GetMultiplayerAuthority() == NetId)
        {
            if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                var _actualRotation = new Vector3(0, 0, 0);
                // Horizontal mouse look.
                _actualRotation.Y -= mouseMotion.Relative.X / 1000 * MouseSensitivity;

                // Vertical mouse look.
                _actualRotation.X = Mathf.Clamp(-mouseMotion.Relative.Y / 1000 * MouseSensitivity, -1.45f, 1.45f);
                RotateY(_actualRotation.Y);
                Camera.RotateX(_actualRotation.X);
                if (Camera.Rotation.X < -1.45f || Camera.Rotation.X > 1.45f)
                {
                    Camera.RotateX(-_actualRotation.X);
                }
            }
            //syncRot = GlobalRotation;
        }
    }
}
