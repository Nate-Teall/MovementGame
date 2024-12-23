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

		player.Velocity += player.GetGravity() * (float)delta;

		player.MoveAndSlide();

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");

		if (player.IsOnFloor())
		{
			if (inputDir == Vector2.Zero)
			{
				EmitSignal(SignalName.Finished, IDLE);
			}
			else if (Input.IsActionPressed("sprint") && inputDir.Y <= 0)
			{
				EmitSignal(SignalName.Finished, SPRINTING);
			}
			else
			{
				EmitSignal(SignalName.Finished, WALKING);
			}
		}
	}

	public override void Exit() { }
}
