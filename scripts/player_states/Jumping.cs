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
		player.Velocity += player.GetGravity() * (float)delta;

		player.MoveAndSlide();

		if (player.Velocity.Y < 0)
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
