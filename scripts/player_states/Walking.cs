using Godot;
using System;

public partial class Walking : PlayerState
{
	public override void Enter(string prevState) { }

	public override void HandleInput(InputEvent @event) 
	{ 
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");

		if (Input.IsActionJustPressed("jump"))
		{
			EmitSignal(SignalName.Finished, JUMPING);
		}
		else if (Input.IsActionJustPressed("sprint"))
		{
			if (inputDir.Y <= 0)
				EmitSignal(SignalName.Finished, SPRINTING);
		}
		else if (Input.IsActionJustPressed("crouch"))
		{
			EmitSignal(SignalName.Finished, CROUCHING);
		}
	}

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta) 
	{
		player.Velocity = moveInDirection(player.walkSpeed);
		player.MoveAndSlide();

		if (player.Velocity == Vector3.Zero)
		{
			EmitSignal(SignalName.Finished, IDLE);	
		}
		else if (!player.IsOnFloor())
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
