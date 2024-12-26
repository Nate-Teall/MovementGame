using Godot;
using System;

public partial class Wallriding : PlayerState
{
	public override void Enter(string prevState) { }

	public override void HandleInput(InputEvent @event) 
	{ 
		if (Input.IsActionJustReleased("jump"))
		{
			// Also, give player a boost of speed in the direction they are looking

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

		// Remove the component of the player's velocity that is perpendicular to the wall
		Vector3 velocityAwayFromWall = newVelocty.Project(wallNormal);
		newVelocty -= velocityAwayFromWall;

		// Player can only move horizontally along the wall
		// Note: Lucio wallriding does bring the player upwards for a moment at the beginning
		if ( Mathf.Abs(newVelocty.Y) > 4f )
		{
			newVelocty.Y = 4f * Mathf.Sign(newVelocty.Y);
		}
		
		// Set the player's speed on the wall to a minimum.
		// If it is above the minimum, decellerate the player towards it.
		if (newVelocty.Length() < player.minWallSpeed)
		{
			//newVelocty = newVelocty.Normalized() * player.minWallSpeed;
		}

		player.Velocity = newVelocty;
		player.MoveAndSlide();

		if (!player.IsOnWall())
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
