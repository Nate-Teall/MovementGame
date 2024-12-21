using Godot;
using System;

public partial class Walking : PlayerState
{
	public override void Enter(string prevState) { }

	public override void HandleInput(InputEvent @event) 
	{ 
		if (player.IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			EmitSignal(SignalName.Finished, JUMPING);
		}
	}

	public override void Update(double delta) { }

	public override void PhysicsUpdate(double delta) 
	{
		Vector3 newVelocity = player.Velocity;

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (player.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			newVelocity.X = direction.X * player.walkSpeed;
			newVelocity.Z = direction.Z * player.walkSpeed;
		}
		else
		{
			newVelocity.X = Mathf.MoveToward(player.Velocity.X, 0, player.walkSpeed);
			newVelocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, player.walkSpeed);
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
