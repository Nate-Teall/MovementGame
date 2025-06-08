using Godot;
using System;

public partial class Wallriding : PlayerState
{
	private float wallrideSpeed = 8f;
	private float wallDecel = 8f;
	private float currentWallSpeed;
	private float exitLookBoost = 8f;	
	private float exitWallBoost = 4f;

	public override void Enter(string prevState)
	{
		Vector3 wallNormal = player.GetWallNormal();
		Vector3 currentVelocity = player.Velocity;

		// Force the player's velocity to be parallel to the wall
		Vector3 velocityAwayFromWall = currentVelocity.Project(wallNormal);
		currentVelocity -= velocityAwayFromWall;
		currentVelocity.Y = 0;

		currentWallSpeed = currentVelocity.Length();

		// If the player is looking the opposite direction, flip their velocity and apply a slowdown
		Vector3 playerLookDir = -player.head.Transform.Basis.Z;
		Vector3 lookDirAwayFromWall = playerLookDir.Project(wallNormal);
		playerLookDir -= lookDirAwayFromWall;
		playerLookDir.Y = 0;

		if (playerLookDir.Dot(currentVelocity) < 0)
		{
			currentVelocity *= -1;
			currentVelocity = currentVelocity.Normalized() * wallrideSpeed;
		}

		player.Velocity = currentVelocity;

	}

	public override void HandleInput(InputEvent @event) 
	{
		if (Input.IsActionJustReleased("jump"))
		{
			// give player a boost of speed in the direction they are looking and away from the wall
			player.Velocity += -player.head.Transform.Basis.Z * exitLookBoost;
			player.Velocity += Vector3.Up * exitWallBoost;

			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta) 
	{ 
		// Wallriding is similar to sliding
		// The player rides on the wall at their current speed, but will be slowed down
		// to a set speed over time. Jumping off the wall gives them a boost of speed
		// in their look direction
		
		Vector3 wallNormal = player.GetWallNormal();
		Vector3 newVelocity = player.Velocity;

		// Player can only move horizontally along the wall
		Vector3 velocityAwayFromWall = newVelocity.Project(wallNormal);
		newVelocity -= velocityAwayFromWall;
		newVelocity.Y = 0;

		// Decellerate/accellerate the player towards the wall riding speed
		currentWallSpeed = Mathf.MoveToward(currentWallSpeed, wallrideSpeed, wallDecel * (float)delta);
		newVelocity = newVelocity.Normalized() * currentWallSpeed;

		player.Velocity = newVelocity;
		player.MoveAndSlide();

		if (!player.IsOnWall())
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
