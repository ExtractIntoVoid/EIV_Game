using Godot;

namespace ExtractIntoVoid.Physics;
// Next one is copy-pasted. will be working toward the better one.
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
    public Vector3 Pos = new();

    [Export]
    public Vector3 Rot = new();

    public override void _EnterTree()
    {
        NetId = Name.ToString().ToInt();
        SetMultiplayerAuthority(NetId);
        GetNode<MultiplayerSynchronizer>("MSync").SetMultiplayerAuthority(NetId);
        //GameManager.Instance.MyPlayerInstance = this;

    }

    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("Camera");
        Camera.Current = IsMultiplayerAuthority();
        HandlePoint = Camera.FindChild("HandlePoint");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsMultiplayerAuthority())
        {
            GlobalPosition = GlobalPosition.Lerp(Pos, 0.5f);
            GlobalRotation = GlobalRotation.Lerp(Rot, 0.5f);
            return;
        }
        bool IsJumping = false;
        var speed = Speed;
        Vector3 velocity = Velocity;
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;
        /*
        if (Input.IsActionJustPressed("inventory_open") && !GameManager.Instance.UIManagerInstance.IsInEscapeScene)
        {
            Inventory.Inventory_Used();
        }*/

        //if (GameManager.Instance.UIManagerInstance.IsInEscapeScene || Inventory.IsOpen)
        //return;
        if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
        {
            IsJumping = true;
            velocity.Y = JumpVelocity;
        }

        if (Input.IsActionJustPressed("use"))
            //Use();

            if (Input.IsActionJustPressed("cancel_use"))
                //CancelUse();

                if (Input.IsActionJustPressed("weapon_reload"))
                    //Reload();

                    if (Input.IsActionJustPressed("move_sprint"))
                        speed = RunSpeed;

        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        RpcId(1, "ServerMove", NetId, direction, Velocity, (float)delta, IsJumping);
        /*
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }

        Velocity = velocity;
        MoveAndSlide();
        Pos = GlobalPosition;
        Rot = GlobalRotation;*/
    }
    /*
    [Rpc(mode: MultiplayerApi.RpcMode.Authority)]
    public void ClientMove(int id, Vector3 velocity)
    {
        if (id == NetId)
        {

        }
    }*/

    [Rpc(mode: MultiplayerApi.RpcMode.Authority)]
    public void ServerMove(int id, Vector3 direction, Vector3 velocity, float delta, bool IsJumping)
    {
        GD.Print($"Client {id} wants to move to D: {direction}, with V: {velocity}, in Delta: {delta} ");
        if (!IsOnFloor())
            velocity.Y -= gravity * delta;
        if (IsOnFloor() && IsJumping)
            velocity.Y = JumpVelocity;

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
        MoveAndSlide();
        //Rpc("ClientMove", id, velocity);

    }
}
