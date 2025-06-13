using Godot;
using Godot.Collections;
using System;

public partial class Jumping : PlayerState
{
	//private float strafeSpeed = 2;
	public override void Enter(string prevState)
	{
		// Change velocity to the facing direction
		// Have to decide which way I want slide-jumping to work. For now this remains disabled
		/*Vector3 facingDirection = -player.head.Transform.Basis.Z;
		float currentSpeed = player.Velocity.Length();
		newVelocity.X = facingDirection.X;
		newVelocity.Z = facingDirection.Z;
		newVelocity *= currentSpeed;*/

		// Jump impulse
		player.Velocity += new Vector3(0, player.jumpVelocity, 0);
	}

	public override void HandleInput(InputEvent @event) 
	{ 
		if (player.IsOnWall() && Input.IsActionPressed("jump"))
		{
			EmitSignal(SignalName.Finished, WALLRIDING);
		}
	}

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta)
	{
		Vector3 newVelocity = player.Velocity;
		newVelocity += player.GetGravity() * (float)delta;

		// move around in the air
		Vector3 moveDirection = GetGlobalInputDirection();
		newVelocity += moveDirection * player.airAccel * (float)delta;

		player.Velocity = newVelocity;
		player.MoveAndSlide();

		if (player.Velocity.Y < 0)
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
