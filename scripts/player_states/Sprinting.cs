using Godot;
using System;

public partial class Sprinting : PlayerState
{
	public override void Enter(string prevState) { }

	public override void HandleInput(InputEvent @event) 
	{ 
		if (Input.IsActionJustReleased("sprint"))
		{
			EmitSignal(SignalName.Finished, WALKING);
		}
		else if (Input.IsActionJustPressed("jump"))
		{
			EmitSignal(SignalName.Finished, JUMPING);
		}
		else if (Input.IsActionJustPressed("crouch"))
		{
			EmitSignal(SignalName.Finished, SLIDING);
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

		// Weird observation: the length of this new velocity is always less than the SprintSpeed (between ~14 and ~16 if SprintSpeed is 16)
		// 	This occurs ONLY when moving forward and back. It is EXACTLY equal to SprintSpeed when moving L/R
		//	Why..? Could it be floating point error?
		//GD.Print("playerVelLen: " + player.Velocity.Length());
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
