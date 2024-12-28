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
			// If moving fast enough, the player can slide on landing
			if (Input.IsActionPressed("crouch"))
			{
				if (player.Velocity.Length() >= player.walkSpeed)
					EmitSignal(SignalName.Finished, SLIDING);
				else
					EmitSignal(SignalName.Finished, CROUCHING);
			}
			else if (inputDir == Vector2.Zero)
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
		else if (player.IsOnWall() && Input.IsActionPressed("jump"))
		{
			EmitSignal(SignalName.Finished, WALLRIDING);
		}
	}

	public override void Exit() { }
}
