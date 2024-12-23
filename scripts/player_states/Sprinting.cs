using Godot;
using System;

public partial class Sprinting : PlayerState
{
	public override void Enter(string prevState) { }

	public override void HandleInput(InputEvent @event) 
	{ 
		if (!Input.IsActionPressed("sprint"))
		{
			EmitSignal(SignalName.Finished, WALKING);
		}
		else if (Input.IsActionJustPressed("jump"))
		{
			EmitSignal(SignalName.Finished, JUMPING);
		}
	}

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta) 
	{ 
		Vector3 newVelocity = player.Velocity;

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (player.head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			newVelocity.X = direction.X * player.sprintSpeed;
			newVelocity.Z = direction.Z * player.sprintSpeed;
		}
		else
		{
			newVelocity.X = 0;
			newVelocity.Z = 0;
		}

		player.Velocity = newVelocity;
		player.MoveAndSlide();

		if (player.Velocity == Vector3.Zero)
		{
			EmitSignal(SignalName.Finished, IDLE);	
		}
		else if (!player.IsOnFloor() && player.Velocity.Y < 0)
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
