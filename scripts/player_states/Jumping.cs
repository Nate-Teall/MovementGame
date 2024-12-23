using Godot;
using Godot.Collections;
using System;

public partial class Jumping : PlayerState
{
	private float strafeSpeed = 2;
	public override void Enter(string prevState)
	{
		Vector3 newVelocity = player.Velocity;
		newVelocity.Y = player.jumpVelocity;
		player.Velocity = newVelocity;
	}

	public override void HandleInput(InputEvent @event) { }

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta)
	{
		player.Velocity += player.GetGravity() * (float)delta;

		// Allow for a little bit of side-to-side air control
		/*Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (player.head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			// Rotate around the up axis (left-right)
			//float rotationAmt = strafeSpeed * (float)delta * direction.X;
			//newVelocity = newVelocity.Rotated(player.head.Transform.Basis.Y, -rotationAmt);
		} */

		player.MoveAndSlide();

		if (player.Velocity.Y < 0)
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
