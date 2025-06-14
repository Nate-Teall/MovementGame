using Godot;
using System;

public partial class Player : CharacterBody3D
{	
	// Movement speed variables
	[Export]
	public float walkSpeed { get; private set; } = 8f;
	[Export]
	public float sprintSpeed { get; private set; } = 16f;
	[Export]
	public float airAccel { get; private set; } = 6f;
	[Export]
	public float jumpVelocity { get; private set; } = 5f;
	[Export]
	public float crouchSpeed { get; private set; } = 4f;
	[Export]
	public float slideDecel { get; private set; } = 8f;

	// Grappling hook variables
	public GrappleHook grapplingHook { get; private set; }

	// Other constants
	public const float crouchHeightScale = 0.3f;

	// Child nodes
	public Node3D head { get; private set; }
	public CollisionShape3D collision { get; private set; }
	private StateMachine stateMachine;

	// Camera movement variables
	[Export]
	private float lookSpeed = 0.001f;
	private Vector2 screenCenter;
	// holds the current X and Y rotation of the player's head
	private float rotationX;
	private float rotationY;

	// Saved location for quick restarting
	private Vector3 savedLocation = new Vector3(0,0,0);


	// Scenes
	private PackedScene grappleHookScene = GD.Load<PackedScene>("res://scenes/grapple_hook.tscn");

    public override void _Ready()
    {
		collision = GetChild<CollisionShape3D>(0);
		head = GetChild<Node3D>(1);
		stateMachine = GetChild<StateMachine>(2);

		grapplingHook = GetNode<GrappleHook>("../GrappleHook");
		grapplingHook.Attached += _GrappleAttached;

		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		screenCenter = new Vector2(
			GetViewport().GetVisibleRect().Size.X / 2,
			GetViewport().GetVisibleRect().Size.Y / 2
		);
    }

	public override void _Input(InputEvent @event)
	{
		// Code for mouse movement was taken from: https://docs.godotengine.org/en/4.0/tutorials/3d/using_transforms.html
		// Saving this link for future reference on using Transforms.
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// modify the rotation based on mouse movement
			rotationX -= mouseMotion.Relative.X * lookSpeed;
			rotationY -= mouseMotion.Relative.Y * lookSpeed;

			// Limit the max angle the player can look up/down
			rotationY = rotationY > Mathf.Pi / 2 ? Mathf.Pi / 2 : rotationY;
			rotationY = rotationY < -Mathf.Pi / 2 ? -Mathf.Pi / 2 : rotationY;

			// reset the rotation of the basis of the head and collision box
			// AKA x, y, z = [1,0,0] [0,1,0] [0,0,1]
			Transform3D headTransform = head.Transform;
			headTransform.Basis = Basis.Identity;
			head.Transform = headTransform;

			Transform3D collisionTransform = collision.Transform;
			collisionTransform.Basis = Basis.Identity;
			collision.Transform = collisionTransform;

			// Apply the rotation 
			head.RotateObjectLocal(Vector3.Up, rotationX);    // Rotate around the Y axis first (side-side motion)
			head.RotateObjectLocal(Vector3.Right, rotationY); // Rotate around the X axis next (up-down motion)

			// Rotate the collision box as well
			collision.RotateObjectLocal(Vector3.Up, rotationX); // Body should be rotated ONLY side-side
		}

		if (Input.IsActionJustPressed("grapple"))
		{
			grapplingHook.Fire(head.GlobalPosition, -head.Transform.Basis.Z.Normalized());
		}
		else if (Input.IsActionJustReleased("grapple"))
		{
			grapplingHook.Return();
		}

		if (Input.IsActionJustPressed("set"))
		{
			savedLocation = this.Position;
		}
		if (Input.IsActionJustPressed("reset"))
		{
			Position = savedLocation;
			Velocity = new Vector3(0, 0, 0);
		}
	}

    public override void _PhysicsProcess(double delta)
	{
	}

	public void _GrappleAttached()
	{
		// Transition to the grappling state when the hook attaches to a surface. 
		// Transitioning between states outside of a State class is probably not best practice
		// The alternative is either making every state listen for the Attached Event, or
		// 	have every PlayerState check the grapplingHook state every frame it is active
		stateMachine.TransitionToNextState("Grappling");
	}
}
