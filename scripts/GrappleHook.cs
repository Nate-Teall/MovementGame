using Godot;
using System;

public partial class GrappleHook : Node3D
{

	[Export]
	private float projectileSpeed = 5f;

	public Vector3 direction;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Move in a straight line
		Vector3 newPos = Position;
		newPos += direction * projectileSpeed * (float)delta;
		Position = newPos;
	}

	public void _BodyEntered(Node3D body) 
	{
		if (body is not Player)
		{
			QueueFree();
		}
	}
}
