using Godot;
using System;

public partial class Wallriding : PlayerState
{
	private float wallrideSpeed = 8f;
	private float wallDecel = 8f;
	private float currentWallSpeed;
	private float exitLookBoost = 4f;	
	private float exitWallBoost = 4f;

	public override void Enter(string prevState) 
	{
		Vector3 wallNormal = player.GetWallNormal();
		Vector3 currentVelocity = player.Velocity;

		Vector3 velocityAwayFromWall = currentVelocity.Project(wallNormal);
		currentVelocity -= velocityAwayFromWall;
		currentVelocity.Y = 0;

		currentWallSpeed = currentVelocity.Length();
	}

	public override void HandleInput(InputEvent @event) 
	{ 
		if (Input.IsActionJustReleased("jump"))
		{
			// give player a boost of speed in the direction they are looking away from the wall
			player.Velocity += -player.head.Transform.Basis.Z * exitLookBoost;
			player.Velocity += player.GetWallNormal() * exitWallBoost; 

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
		Vector3 newVelocty = player.Velocity;

		// Player can only move horizontally along the wall
		Vector3 velocityAwayFromWall = newVelocty.Project(wallNormal);
		newVelocty -= velocityAwayFromWall;
		newVelocty.Y = 0;

		// Decellerate/accellerate the player towards the wall riding speed
		currentWallSpeed = Mathf.MoveToward(currentWallSpeed, wallrideSpeed, wallDecel * (float)delta);
		newVelocty = newVelocty.Normalized() * currentWallSpeed;

		player.Velocity = newVelocty;
		player.MoveAndSlide();

		if (!player.IsOnWall())
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
