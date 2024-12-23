using Godot;
using Godot.Collections;
using System;

public partial class Falling : PlayerState
{
	public override void Enter(string prevState) { }

	public override void HandleInput(InputEvent @event) { }

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta)
	{
		Vector3 newVelocity = player.Velocity;

		newVelocity += player.GetGravity() * (float)delta;

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (player.head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			newVelocity.X = direction.X * player.airSpeed;
			newVelocity.Z = direction.Z * player.airSpeed;
		}
		else
		{
			newVelocity.X = Mathf.MoveToward(player.Velocity.X, 0, player.airSpeed);
			newVelocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, player.airSpeed);
		}

		player.Velocity = newVelocity;
		player.MoveAndSlide();

		if (player.IsOnFloor())
		{
			if (inputDir == Vector2.Zero)
				EmitSignal(SignalName.Finished, IDLE);
			else
				EmitSignal(SignalName.Finished, WALKING);
		}
	}

	public override void Exit() { }
}
