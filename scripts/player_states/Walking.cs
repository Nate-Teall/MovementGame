using Godot;
using System;

public partial class Walking : PlayerState
{
	public override void Enter(string prevState) { }

	public override void HandleInput(InputEvent @event) 
	{ 
		if (Input.IsActionJustPressed("jump"))
		{
			EmitSignal(SignalName.Finished, JUMPING);
		}
		else if (Input.IsActionJustPressed("sprint"))
		{
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
		Vector3 newVelocity = player.Velocity;

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (player.head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			// Accelerate towards the max walking speed, in the correct direction
			//newVelocity.X = Mathf.MoveToward(player.Velocity.X, direction.X * player.walkSpeed, player.walkAccel * (float)delta);
			//newVelocity.Z = Mathf.MoveToward(player.Velocity.Z, direction.Z * player.walkSpeed, player.walkAccel * (float)delta);

			newVelocity.X = direction.X * player.walkSpeed;
			newVelocity.Z = direction.Z * player.walkSpeed;
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
		else if (!player.IsOnFloor())
		{
			EmitSignal(SignalName.Finished, FALLING);
		}
	}

	public override void Exit() { }
}
