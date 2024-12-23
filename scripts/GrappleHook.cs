using Godot;
using System;

public partial class GrappleHook : Node3D
{

	private enum GrappleState
	{
		WAITING,
		FIRED,
		RETURNING
	}

	[Export]
	private float projectileSpeed = 50f;
	[Export]
	private float returnSpeed = 75f;

	private GrappleState state = GrappleState.WAITING;

	private Vector3 direction = Vector3.Zero;

	private Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("../Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _PhysicsProcess(double delta)
    {
		Vector3 newPos = Position;

		switch (state)
		{
			case GrappleState.FIRED:
				// When firing, ove in a straight line
				newPos += direction * projectileSpeed * (float)delta;
				break;
			case GrappleState.RETURNING:
				// When returning, travel back to the player
				direction = (player.Position - this.Position).Normalized();
				newPos += direction * returnSpeed * (float)delta;
				break;
		}

		Position = newPos;
        
    }

    public void _BodyEntered(Node3D body) 
	{
		if (state == GrappleState.FIRED && body is not Player)
		{
			direction = Vector3.Zero;
		}
		else if (state == GrappleState.RETURNING && body is Player)
		{
			Hide();
			state = GrappleState.WAITING;
		}
	}

	// Fire the grappling hook from the given position in a certain direction
	// For now this is always the player's head
	public void Fire(Vector3 position, Vector3 direction)
	{
		Show();
		state = GrappleState.FIRED;
		Position = position;
		this.direction = direction;
	}

	public void Return() { state = GrappleState.RETURNING; }
}
