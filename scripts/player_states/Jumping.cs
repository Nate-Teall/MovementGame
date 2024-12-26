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

		// Allow for a little bit of side-to-side air control
		// Once again, I am not sure how much control I want the player to have mid-air. Will revisit this
		/*Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (player.head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction.X != 0)
			newVelocity.X = Mathf.MoveToward(newVelocity.X, direction.X * player.airSpeed, player.airAccel * (float)delta);

		if (direction.Z != 0)
			newVelocity.Z = Mathf.MoveToward(newVelocity.Z, direction.Z * player.airSpeed, player.airAccel * (float)delta);
		*/

		player.Velocity = newVelocity;
		player.MoveAndSlide();

		if (player.Velocity.Y < 0)
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
