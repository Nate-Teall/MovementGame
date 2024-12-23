using Godot;
using System;

public partial class Sliding : PlayerState
{
	private float slideSpeed;
	private Vector3 facingDirection;

	public override void Enter(string prevState) 
	{ 
		// Get the player's current speed and facing direction
		// NOTE: If sliding immediately out of sprint, the velocity won't be exactly sprint speed.
		//		 See  comment in Sprinting.PhysicsUpdate();
		slideSpeed = player.Velocity.Length();
		facingDirection = -player.head.Transform.Basis.Z.Normalized();

		GD.Print("SprintSpeed: " + slideSpeed + " VelMagnitude: " + player.Velocity.Length());

		Vector3 newHeadPos = player.head.Position;
		newHeadPos.Y *= Player.crouchHeightScale;
		player.head.Position = newHeadPos;
	}

	public override void HandleInput(InputEvent @event) { }

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta) 
	{ 
		// Slowly decellerate until stopped, then get up
		Vector3 newVelocity = player.Velocity;

		// Component of velocity that is parallel to forward
		// Projection onto forward of Velocity
		//Vector3 forwardVel = newVelocity.Project(forward);
		//GD.Print("PlayerVel: " + newVelocity.Length() + " Forward Vel: " + forwardVel.Length());

		//newVelocity.X = Mathf.MoveToward(player.Velocity.X, 0, player.slideDecel * (float)delta);
		//newVelocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, player.slideDecel * (float)delta);

		newVelocity.X = facingDirection.X * slideSpeed;
		newVelocity.Z = facingDirection.Z * slideSpeed;

		// Each frame, decellerate the player until they are stopped
		slideSpeed = Mathf.MoveToward(slideSpeed, 0, player.slideDecel * (float)delta);

		player.Velocity = newVelocity;
		player.MoveAndSlide();

		if (slideSpeed <= 0)
		{
			if (Input.IsActionPressed("crouch"))
			{
				EmitSignal(SignalName.Finished, CROUCHING);
			}
			else if (Input.GetVector("left", "right", "forward", "back") != Vector2.Zero)
			{
				string nextState = Input.IsActionPressed("sprint") ? SPRINTING : WALKING;
				EmitSignal(SignalName.Finished, nextState);
			}
			else
			{
				EmitSignal(SignalName.Finished, IDLE);
			}
		}
	}

	public override void Exit() 
	{ 
		Vector3 newHeadPos = player.head.Position;
		newHeadPos.Y *= 1 / Player.crouchHeightScale;
		player.head.Position = newHeadPos;
	}
}
