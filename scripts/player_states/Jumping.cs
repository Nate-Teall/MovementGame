using Godot;
using Godot.Collections;
using System;

public partial class Jumping : PlayerState
{
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

		if (player.Velocity.Y < 0)
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
