using Godot;
using System;
using System.Dynamic;

public partial class Player : CharacterBody3D
{	
	[Export]
	public float walkSpeed { get; private set; }
	[Export]
	public float sprintSpeed { get; private set; }
	[Export]
	public float airSpeed { get; private set; }
	[Export]
	public float jumpVelocity { get; private set; }

	private const float height = 2;
	private const float cameraOffset = 0.6f;

	private float currentSpeed;

	private Node3D head;
	private CollisionShape3D collision;

    public override void _Ready()
    {
		collision = GetChild<CollisionShape3D>(0);
		head = GetChild<Node3D>(1);
    }

    public override void _PhysicsProcess(double delta)
	{
		/*Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = jumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * currentSpeed;
			velocity.Z = direction.Z * currentSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, currentSpeed);
		}

		// Handle Crouch
		if (Input.IsActionPressed("crouch"))
		{
			head.Position = new Vector3(head.Position.X, cameraOffset * 0.5f, head.Position.Z);
			CapsuleShape3D collisionShape = (CapsuleShape3D)collision.Shape;
			collisionShape.Height = 0.5f * height;
		}
		else
		{
			head.Position = new Vector3(head.Position.X, cameraOffset, head.Position.Z);
			CapsuleShape3D collisionShape = (CapsuleShape3D)collision.Shape;
			collisionShape.Height = height;
		}

		// Handle Sprint
		if (Input.IsActionPressed("sprint"))
		{
			currentSpeed = sprintSpeed;
		}
		else
		{
			currentSpeed = walkSpeed;
		}

		Velocity = velocity;
		MoveAndSlide();*/
	}
}
