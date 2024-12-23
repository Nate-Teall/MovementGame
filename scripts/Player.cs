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

	public Node3D head { get; private set; }
	private CollisionShape3D collision;

	// Camera movement variables
	[Export]
	private float lookSpeed = 0.001f;
	private Vector2 screenCenter;
	// holds the current X and Y rotation of the player's head
	private float rotationX;
	private float rotationY;

    public override void _Ready()
    {
		collision = GetChild<CollisionShape3D>(0);
		head = GetChild<Node3D>(1);

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
			//	modify the rotation based on mouse movement
			rotationX -= mouseMotion.Relative.X * lookSpeed;
			rotationY -= mouseMotion.Relative.Y * lookSpeed;

			// reset the rotation of the basis of the head
			// AKA x, y, z = [1,0,0] [0,1,0] [0,0,1]
			Transform3D headTransform = head.Transform;
        	headTransform.Basis = Basis.Identity;
        	head.Transform = headTransform;

			// Apply the rotation 
			head.RotateObjectLocal(Vector3.Up, rotationX);    // Rotate around the Y axis first (side-side motion)
			head.RotateObjectLocal(Vector3.Right, rotationY); // Rotate around the X axis next (up-down motion)
		}
	}

    public override void _PhysicsProcess(double delta)
	{
	}
}
